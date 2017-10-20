using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Newtonsoft.Json;
using WotDossier.Common.Extensions;
using WotDossier.Common.Python;
using WotDossier.Domain.Dossier.Processor;
using WotDossier.Domain.Dossier.Utils;
using WotDossier.Domain.Tank;

namespace WotDossier.Domain.Dossier
{
    public static class DossierReader
    {
        public static List<TankJson> Read(FileInfo cacheFile)
        {
            var result = new List<TankJson>();
            using (Unpickler unpickler = new Unpickler())
            {
                object[] pickle = (object[])unpickler.load(cacheFile.OpenRead());
                var dossierversion = pickle[0].ToString();
                try
                {
                    var s = Encoding.UTF8.GetString(Base32.FromBase32String(Path.GetFileNameWithoutExtension(cacheFile.Name)));
                    var arr = s.Split(';');
                    var server = arr[0];
                    var username = arr[1];
                }
                catch
                {
                    // ignored
                }
                var layoutList =
                    XDocument.Load(
                        typeof(DossierReader).Assembly.GetManifestResourceStream(
                            $"{typeof(DossierReader).Namespace}.vehicleDossierLayout.xml"));
                
                Hashtable tankItems = (Hashtable)pickle[1];
                foreach (DictionaryEntry tankItem in tankItems)
                {
                    var data = ((string)((object[])tankItem.Value)[1]).Select(Convert.ToByte).ToArray();

                    var tankversion = GetIntValue(data, 0, "B");

                    if (!(tankItem.Key is object[] tankDescr))
                    {
                        Console.WriteLine("Invalid tankdata");
                        continue;
                    }
                    var compDescr = Convert.ToInt32(tankDescr[1]);
                    var layout = layoutList.Root.Elements("vehicleDossierLayout")
                        .FirstOrDefault(e => Convert.ToInt32(e.Attribute("version").Value) == tankversion);

                    if (layout == null)
                    {
                        Console.WriteLine($"Invalid tankversion{tankversion}");
                        continue;
                    }

                    var tankData = (layout.Attribute("dossierVersion").Value == "1")
                        ? ProcessTank(data, layout)
                        : ProcessTankV2(data, layout);

                    tankData.updated = Convert.ToInt32(((object[])tankItem.Value)[0]);
                    tankData.compDescr = compDescr;
                    tankData.basedonversion = tankversion;
                    
                    var item = TankDataProcessor.GetProcessor(tankversion)(tankData);
                    item.Raw = CompressHelper.Compress(JsonConvert.SerializeObject(item));
                    result.Add(item);
                }
                return result;
            }
        }

        private static DossierTankData ProcessTankV2(byte[] data, XElement layout)
        {
            var result = new DossierTankData();

            var blocks = layout.Elements().ToList();
            var offset = (blocks.Count + 1) * 2;
            var blockSizes = data.Unpack($"<{blocks.Count + 1}h").Skip(1).Select(Convert.ToInt32).ToArray();
            if (blocks.Count != blockSizes.Length)
            {
                Console.WriteLine($"Invalid block sizes");
                return result;
            }
            for (var i = 0; i < blocks.Count; i++)
            {
                if (blockSizes[i] == 0) continue;

                var block = blocks[i];
                

                if (block.Attribute("blockBuilder").Value == "StaticSize")
                {
                    result.StaticSize.Add(block.Name.LocalName, ProcessStaticSizeBlock(data, block, offset));
                }
                else if (block.Attribute("blockBuilder").Value == "Dict")
                {
                    result.Dict.Add(block.Name.LocalName, ProcessDictBlock(data, block, offset, blockSizes[i]));
                }
                else if (block.Attribute("blockBuilder").Value == "List")
                {
                    result.List.Add(block.Name.LocalName, ProcessListBlock(data, block, offset, blockSizes[i]));
                }
                else if (block.Attribute("blockBuilder").Value == "Binary")
                {
                    var bt = data.Unpack($"{blockSizes[i]}B", offset).Cast<byte>().ToArray();

                    result.Binary.Add(block.Name.LocalName, bt);
                }
                offset += blockSizes[i];
            }
            return result;
        }

        private static DossierTankData ProcessTank(byte[] data, XElement layout)
        {
            var result = new DossierTankData();
            var baseoffset = 0;
            foreach (var block in layout.Elements())
            {
                if (block.Attribute("blockBuilder").Value == "StaticSize")
                    result.StaticSize.Add(block.Name.LocalName, ProcessStaticSizeBlock(data, block, baseoffset));
                else if (block.Attribute("blockBuilder").Value == "Dict")
                {
                    var offsetAttr = block.Attributes("offset").FirstOrDefault();
                    var offset = baseoffset;
                    if (offsetAttr != null)
                    {
                        offset += Convert.ToInt32(offsetAttr.Value) + 2;
                    }
                    var numfrags = GetIntValue(data, offset, "H");
                    offset += 2;
                    var fragslist = new Dictionary<object,object>();
                    result.Dict.Add(block.Name.LocalName, fragslist);
                    if (numfrags > 0)
                    {
                        for (var i = 0; i < numfrags; i++)
                        {
                            var tankoffset = offset + i * 4;
                            var fragoffset = offset + numfrags * 4 + i * 2;
                            var id = GetIntValue(data, tankoffset, "I");
                            var amount = GetIntValue(data, fragoffset, "H");
                            fragslist.Add(id, amount);
                        }
                    }
                }
            }
            return result;
        }

        private static Dictionary<string, int> ProcessStaticSizeBlock(byte[] data, XElement block, int baseoffset)
        {
            var dictionary = new Dictionary<string, int>();
            var offset = baseoffset;
            foreach (var element in block.Elements())
            {
                var fmt = element.Attribute("valueFormat").Value;
                var sz = GetSize(fmt);
                if(offset + sz > data.Length)
                    break;
                dictionary.Add(element.Name.LocalName, data.GetIntValue(offset, fmt));
                offset += sz;
            }
            return dictionary;
        }

        private static Dictionary<object, object> ProcessDictBlock(byte[] data, XElement block, int baseoffset, int blockSize)
        {
            var result = new Dictionary<object, object>();
            var keyFormat = block.Attribute("keyFormat").Value;
            var valueFormat = block.Attribute("valueFormat").Value;
            var keySize = ByteArrayExtensions.CalcSize(keyFormat);
            var valueSize = ByteArrayExtensions.CalcSize(valueFormat);
            var itemSize = keySize + valueSize;
            if (itemSize == 0)
            {
                Console.WriteLine("Invalid dictionary item formats");
                return result;
            }
            var length = blockSize / itemSize;
            if (length == 0)
                return result;
            for (var i = 0; i < length; i++)
            {
                var key = data.Unpack(keyFormat, baseoffset + i * itemSize);
                var value = data.Unpack(valueFormat, baseoffset + i * itemSize + keySize);
                if (key.Length == 1 && value.Length == 1)
                    result.Add(key[0], value[0]);
                else if (key.Length == 1 && value.Length != 1)
                    result.Add(key[0], value);
                else if (key.Length != 1 && value.Length == 1)
                    result.Add(key, value[0]);
                else
                    result.Add(key, value);
            }
            return result;
        }

        private static List<object> ProcessListBlock(byte[] data, XElement block, int baseoffset, int blockSize)
        {
            var result = new List<object>();
            var valueFormat = block.Attribute("itemFormat").Value;
            var valueSize = ByteArrayExtensions.CalcSize(valueFormat);
            if (valueSize == 0)
            {
                Console.WriteLine("Invalid list item formats");
                return result;
            }
            var length = blockSize / valueSize;
            if (length == 0)
                return result;
            for (var i = 0; i < length; i++)
            {
                var value = data.Unpack(valueFormat, baseoffset + i * valueSize);
                result.Add(value.Length == 1 ? value[0] : value);
            }
            return result;
        }

        private static int GetSize(string fmt)
        {
            switch (fmt)
            {
                case "H":
                case "h":
                    return 2;
                case "I":
                case "i":
                    return 4;
                case "B":
                case "b":
                    return 1;
            }
            return ByteArrayExtensions.CalcSize(fmt);
        }

        private static int GetIntValue(this byte[] data, int startoffset, string fmt)
        {
            var value = data.Unpack("<" + fmt, startoffset);

            return Convert.ToInt32(value[0]);
        }
    }
}

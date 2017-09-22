using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Common.Extensions;
using WotDossier.Common.Python;
using WotDossier.Domain.Dossier.Utils;

namespace WotDossier.Domain.Dossier
{
    public static class DossierReader
    {
        public static DossierData Read(FileInfo cacheFile)
        {
            var result = new DossierData();
            using (Unpickler unpickler = new Unpickler())
            {
                object[] pickle = (object[])unpickler.load(cacheFile.OpenRead());
                result.header.dossierversion = pickle[0].ToString();
                try
                {
                    var s = Encoding.UTF8.GetString(Base32.FromBase32String(Path.GetFileNameWithoutExtension(cacheFile.Name)));
                    var arr = s.Split(';');
                    result.header.server = arr[0];
                    result.header.username = arr[1];
                }
                catch
                {
                    // ignored
                }
                Hashtable tankItems = (Hashtable)pickle[1];
                foreach (DictionaryEntry tankItem in tankItems)
                {
                    //string data = (string)((object[])tankItem.Value)[1];
                    var data = Encoding.UTF8.GetBytes((string)((object[])tankItem.Value)[1]);

                    var tankstruct = data.Length + "B";
                    var tupledata = data.Unpack(tankstruct);
                    var tankversion = Convert.ToInt32(data.GetData(0, 1));
                }
                return null;
            }
        }

        private static object GetData(this byte[] data, int startoffset, int offsetlength)
        {
            if (data.Length < startoffset + offsetlength)
                return 0;

            var structformat = "H";

            if (offsetlength == 1)
                structformat = "B";

            if (offsetlength == 2)
                structformat = "H";

            if (offsetlength == 4)
                structformat = "I";

            var value = data.Unpack(structformat, startoffset);

            return value;
        }

    }
}

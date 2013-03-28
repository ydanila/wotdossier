﻿using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace WotDossier.Dal
{
    public class WotApiHelper
    {
        public static string GetCountryNameCode(int countryid)
        {
            switch (countryid)
            {
                case 0:
                    return "ussr";
                case 1:
                    return "germany";
                case 2:
                    return "usa";
                case 3:
                    return "china";
                case 4:
                    return "france";
                case 5:
                    return "uk";
            }
            return string.Empty;
        }

        public static byte[] Zip(string value)
        {
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionMode.Compress))
                using (var writer = new StreamWriter(zip, Encoding.UTF8))
                {
                    writer.Write(value);
                }
                return ms.ToArray();
            }
        }

        public static string UnZip(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            using (var zip = new GZipStream(ms, CompressionMode.Decompress))
            using (var sr = new StreamReader(zip, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public static T UnZipObject<T>(byte[] byteArray)
        {
            string json = UnZip(byteArray);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
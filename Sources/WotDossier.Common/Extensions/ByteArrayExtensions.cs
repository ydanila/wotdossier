using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WotDossier.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        public static void Dump(this byte[] array, string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                fileStream.Write(array, 0, array.Length);
                fileStream.Flush();
            }
        }

        public static void Dump(this string array, string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                TextWriter writer = new StreamWriter(fileStream);
                writer.Write(array);
                writer.Flush();
            }
        }

        public static float ToSingle(this byte[] array)
        {
            return BitConverter.ToSingle(array, 0);
        }

        public static ulong ConvertLittleEndian(this byte[] array)
        {
            int pos = 0;
            ulong result = 0;
            foreach (byte by in array)
            {
                result |= (ulong)(by << pos);
                pos += 8;
            }
            return result;
        }

        public static string GetUtf8String(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string GetAsciiString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
		/*
        public static List<int> Unpack(this byte[] array, string format)
        {
            List<int> result = new List<int>();
            using (var stream = new MemoryStream(array))
            {
                for (int i = 0; i < format.Length; i++)
                {
                    switch (format[i])
                    {
                        case 'B':
                            var bytes1 = stream.Read(1);
                            result.Add((int)bytes1.ConvertLittleEndian());
                            break;
                        case 'H':
                            var bytes = stream.Read(2);
                            result.Add((int)bytes.ConvertLittleEndian());
                            break;
                        case 'Q':
                            var read = stream.Read(4);
                            result.Add((int)read.ConvertLittleEndian());
                            break;
                    }
                }
            }
            return result;
        }
        */

		public static bool Compare(this byte[] source, byte[] target)
		{
			if (source == null && target == null) return true;
			if (source == null || target == null) return false;
			if (source.Length != target.Length) return false;
			return !source.Where((t, i) => t != target[i]).Any();
		}

		private static List<(char Type, int Length)> ProcessFormat(string fmt)
        {
            // First we parse the format string to make sure it's proper.
            if (fmt.Length < 1) throw new ArgumentException("Format string cannot be empty.");

            bool endianFlip = false;
            if (fmt.Substring(0, 1) == "<")
            {
                // Little endian.
                // Do we need to flip endianness?
                if (BitConverter.IsLittleEndian == false) endianFlip = true;
                fmt = fmt.Substring(1);
            }
            else if (fmt.Substring(0, 1) == ">")
            {
                // Big endian.
                // Do we need to flip endianness?
                if (BitConverter.IsLittleEndian == true) endianFlip = true;
                fmt = fmt.Substring(1);
            }
            var validChars = new char[]
                {'x', 'c', 'b', 'B', '?', 'h', 'H', 'i', 'I', 'l', 'L', 'q', 'Q', 'f', 'd', 's', 'p', 'P'};

            var formats = new List<(char Type, int Length)>();

            var curlen = "";
            foreach (var c in fmt)
            {
                if (c >= '0' & c <= '9')
                {
                    curlen += c;
                    continue;
                }
                if (!validChars.Contains(c))
                    throw new ArgumentException("Invalid character found in format string.");
                var len = 1;
                if (!string.IsNullOrEmpty(curlen))
                {
                    len = Convert.ToInt32(curlen);
                    curlen = String.Empty;
                }

                formats.Add((c, len));
            }
            return formats;
        }

        /// <summary>
        /// Convert a byte array into an array of objects based on Python's "struct.unpack" protocol.
        /// </summary>
        /// <param name="fmt">A "struct.pack"-compatible format string</param>
        /// <param name="bytes">An array of bytes to convert to objects</param>
        /// <param name="startoffset">offset to convert to objects</param>
        /// <returns>Array of objects.</returns>
        /// <remarks>You are responsible for casting the objects in the array back to their proper types.</remarks>
        public static object[] Unpack(this byte[] bytes, string fmt, int startoffset = 0)
        {
            var formats = ProcessFormat(fmt);
            
            // Ok, we can go ahead and start parsing bytes!
            var byteArrayPosition = startoffset;
            var outputList = new List<object>();
            foreach (var format in formats)
            {
                if (format.Type == 's')
                {
                    outputList.Add(GetString(format.Length));
                    byteArrayPosition += format.Length;
                    continue;
                }
                if(format.Type == 'p' || format.Type == 'P')
                    throw new ArgumentException("NOT SUPPORTED character found in format string.");

                for (var i = 0; i < format.Length; i++)
                {
                    (object value, int offset ) = GetValue(format.Type, byteArrayPosition);

                    if (value != null)
                        outputList.Add(value);

                    byteArrayPosition += offset;
                }
            }
            return outputList.ToArray();

            string GetString(int length)
            {
                var buf = new byte[length];
                Array.Copy(bytes, byteArrayPosition, buf, 0, length);
                return Encoding.UTF8.GetString(buf);
            }

            (object, int) GetValue(char format, int position)
            {
                var buf = new byte[1];
                switch (format)
                {
                    case 'q':
                        return (BitConverter.ToInt64(bytes, byteArrayPosition), 8);
                    case 'Q':
                        return (BitConverter.ToUInt64(bytes, byteArrayPosition), 8);
                    case 'l':
                    case 'i':
                        return (BitConverter.ToInt32(bytes, byteArrayPosition), 4);
                    case 'L':
                    case 'I':
                        return (BitConverter.ToUInt32(bytes, byteArrayPosition), 4);
                    case 'h':
                        return (BitConverter.ToInt16(bytes, byteArrayPosition), 2);
                    case 'H':
                        return (BitConverter.ToUInt16(bytes, byteArrayPosition), 2);
                    case 'f':
                        return (BitConverter.ToSingle(bytes, byteArrayPosition), 4);
                    case 'd':
                        return (BitConverter.ToDouble(bytes, byteArrayPosition), 8);
                    case 'b':
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        return ((sbyte) buf[0], 1);
                    case 'c':
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        return ((char)buf[0], 1);
                    case 'B':
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        return ((byte) buf[0], 1);
                    case 'x':
                        return (null, 1);
                }
                throw new ArgumentException("NOT SUPPORTED character found in format string.");
            }
        }

        public static int CalcSize(string fmt)
        {
            return ProcessFormat(fmt).Sum(tp => GetSize(tp.Type) * tp.Length);

            int GetSize(char format)
            {
                switch (format)
                {
                    case 'q':
                    case 'Q':
                        return 8;
                    case 'l':
                    case 'i':
                    case 'L':
                    case 'I':
                        return 4;
                    case 'h':
                    case 'H':
                        return 2;
                    case 'f':
                        return 4;
                    case 'd':
                        return 8;
                    case 'b':
                    case 'c':
                    case 'B':
                    case 'x':
                        return 1;
                }
                throw new ArgumentException("NOT SUPPORTED character found in format string.");
            }
        }

        public static byte[] Xor(byte[] result, byte[] matchValue)
        {
            if (result.Length == 0)
            {
                return matchValue;
            }

            byte[] newResult = new byte[matchValue.Length > result.Length ? matchValue.Length : result.Length];

            for (int i = 1; i < newResult.Length + 1; i++)
            {
                //Use XOR on the LSBs until we run out
                if (i > result.Length)
                {
                    newResult[newResult.Length - i] = matchValue[matchValue.Length - i];
                }
                else if (i > matchValue.Length)
                {
                    newResult[newResult.Length - i] = result[result.Length - i];
                }
                else
                {
                    newResult[newResult.Length - i] =
                        (byte)(matchValue[matchValue.Length - i] ^ result[result.Length - i]);
                }
            }
            return newResult;
        }
    }
}
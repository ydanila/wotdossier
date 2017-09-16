using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Convert a byte array into an array of objects based on Python's "struct.unpack" protocol.
        /// </summary>
        /// <param name="fmt">A "struct.pack"-compatible format string</param>
        /// <param name="bytes">An array of bytes to convert to objects</param>
        /// <returns>Array of objects.</returns>
        /// <remarks>You are responsible for casting the objects in the array back to their proper types.</remarks>
        public static object[] Unpack(this byte[] bytes, string fmt)
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

            // Now, we find out how long the byte array needs to be
            int totalByteLength = 0;
            foreach (char c in fmt.ToCharArray())
            {
                switch (c)
                {
                    case 'q':
                    case 'Q':
                        totalByteLength += 8;
                        break;
                    case 'i':
                    case 'I':
                        totalByteLength += 4;
                        break;
                    case 'h':
                    case 'H':
                        totalByteLength += 2;
                        break;
                    case 'b':
                    case 'B':
                    case 'x':
                        totalByteLength += 1;
                        break;
                    default:
                        throw new ArgumentException("Invalid character found in format string.");
                }
            }

            // Test the byte array length to see if it contains as many bytes as is needed for the string.
            if (bytes.Length != totalByteLength) throw new ArgumentException("The number of bytes provided does not match the total length of the format string.");

            // Ok, we can go ahead and start parsing bytes!
            int byteArrayPosition = 0;
            List<object> outputList = new List<object>();
            byte[] buf;

            foreach (char c in fmt.ToCharArray())
            {
                switch (c)
                {
                    case 'q':
                        outputList.Add((object)(long)BitConverter.ToInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        break;
                    case 'Q':
                        outputList.Add((object)(ulong)BitConverter.ToUInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        break;
                    case 'l':
                        outputList.Add((object)(int)BitConverter.ToInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        break;
                    case 'L':
                        outputList.Add((object)(uint)BitConverter.ToUInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        break;
                    case 'h':
                        outputList.Add((object)(short)BitConverter.ToInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        break;
                    case 'H':
                        outputList.Add((object)(ushort)BitConverter.ToUInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        break;
                    case 'b':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((object)(sbyte)buf[0]);
                        byteArrayPosition++;
                        break;
                    case 'B':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((object)(byte)buf[0]);
                        byteArrayPosition++;
                        break;
                    case 'x':
                        byteArrayPosition++;
                        break;
                    default:
                        throw new ArgumentException("You should not be here.");
                }
            }
            return outputList.ToArray();
        }

        /// <summary>
        /// Convert a byte array into an array of objects based on Python's "struct.unpack" protocol.
        /// </summary>
        /// <param name="fmt">A "struct.pack"-compatible format string</param>
        /// <param name="bytes">An array of bytes to convert to objects</param>
        /// <param name="startoffset">offset to convert to objects</param>
        /// <returns>Array of objects.</returns>
        /// <remarks>You are responsible for casting the objects in the array back to their proper types.</remarks>
        public static object[] UnpackFrom(this byte[] bytes, string fmt, int startoffset)
        {
            var tgt = new byte[bytes.Length - startoffset];
            bytes.CopyTo(tgt, startoffset);
            return tgt.Unpack(fmt);
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
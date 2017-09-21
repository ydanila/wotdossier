using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WotDossier.Domain.Dossier.Utils
{
    //public class Base32
    //{
    //    private const string DEF_ENCODING_TABLE = "abcdefghijklmnopqrstuvwxyz234567";
    //    private const char DEF_PADDING = '=';

    //    private readonly string eTable; //Encoding table
    //    private readonly char padding;
    //    private readonly byte[] dTable; //Decoding table

    //    public Base32() : this(DEF_ENCODING_TABLE, DEF_PADDING) { }
    //    public Base32(char padding) : this(DEF_ENCODING_TABLE, padding) { }
    //    public Base32(string encodingTable) : this(encodingTable, DEF_PADDING) { }

    //    public Base32(string encodingTable, char padding)
    //    {
    //        this.eTable = encodingTable;
    //        this.padding = padding;
    //        dTable = new byte[0x80];
    //        InitialiseDecodingTable();
    //    }

    //    /// <summary>
    //    /// Converts a Base32-k string into an array of bytes.
    //    /// </summary>
    //    /// <exception cref="System.ArgumentException">
    //    /// Input string <paramref name="s">s</paramref> contains invalid Base32-k characters.
    //    /// </exception>
    //    public static byte[] FromBase32String(string str)
    //    {
    //        return new Base32().Decode(str);
    //    }

    //    /// <summary>
    //    /// Converts an array of bytes to a Base32-k string.
    //    /// </summary>
    //    public static string ToBase32String(byte[] bytes)
    //    {
    //        return new Base32().Encode(bytes);
    //    }

    //    public virtual string Encode(byte[] input)
    //    {
    //        var output = new StringBuilder();
    //        int specialLength = input.Length % 5;
    //        int normalLength = input.Length - specialLength;
    //        for (int i = 0; i < normalLength; i += 5)
    //        {
    //            int b1 = input[i] & 0xff;
    //            int b2 = input[i + 1] & 0xff;
    //            int b3 = input[i + 2] & 0xff;
    //            int b4 = input[i + 3] & 0xff;
    //            int b5 = input[i + 4] & 0xff;

    //            output.Append(eTable[(b1 >> 3) & 0x1f]);
    //            output.Append(eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
    //            output.Append(eTable[(b2 >> 1) & 0x1f]);
    //            output.Append(eTable[((b2 << 4) | (b3 >> 4)) & 0x1f]);
    //            output.Append(eTable[((b3 << 1) | (b4 >> 7)) & 0x1f]);
    //            output.Append(eTable[(b4 >> 2) & 0x1f]);
    //            output.Append(eTable[((b4 << 3) | (b5 >> 5)) & 0x1f]);
    //            output.Append(eTable[b5 & 0x1f]);
    //        }

    //        switch (specialLength)
    //        {
    //            case 1:
    //                {
    //                    int b1 = input[normalLength] & 0xff;
    //                    output.Append(eTable[(b1 >> 3) & 0x1f]);
    //                    output.Append(eTable[(b1 << 2) & 0x1f]);
    //                    output.Append(padding).Append(padding).Append(padding).Append(padding).Append(padding).Append(padding);
    //                    break;
    //                }

    //            case 2:
    //                {
    //                    int b1 = input[normalLength] & 0xff;
    //                    int b2 = input[normalLength + 1] & 0xff;
    //                    output.Append(eTable[(b1 >> 3) & 0x1f]);
    //                    output.Append(eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
    //                    output.Append(eTable[(b2 >> 1) & 0x1f]);
    //                    output.Append(eTable[(b2 << 4) & 0x1f]);
    //                    output.Append(padding).Append(padding).Append(padding).Append(padding);
    //                    break;
    //                }
    //            case 3:
    //                {
    //                    int b1 = input[normalLength] & 0xff;
    //                    int b2 = input[normalLength + 1] & 0xff;
    //                    int b3 = input[normalLength + 2] & 0xff;
    //                    output.Append(eTable[(b1 >> 3) & 0x1f]);
    //                    output.Append(eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
    //                    output.Append(eTable[(b2 >> 1) & 0x1f]);
    //                    output.Append(eTable[((b2 << 4) | (b3 >> 4)) & 0x1f]);
    //                    output.Append(eTable[(b3 << 1) & 0x1f]);
    //                    output.Append(padding).Append(padding).Append(padding);
    //                    break;
    //                }
    //            case 4:
    //                {
    //                    int b1 = input[normalLength] & 0xff;
    //                    int b2 = input[normalLength + 1] & 0xff;
    //                    int b3 = input[normalLength + 2] & 0xff;
    //                    int b4 = input[normalLength + 3] & 0xff;
    //                    output.Append(eTable[(b1 >> 3) & 0x1f]);
    //                    output.Append(eTable[((b1 << 2) | (b2 >> 6)) & 0x1f]);
    //                    output.Append(eTable[(b2 >> 1) & 0x1f]);
    //                    output.Append(eTable[((b2 << 4) | (b3 >> 4)) & 0x1f]);
    //                    output.Append(eTable[((b3 << 1) | (b4 >> 7)) & 0x1f]);
    //                    output.Append(eTable[(b4 >> 2) & 0x1f]);
    //                    output.Append(eTable[(b4 << 3) & 0x1f]);
    //                    output.Append(padding);
    //                    break;
    //                }
    //        }

    //        return output.ToString();
    //    }

    //    public virtual byte[] Decode(string data)
    //    {
    //        var outStream = new List<Byte>();

    //        int length = data.Length;
    //        while (length > 0)
    //        {
    //            if (!this.Ignore(data[length - 1])) break;
    //            length--;
    //        }

    //        int i = 0;
    //        int finish = length - 8;
    //        for (i = this.NextI(data, i, finish); i < finish; i = this.NextI(data, i, finish))
    //        {
    //            byte b1 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b2 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b3 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b4 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b5 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b6 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b7 = dTable[data[i++]];
    //            i = this.NextI(data, i, finish);
    //            byte b8 = dTable[data[i++]];

    //            outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
    //            outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
    //            outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
    //            outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
    //            outStream.Add((byte)((b7 << 5) | b8));
    //        }
    //        this.DecodeLastBlock(outStream,
    //            data[length - 8], data[length - 7], data[length - 6], data[length - 5],
    //            data[length - 4], data[length - 3], data[length - 2], data[length - 1]);

    //        return outStream.ToArray();
    //    }

    //    protected virtual int DecodeLastBlock(ICollection<byte> outStream, char c1, char c2, char c3, char c4, char c5, char c6, char c7, char c8)
    //    {
    //        if (c3 == padding)
    //        {
    //            byte b1 = dTable[c1];
    //            byte b2 = dTable[c2];
    //            outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
    //            return 1;
    //        }

    //        if (c5 == padding)
    //        {
    //            byte b1 = dTable[c1];
    //            byte b2 = dTable[c2];
    //            byte b3 = dTable[c3];
    //            byte b4 = dTable[c4];
    //            outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
    //            outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
    //            return 2;
    //        }

    //        if (c6 == padding)
    //        {
    //            byte b1 = dTable[c1];
    //            byte b2 = dTable[c2];
    //            byte b3 = dTable[c3];
    //            byte b4 = dTable[c4];
    //            byte b5 = dTable[c5];

    //            outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
    //            outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
    //            outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
    //            return 3;
    //        }

    //        if (c8 == padding)
    //        {
    //            byte b1 = dTable[c1];
    //            byte b2 = dTable[c2];
    //            byte b3 = dTable[c3];
    //            byte b4 = dTable[c4];
    //            byte b5 = dTable[c5];
    //            byte b6 = dTable[c6];
    //            byte b7 = dTable[c7];

    //            outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
    //            outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
    //            outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
    //            outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
    //            return 4;
    //        }

    //        else
    //        {
    //            byte b1 = dTable[c1];
    //            byte b2 = dTable[c2];
    //            byte b3 = dTable[c3];
    //            byte b4 = dTable[c4];
    //            byte b5 = dTable[c5];
    //            byte b6 = dTable[c6];
    //            byte b7 = dTable[c7];
    //            byte b8 = dTable[c8];
    //            outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
    //            outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
    //            outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
    //            outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
    //            outStream.Add((byte)((b7 << 5) | b8));
    //            return 5;
    //        }
    //    }

    //    protected int NextI(string data, int i, int finish)
    //    {
    //        while ((i < finish) && this.Ignore(data[i])) i++;

    //        return i;
    //    }

    //    protected bool Ignore(char c)
    //    {
    //        return (c == '\n') || (c == '\r') || (c == '\t') || (c == ' ') || (c == '-');
    //    }

    //    protected void InitialiseDecodingTable()
    //    {
    //        for (int i = 0; i < eTable.Length; i++)
    //        {
    //            dTable[eTable[i]] = (byte)i;
    //        }
    //    }
    //}

    public class Base32
    {

        //    public static byte[] FromBase32String(string str)
        //    {
        //        return new Base32().Decode(str);
        //    }

        //    /// <summary>
        //    /// Converts an array of bytes to a Base32-k string.
        //    /// </summary>
        //    public static string ToBase32String(byte[] bytes)
        //    {
        //        return new Base32().Encode(bytes);
        //    }
        /// <summary>
        /// Converts a Base32-k string into an array of bytes.
        /// </summary>
        /// <exception cref="System.ArgumentException">
        /// Input string <paramref name="input">input</paramref> contains invalid Base32-k characters.
        /// </exception>
        public static byte[] FromBase32String(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            input = input.TrimEnd('='); //remove padding characters
            int byteCount = input.Length * 5 / 8; //this must be TRUNCATED
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int mask = 0, arrayIndex = 0;

            foreach (char c in input)
            {
                int cValue = CharToValue(c);

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
            {
                returnArray[arrayIndex] = curByte;
            }

            return returnArray;
        }
        
        /// <summary>
        /// Converts an array of bytes to a Base32-k string.
        /// </summary>
        public static string ToBase32String(byte[] input)
        {
            if (input == null || input.Length == 0)
            {
                throw new ArgumentNullException("input");
            }

            int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
            char[] returnArray = new char[charCount];

            byte nextChar = 0, bitsRemaining = 5;
            int arrayIndex = 0;

            foreach (byte b in input)
            {
                nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
                returnArray[arrayIndex++] = ValueToChar(nextChar);

                if (bitsRemaining < 4)
                {
                    nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
                    returnArray[arrayIndex++] = ValueToChar(nextChar);
                    bitsRemaining += 5;
                }

                bitsRemaining -= 3;
                nextChar = (byte)((b << bitsRemaining) & 31);
            }

            //if we didn't end with a full char
            if (arrayIndex != charCount)
            {
                returnArray[arrayIndex++] = ValueToChar(nextChar);
                while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
            }

            return new string(returnArray);
        }

        private static int CharToValue(char c)
        {
            int value = (int)c;

            //65-90 == uppercase letters
            if (value < 91 && value > 64)
            {
                return value - 65;
            }
            //50-55 == numbers 2-7
            if (value < 56 && value > 49)
            {
                return value - 24;
            }
            //97-122 == lowercase letters
            if (value < 123 && value > 96)
            {
                return value - 97;
            }

            throw new ArgumentException("Character is not a Base32 character.", "c");
        }

        private static char ValueToChar(byte b)
        {
            if (b < 26)
            {
                return (char)(b + 65);
            }

            if (b < 32)
            {
                return (char)(b + 24);
            }

            throw new ArgumentException("Byte is not a value Base32 value.", "b");
        }

    }

}

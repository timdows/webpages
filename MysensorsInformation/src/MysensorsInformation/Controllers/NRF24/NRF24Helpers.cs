using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace MysensorListener.Controllers.NRF24
{
    public class NRF24Helpers
    {
        public static BitArray GetPartOfBitArray(BitArray bitArray, int start, int length)
        {
            if (length <= 0 || bitArray.Length < start + length)
                return new BitArray(0);

            var returnBitArray = new BitArray(length);
            var count = 0;
            while (count < length)
            {
                returnBitArray[count] = bitArray[start];
                start++;
                count++;
            }

            return returnBitArray;
        }

        public static BitArray GetBitArrayFromHexString(string str)
        {
            // Get a list of hex values from the input string
            var strings = NRF24Helpers.GetSplittedParts(str, 2);

            // Create a new bitarray
            var bitArray = new BitArray(strings.Count*8);

            var count = 0;
            foreach (var chunk in strings)
            {
                var byteValue = Convert.ToByte(chunk, 16);
                var array = new BitArray(new byte[] {byteValue});
                for (var i = 7; i >= 0; i--)
                {
                    bitArray[count++] = array[i];
                }
            }

            return bitArray;
        }

        public static List<string> GetSplittedParts(string str, int splitSize)
        {
            var list = new List<string>();
            for (var i = 0; i < str.Length; i += splitSize)
            {
                list.Add(str.Substring(i, splitSize));
            }

            return list;
        }

        public static string BitArrayToString(BitArray bitArray)
        {
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < bitArray.Length - 7; i += 8)
            {
                var b = GetByteFromBitArray(bitArray, i);
                var result = (char) b;
                stringBuilder.Append(result);
            }

            return stringBuilder.ToString();
        }

        public static int GetNumberFromBitArray(BitArray bitArray)
        {
            var number = 0;
            if (bitArray.Length > 0 && bitArray.Get(bitArray.Length - 1)) number++;
            if (bitArray.Length > 1 && bitArray.Get(bitArray.Length - 2)) number += 2;
            if (bitArray.Length > 2 && bitArray.Get(bitArray.Length - 3)) number += 4;
            if (bitArray.Length > 3 && bitArray.Get(bitArray.Length - 4)) number += 8;
            if (bitArray.Length > 4 && bitArray.Get(bitArray.Length - 5)) number += 16;
            if (bitArray.Length > 5 && bitArray.Get(bitArray.Length - 6)) number += 32;
            if (bitArray.Length > 6 && bitArray.Get(bitArray.Length - 7)) number += 64;
            if (bitArray.Length > 7 && bitArray.Get(bitArray.Length - 8)) number += 128;

            return number;
        }

        public static byte GetByteFromBitArray(BitArray bitArray, int bitOffset)
        {
            if (bitArray.Length < bitOffset + 8)
                return new byte();

            byte b = 0;
            if (bitArray.Get(7 + bitOffset)) b++;
            if (bitArray.Get(6 + bitOffset)) b += 2;
            if (bitArray.Get(5 + bitOffset)) b += 4;
            if (bitArray.Get(4 + bitOffset)) b += 8;
            if (bitArray.Get(3 + bitOffset)) b += 16;
            if (bitArray.Get(2 + bitOffset)) b += 32;
            if (bitArray.Get(1 + bitOffset)) b += 64;
            if (bitArray.Get(0 + bitOffset)) b += 128;

            return b;
        }

        public static string GetHexStringFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length != 8)
                return string.Empty;

            var b = NRF24Helpers.GetByteFromBitArray(bitArray, 0);
            return b.ToString("X2");
        }

        public static ushort GetCrc16(BitArray bitArray)
        {
            ushort crc = 0xFFFF;

            ushort data;
            byte workingByte;
            int shift;
            ushort bitoffs = 0;

            workingByte = NRF24Helpers.GetByteFromBitArray(bitArray, bitoffs >> 3);
            while (bitoffs < bitArray.Length)
            {
                shift = bitoffs & 7;
                // Shift the active bit to the position of bit 15 
                data = (ushort) (workingByte << (8 + shift));
                // Assure all other bits are 0
                data &= 0x8000;
                crc ^= data;

                if ((crc & 0x8000) > 0)
                //if ((crc & 0x8000) != 0)
                //if ((crc & 0x8000) == 0)
                {
                    crc = (ushort) ((crc << 1) ^ 0x8408); // 0x1021 = (1) 0001 0000 0010 0001 = x^16+x^12+x^5+1
                }
                else
                {
                    crc = (ushort) (crc << 1);
                    //crc <<= 1;
                }
                ++bitoffs;
                if (0 == (bitoffs & 7))
                {
                    // Get a new byte for the next 8 bits.
                    workingByte = NRF24Helpers.GetByteFromBitArray(bitArray, bitoffs >> 3);
                }
            }

            return crc;
        }

        public static ushort TestCrc(string data, int test)
        {
            var combinedBitArray = NRF24Helpers.GetBitArrayFromHexString(data);

            ushort testCrc = 0xFFFF;
            for (var offsetFront = 0; offsetFront <= combinedBitArray.Length; offsetFront++)
            {
                for (var i = combinedBitArray.Length - offsetFront; i >= 0; i--)
                {
                    //backwards stripping
                    var subBitArray = NRF24Helpers.GetPartOfBitArray(combinedBitArray, offsetFront, i);
                    testCrc = NRF24Helpers.GetCrc16(subBitArray);

                    if (testCrc == 0xB4CC && test == 1)
                    {
                        var b = "a";
                    }
                    if (testCrc == 0x52F7 && test == 2)
                    {
                        var b = "a";
                    }
                    if (testCrc == 0x354E && test == 3)
                    {
                        var b = "a";
                    }
                }
            }

            return testCrc;
        }

        public static string CalcCRC16(string strInput)
        {
            ushort crc = 0xFFFF;
            byte[] data = NRF24Helpers.GetBytesFromHexString(strInput);
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc.ToString("X4");
        }

        //TODO ???
        public static byte[] GetBytesFromHexString(string strInput)
        {
            var bytArOutput = new byte[] { };
            if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
            {
                SoapHexBinary hexBinary = null;
                try
                {
                    hexBinary = SoapHexBinary.Parse(strInput);
                    if (hexBinary != null)
                    {
                        bytArOutput = hexBinary.Value;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            return bytArOutput;
        }

        public static byte[] GetBytesFromBitArray(BitArray bitArray)
        {
            if (bitArray.Length%8 != 0)
                return new byte[0];

            var count = bitArray.Length/8;
            var bytes = new byte[count];

            for (var i = 0; i < count; i++)
            {
                bytes[i] = NRF24Helpers.GetByteFromBitArray(bitArray, i*8);
            }

            return bytes;
        }

        public static ushort Crc16Ccitt(string str)
        {
            byte[] bytes = NRF24Helpers.GetBytesFromHexString(str);
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0xffff;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort)(i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort)((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }
                table[i] = temp;
            }
            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }
            return crc;
        }

        public static ushort Crc16Bit(BitArray bitArray)
        {
            ushort crc = 0xffff;
            var length = bitArray.Length;
            while (length >= 0)
            {

                length--;
            }

            return crc;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    internal class Checksum
    {
        public static byte AlgebraicChecksum(byte[] bytes)
        {
            int sum = 0;
            foreach (var addend in bytes)
            {
                sum += addend;
                sum %= 256;
            }
            return (byte)sum;
        }

        public static byte[] CreateCheckSumCRC(byte[] package)
        {
            int checksumCRC = 0;
            foreach (byte _byte in package)
            {
                checksumCRC = checksumCRC ^ _byte << 8;
                for (int i = 0; i < 8; i++)
                {
                    if (Convert.ToBoolean(checksumCRC & 0x8000))
                        checksumCRC = (checksumCRC >> 1) ^ 0x1021;
                    else
                        checksumCRC = checksumCRC >> 1;
                }
            }
            return BitConverter.GetBytes(checksumCRC);
        }
    }
}

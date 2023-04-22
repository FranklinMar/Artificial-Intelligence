﻿using System;
using System.Security.Cryptography;

namespace Lab1
{
    public class SecureRandom : RandomNumberGenerator
    {
        private readonly RandomNumberGenerator rng = new RNGCryptoServiceProvider();


        public int Next()
        {
            var data = new byte[sizeof(int)];
            rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
        }

        public int Next(int maxValue)
        {
            return Next(0, maxValue);
        }

        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }
            return (int)Math.Floor((minValue + ((double)maxValue - minValue) * NextDouble()));
        }

        public double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            rng.GetBytes(data);
            var randUint = BitConverter.ToUInt32(data, 0);
            return randUint / (uint.MaxValue + 1.0);
        }
        public double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(minValue));
            }
            return NextDouble() * (maxValue - minValue) + minValue;
        }

        public override void GetBytes(byte[] data)
        {
            rng.GetBytes(data);
        }

        public override void GetNonZeroBytes(byte[] data)
        {
            rng.GetNonZeroBytes(data);
        }
    }
}
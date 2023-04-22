using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Lab1;

namespace Lab3
{
    //[StructLayout(LayoutKind.Explicit)]
    //[StructLayout(LayoutKind.Explicit)]
    public class Genome<T> where T : struct
    {
        private static int SIZE = Marshal.SizeOf(typeof(T));
        private static int BYTE_SIZE = 8;
        private static SecureRandom Generator = new();
        //[FieldOffset(0)]
        //[FieldOffset(0)]
        private T Value;
        public T Variable
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
                /*Object obj = Variable;
                if (obj == null)
                    BytesOfVariable = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    BytesOfVariable = ms.ToArray();
                }*/
                Bytes = ConvertToBytes(value);
            }
        }
        //[FieldOffset(1)]
        //[FieldOffset(0)]
        private byte[] Bytes;
        public byte[] BytesOfVariable {
            get
            {
                return (byte[]) Bytes.Clone();
            }
            set
            {
                Bytes = value;
                Value = ConvertToObject(value);
            }// = new byte[Marshal.SizeOf(typeof(T))];
        }
        public Genome(T Value){
            Variable = Value;
            //BytesOfVariable = obj == null ? obj : BitConverter.GetBytes(obj);
            //Buffer.BlockCopy(Variable, 0, BytesOfVariable, 0, BytesOfVariable.Length);

        }
        public Genome(byte[] Bytes)
        {
            BytesOfVariable = Bytes;
            //BytesOfVariable = obj == null ? obj : BitConverter.GetBytes(obj);
            //Buffer.BlockCopy(Variable, 0, BytesOfVariable, 0, BytesOfVariable.Length);

        }

        public void InverseByte(int Index)
        {
            if (Index < 0 || Index >= BYTE_SIZE * Bytes.Length)
            {
                throw new ArgumentException("Byte size exceeded");
            }
            Bytes[Index / BYTE_SIZE] = (byte)(Bytes[Index/BYTE_SIZE] ^ (1 << Index % BYTE_SIZE));
            Value = (T) ConvertToObject(Bytes);
        }

        public static byte[] ConvertToBytes(object obj)
        {
            var size = Marshal.SizeOf(typeof(T));
            // Both managed and unmanaged buffers required.
            var bytes = new byte[SIZE];
            var ptr = Marshal.AllocHGlobal(SIZE);
            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr(obj, ptr, false);
            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy(ptr, bytes, 0, SIZE);
            // Release unmanaged memory.
            Marshal.FreeHGlobal(ptr);
            return bytes;//.Reverse().ToArray();
        }

        public static T ConvertToObject(byte[] Array)
        {
            var ptr = Marshal.AllocHGlobal(SIZE);
            Marshal.Copy(Array/*.Reverse().ToArray()*/, 0, ptr, SIZE);
            object obj = Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return (T) obj;
        }

        public static string BytesToString(byte[] array)
        {
            //string.Join(' ', Array.ConvertAll<byte, string>(genome.BytesOfVariable, Byte => new string(Convert.ToString(Byte, 2).PadLeft(8, '0'))).Reverse())
            string[] BinaryStringArray = Array.ConvertAll(array, Byte => new string(Convert.ToString(Byte, 2).PadLeft(8, '0')));
            return string.Join(' ', BinaryStringArray.Reverse());
        }
    }
}

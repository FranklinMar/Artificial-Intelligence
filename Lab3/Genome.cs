using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace Lab3
{
    //[StructLayout(LayoutKind.Explicit)]
    //[StructLayout(LayoutKind.Explicit)]
    public class Genome<T>
    {
        private static int SIZE = Marshal.SizeOf(typeof(T));

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
                _Genes = ConvertToBytes(value);
            }
        }
        //[FieldOffset(1)]
        //[FieldOffset(0)]
        private BitArray _Genes;
        public BitArray Genes {
            get
            {
                return (BitArray) _Genes.Clone();
            }
            set
            {
                _Genes = value;
                Value = ConvertToObject(value);
            }// = new byte[Marshal.SizeOf(typeof(T))];
        }
        public int BitCount
        {
            get
            {
                return _Genes.Count;
            }
        }

        public Genome(T Value){
            Variable = Value;
            //BytesOfVariable = obj == null ? obj : BitConverter.GetBytes(obj);
            //Buffer.BlockCopy(Variable, 0, BytesOfVariable, 0, BytesOfVariable.Length);

        }
        public Genome(BitArray Array)
        {
            Genes = Array;
            //BytesOfVariable = obj == null ? obj : BitConverter.GetBytes(obj);
            //Buffer.BlockCopy(Variable, 0, BytesOfVariable, 0, BytesOfVariable.Length);

        }

        public Genome<T> Cross(Genome<T> Other, int Point)
        {
            /*byte[] New = Other.BytesOfVariable;
            if (Point < 0 || Point >= New.Length || Point >= Bytes.Length)
            {
                throw new ArgumentException("Point exceeds length of genome");
            }
            
            for (int i = 0; i < Point; i++)
            {
                New[i]
            }*/
            //BitArray New = Other.Genes;
            if (Point < 0 || Point >= _Genes.Count || Point >= Other._Genes.Count)
            {
                throw new ArgumentException("Point exceeds bounds of genome");
            }
            if (_Genes.Count != Other._Genes.Count)
            {
                throw new ArgumentException("Different Gene Length");
            }
            BitArray Array = Genes;
            for (int i = Point; i < Array.Count; i++)
            {
                Array[i] = Other._Genes[i];
            }
            return new(Array);
        }

        public void InverseGene(int Index)
        {
            /*if (Index < 0 || Index >= BYTE_SIZE * Bytes.Length)
            {
                throw new ArgumentException("Byte size exceeded");
            }
            Bytes[Index / BYTE_SIZE] = (byte)(Bytes[Index/BYTE_SIZE] ^ (1 << Index % BYTE_SIZE));
            Value = ConvertToObject(Bytes);*/
            if (Index < 0 || Index >= _Genes.Count)
            {
                throw new ArgumentException("Index out of bounds");
            }
            _Genes[Index] = !_Genes[Index];
            Value = ConvertToObject(_Genes);
        }

        public void SetGene(bool Gene, int Index)
        {
            if (Index < 0 || Index >= _Genes.Count)
            {
                throw new ArgumentException("Index out of bounds");
            }
            _Genes[Index] = Gene;
            /*if (Index < 0 || Index >= BYTE_SIZE * Bytes.Length)
            {
                throw new ArgumentException("Byte size exceeded");
            }
            Bytes[Index / BYTE_SIZE] = (byte) (Bytes[Index / BYTE_SIZE] ^ ((Gene ? 1 : 0) << Index % BYTE_SIZE));
            Value = ConvertToObject(Bytes);*/
        }

        public static BitArray ConvertToBytes(object Object)
        {
            //var size = Marshal.SizeOf(typeof(T));
            // Both managed and unmanaged buffers required.
            var bytes = new byte[SIZE];
            var ptr = Marshal.AllocHGlobal(SIZE);
            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr(Object, ptr, false);
            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy(ptr, bytes, 0, SIZE);
            // Release unmanaged memory.
            Marshal.FreeHGlobal(ptr);
            return new BitArray(bytes);//.Reverse().ToArray();
        }

        public static T ConvertToObject(BitArray Array)
        {
            byte[] array = new byte[(int) Math.Ceiling((double)Array.Count/sizeof(byte))];
            Array.CopyTo(array, 0);
            var ptr = Marshal.AllocHGlobal(SIZE);
            Marshal.Copy(array/*.Reverse().ToArray()*/, 0, ptr, SIZE);
            T Object = (T) Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return Object;
        }

        public string GenesToString()
        {
            StringBuilder Builder = new();
            for (int i = _Genes.Count - 1; i >= 0; i--)
            {
                Builder.Append(_Genes[i] ? '1' : '0');
            }
            return Builder.ToString();
        }

        /*public static string BytesToString(byte[] array)
        {
            //string.Join(' ', Array.ConvertAll<byte, string>(genome.BytesOfVariable, Byte => new string(Convert.ToString(Byte, 2).PadLeft(8, '0'))).Reverse())
            string[] BinaryStringArray = Array.ConvertAll(array, Byte => new string(Convert.ToString(Byte, 2).PadLeft(8, '0')));
            return string.Join(' ', BinaryStringArray.Reverse());
        }*/
    }
}

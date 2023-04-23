using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace Lab3
{
    public class Genome<T>
    {
        private static readonly int SIZE = Marshal.SizeOf(typeof(T));

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
                _Genes = ConvertToBytes(value);
            }
        }
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
            }
        }
        public int BitCount
        {
            get
            {
                return _Genes.Count;
            }
        }

        public Genome(T Value)=> Variable = Value;
        
        public Genome(BitArray Array) => Genes = Array;
        

        public Genome<T> Cross(Genome<T> Other, int Point)
        {
            if (Point < 0 || Point >= _Genes.Count || Point >= Other._Genes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Point));
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
            if (Index < 0 || Index >= _Genes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Index));
            }
            _Genes[Index] = !_Genes[Index];
            Value = ConvertToObject(_Genes);
        }

        public void SetGene(bool Gene, int Index)
        {
            if (Index < 0 || Index >= _Genes.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Index));
            }
            _Genes[Index] = Gene;
        }

        public static BitArray ConvertToBytes(object Object)
        {
            // Both managed and unmanaged buffers required.
            var bytes = new byte[SIZE];
            var ptr = Marshal.AllocHGlobal(SIZE);
            // Copy object byte-to-byte to unmanaged memory.
            Marshal.StructureToPtr(Object, ptr, false);
            // Copy data from unmanaged memory to managed buffer.
            Marshal.Copy(ptr, bytes, 0, SIZE);
            // Release unmanaged memory.
            Marshal.FreeHGlobal(ptr);
            return new BitArray(bytes);
        }

        public static T ConvertToObject(BitArray Array)
        {
            byte[] array = new byte[(int) Math.Ceiling((double)Array.Count/sizeof(byte))];
            Array.CopyTo(array, 0);
            var ptr = Marshal.AllocHGlobal(SIZE);
            Marshal.Copy(array, 0, ptr, SIZE);
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
    }
}

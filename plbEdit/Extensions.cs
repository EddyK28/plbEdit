using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace plbEdit
{
    public static class Extensions
    {
        public static void WriteToBytes(this string source, byte[] destination, int offset, bool reverse = false)
        {
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(source), 0, destination, offset, source.Length*2);
        }

        public static void WriteToBytes<T>(this T source, byte[] destination, int offset, bool reverse = false)
        {
            if (destination == null)
                throw new ArgumentException("Destination array cannot be null");

            // check if there is enough space for all the 4 bytes we will copy  //TODO: check correct size
            if (destination.Length < offset + 4)
                throw new ArgumentException("Not enough room in the destination array");

            dynamic parameter = source;
            int size = Marshal.SizeOf(typeof(T));
            byte[] data = BitConverter.GetBytes(parameter);

            if (reverse)
                Array.Reverse(data);

            Buffer.BlockCopy(data, 0, destination, offset, size);
        }

    }
}

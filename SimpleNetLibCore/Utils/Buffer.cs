using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNetLibCore.Utils
{
    public class Buffer<T>
    {
        public T[] data;

        public Buffer()
        {
            data = new T[8];

            for (int x = 0; x < data.Length; x++)
                data[x] = default(T);
        }

        public void Allocate(int size)
        {
            data = realloc(size);
        }

        public void Expand(int size)
        {
            data = realloc(data.Length + size);
        }

        public void Copy(T[] input)
        {
            input.CopyTo(data, 0);
        }

        public T[] Clone()
        {
            return data.ToArray();
        }

        private T[] realloc(int newSize)
        {
            T[] newArray = new T[newSize];

            int startIndex = 0;
            if (data != null)
            {
                for(int x = 0; x < data.Length; x++)
                {
                    newArray[x] = data[x];
                    startIndex++;
                }
            }

            for (int x = startIndex; x < newArray.Length; x++)
                newArray[x] = default(T);

            return newArray;
        }
    }
}

using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Blux
{
    public struct Area
    {
        public UnsafeList<Chunk> chunks;

        public void Allocate()
        {
            chunks = new UnsafeList<Chunk>(Defs.Chunk.numChunksY, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            chunks.Length = Defs.Chunk.numChunksY;

            for (int i = 0; i < Defs.Chunk.numChunksY; i++)
            {
                chunks.ElementAt(i).Allocate();
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < Defs.Chunk.numChunksY; i++)
            {
                chunks.ElementAt(i).Dispose();
            }
        }
    }
}


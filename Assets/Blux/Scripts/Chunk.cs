using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Blux
{
    public struct Chunk
    {
        public UnsafeList<byte> blocks;

        public void Allocate()
        {
            blocks = new UnsafeList<byte>(Defs.Chunk.voxelsTotal, Allocator.Persistent, NativeArrayOptions.ClearMemory);
            blocks.Length = Defs.Chunk.voxelsTotal;
        }

        public void Dispose()
        {
            blocks.Dispose();
        }

        public int GetNumVoxels()
        {
            int numVoxels = 0;

            for (int i = 0; i < Defs.Chunk.voxelsTotal; i++)
            {
                if (blocks[i] > 0)
                {
                    numVoxels++;
                }
            }

            return numVoxels;
        }
    }
}

using Unity.Burst;
using Unity.Mathematics;

namespace Blux
{
    public class Generator
    {
        [BurstCompile]
        public static void GenerateArea(Area area, int2 areaCoordinate)
        {
            for (int i = 0; i < Defs.Chunk.numChunksY; i++)
            {
                GenerateChunk(area.chunks.ElementAt(i), new int3(areaCoordinate.x, i, areaCoordinate.y));
            }
        }

        [BurstCompile]
        public static void GenerateChunk(Chunk chunk, int3 chunkCoordinate)
        {
            float3 chunkPosition = Defs.Chunk.voxelsPerStride * chunkCoordinate;

            int i = 0;
            for (int y = 0; y < Defs.Chunk.voxelsPerStride; y++)
            {
                for (int z = 0; z < Defs.Chunk.voxelsPerStride; z++)
                {
                    for (int x = 0; x < Defs.Chunk.voxelsPerStride; x++)
                    {
                        float3 voxelPosition = chunkPosition + new float3(x, y, z);
                        chunk.blocks.ElementAt(i) = GenerateVoxel(voxelPosition);
                        i++;
                    }
                }
            }
        }

        [BurstCompile]
        public static byte GenerateVoxel(float3 worldPosition)
        {
            float value = noise.snoise(0.1f * worldPosition);

            if (value < 0.0f)
            {
                return 0;
            }
            else
            {
                int id = (int)(1.0f + 4.999f * noise.snoise(0.1f * worldPosition + new float3(500.0f)));
                return (byte)id;
            }
        }
    }
}

using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Blux
{
    public class AreaObject
    {
        public Area area;
        public Mesh[] chunkMeshes;
        public GameObject[] chunkObjects;

        public bool isBlockJobStarted;
        public bool isGenerated;

        private BlocksJob blocksJob;
        private JobHandle blocksHandle;

        public void Init()
        {
            isBlockJobStarted = false;

            area.Allocate();
            
            chunkObjects = new GameObject[Defs.Chunk.numChunksY];
            chunkMeshes = new Mesh[Defs.Chunk.numChunksY];
        }

        public void GenerateBlocks(int2 areaCoordinate)
        {
            if (!isBlockJobStarted)
            {
                blocksJob = new();
                blocksJob.area = area;
                blocksJob.areaCoordinate = areaCoordinate;
                blocksHandle = blocksJob.Schedule();
                isBlockJobStarted = true;
            }
            else
            {
                if (blocksHandle.IsCompleted)
                {
                    blocksHandle.Complete();
                    isGenerated = true;
                    isBlockJobStarted = false;
                }
            }
        }

        public void GenerateMeshes(World world, int2 areaCoordinate)
        {
            for (int i = 0; i < Defs.Chunk.numChunksY; i++)
            {
                chunkMeshes[i] = new Mesh();
                chunkObjects[i] = world.InstantiateChunkObject();
                chunkObjects[i].transform.position = new Vector3(
                    areaCoordinate.x * Defs.Chunk.voxelsPerStride,
                    i * Defs.Chunk.voxelsPerStride,
                    areaCoordinate.y * Defs.Chunk.voxelsPerStride
                );
                Mesher.MeshChunk(area.chunks[i], ref chunkMeshes[i]);
                chunkObjects[i].GetComponent<MeshFilter>().mesh = chunkMeshes[i];
            }
        }

        public void Destroy()
        {
            area.Dispose();
        }
    }
}

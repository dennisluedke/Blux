using UnityEngine;
using Unity.Mathematics;

namespace Blux
{
    public class World : MonoBehaviour
    {
        public GameObject chunkPrefab;

        private Area area;

        private Mesh[] chunkMeshes;
        private GameObject[] chunkObjects;

        private void Start()
        {
            area.Allocate();

            Generator.GenerateArea(area, new int2(0, 0));

            chunkObjects = new GameObject[Defs.Chunk.numChunksY];
            chunkMeshes = new Mesh[Defs.Chunk.numChunksY];

            for (int i = 0; i < Defs.Chunk.numChunksY; i++)
            {
                chunkMeshes[i] = new Mesh();
                chunkObjects[i] = Instantiate(chunkPrefab);
                chunkObjects[i].transform.position = new Vector3(0.0f, i * Defs.Chunk.voxelsPerStride, 0.0f);
                Mesher.MeshChunk(area.chunks[i], ref chunkMeshes[i]);
                chunkObjects[i].GetComponent<MeshFilter>().mesh = chunkMeshes[i];
            }
        }

        private void OnDestroy()
        {
            area.Dispose();
        }
    }
}

using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using Codice.Client.BaseCommands.BranchExplorer;

namespace Blux
{
    public class World : MonoBehaviour
    {
        public GameObject chunkPrefab;

        private static readonly int viewDistance = 4;
        private static readonly int loadDistance = viewDistance + 1;

        private Dictionary<int2, AreaObject> areas;
        private Queue<int2> generateBlocksQueue;
        private Queue<int2> generateMeshesQueue;

        private void Start()
        {
            generateBlocksQueue = new(256);
            generateMeshesQueue = new(256);
            areas = new(128);

            // Generate Blocks Queue
            {
                int i = 0;
                for (int z = -loadDistance; z <= loadDistance; z++)
                {
                    for (int x = -loadDistance; x <= loadDistance; x++)
                    {
                        generateBlocksQueue.Enqueue(new int2(x, z));
                        i++;
                    }
                }
            }

            // Generate Meshes Queue
            {
                int i = 0;
                for (int z = -viewDistance; z <= viewDistance; z++)
                {
                    for (int x = -viewDistance; x <= viewDistance; x++)
                    {
                        generateMeshesQueue.Enqueue(new int2(x, z));
                        i++;
                    }
                }
            }
        }

        private void Update()
        {
            if (generateBlocksQueue.Count > 0)
            {
                var areaCoordinate = generateBlocksQueue.Peek();

                if (!areas.ContainsKey(areaCoordinate))
                {
                    AreaObject areaObject = new();
                    areaObject.Init();
                    areas.Add(areaCoordinate, areaObject);
                }

                if (!areas[areaCoordinate].isGenerated)
                {
                    areas[areaCoordinate].GenerateBlocks(areaCoordinate);
                }
                else
                {
                    generateBlocksQueue.Dequeue();
                }
            }

            if (generateMeshesQueue.Count > 0)
            {
                var areaCoordinate = generateMeshesQueue.Peek();
                bool areNeighborsLoaded = true;

                for (int z = -1; z <= 1; z++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (Mathf.Abs(x) == 1 && Mathf.Abs(z) == 1)
                        {
                            continue;
                        }

                        if (!areas.ContainsKey(areaCoordinate + new int2(x, z)) || !areas[areaCoordinate + new int2(x, z)].isGenerated)
                        {
                            areNeighborsLoaded = false;
                        }
                    }
                }

                if (areNeighborsLoaded)
                {
                    areas[areaCoordinate].GenerateMeshes(this, areaCoordinate);
                    generateMeshesQueue.Dequeue();
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var area in areas)
            {
                area.Value.Destroy();
            }
        }

        public GameObject InstantiateChunkObject()
        {
            return Instantiate(chunkPrefab);
        }
    }
}

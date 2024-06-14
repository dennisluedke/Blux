using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Blux
{
    public class Mesher
    {
        public static void MeshChunk(Chunk chunk, ref Mesh mesh)
        {
            int numVoxels = chunk.GetNumVoxels();
            int numVertices = numVoxels * 4 * 6;
            int numIndices = numVoxels * 2 * 3 * 6;

            NativeArray<float3> vertices = new(numVertices, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            NativeArray<float3> normals = new(numVertices, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            NativeArray<float3> uvs = new(numVertices, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            NativeArray<int> indices = new(numIndices, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

            int vertexCount = 0;
            int indexCount = 0;

            int i = 0;
            for (int y = 0; y < Defs.Chunk.voxelsPerStride; y++)
            {
                for (int z = 0; z < Defs.Chunk.voxelsPerStride; z++)
                {
                    for (int x = 0; x < Defs.Chunk.voxelsPerStride; x++)
                    {
                        MeshVoxel(chunk.blocks[i], new int3(x, y, z), ref vertexCount, ref indexCount, vertices, normals, uvs, indices);
                        i++;
                    }
                }
            }

            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();

            vertices.Dispose();
            normals.Dispose();
            uvs.Dispose();
            indices.Dispose();
        }

        [BurstCompile]
        public static void MeshVoxel(byte block, int3 offset, ref int vertexCount, ref int indexCount, NativeArray<float3> vertices, NativeArray<float3> normals, NativeArray<float3> uvs, NativeArray<int> indices)
        {
            if (block == 0)
            {
                return;
            }

            // Bottom Side
            vertices[vertexCount] = new float3(1.0f, 0.0f, 0.0f) + offset;
            vertices[vertexCount + 1] = new float3(1.0f, 0.0f, 1.0f) + offset;
            vertices[vertexCount + 2] = new float3(0.0f, 0.0f, 1.0f) + offset;
            vertices[vertexCount + 3] = new float3(0.0f, 0.0f, 0.0f) + offset;

            normals[vertexCount] = new float3(0.0f, -1.0f, 0.0f);
            normals[vertexCount + 1] = new float3(0.0f, -1.0f, 0.0f);
            normals[vertexCount + 2] = new float3(0.0f, -1.0f, 0.0f);
            normals[vertexCount + 3] = new float3(0.0f, -1.0f, 0.0f);

            uvs[vertexCount] = new(0.0f, 0.0f, (float)block - 1.0f);
            uvs[vertexCount + 1] = new(0.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 2] = new(1.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 3] = new(1.0f, 0.0f, (float)block - 1.0f);

            indices[indexCount] = vertexCount;
            indices[indexCount + 1] = vertexCount + 1;
            indices[indexCount + 2] = vertexCount + 2;
            indices[indexCount + 3] = vertexCount;
            indices[indexCount + 4] = vertexCount + 2;
            indices[indexCount + 5] = vertexCount + 3;

            vertexCount += 4;
            indexCount += 6;

            // Top Side
            vertices[vertexCount] = new float3(0.0f, 1.0f, 0.0f) + offset;
            vertices[vertexCount + 1] = new float3(0.0f, 1.0f, 1.0f) + offset;
            vertices[vertexCount + 2] = new float3(1.0f, 1.0f, 1.0f) + offset;
            vertices[vertexCount + 3] = new float3(1.0f, 1.0f, 0.0f) + offset;

            normals[vertexCount] = new float3(0.0f, 1.0f, 0.0f);
            normals[vertexCount + 1] = new float3(0.0f, 1.0f, 0.0f);
            normals[vertexCount + 2] = new float3(0.0f, 1.0f, 0.0f);
            normals[vertexCount + 3] = new float3(0.0f, 1.0f, 0.0f);

            uvs[vertexCount] = new(0.0f, 0.0f, (float)block - 1.0f);
            uvs[vertexCount + 1] = new(0.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 2] = new(1.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 3] = new(1.0f, 0.0f, (float)block - 1.0f);

            indices[indexCount] = vertexCount;
            indices[indexCount + 1] = vertexCount + 1;
            indices[indexCount + 2] = vertexCount + 2;
            indices[indexCount + 3] = vertexCount;
            indices[indexCount + 4] = vertexCount + 2;
            indices[indexCount + 5] = vertexCount + 3;

            vertexCount += 4;
            indexCount += 6;

            // Backwards Side
            vertices[vertexCount] = new float3(0.0f, 0.0f, 0.0f) + offset;
            vertices[vertexCount + 1] = new float3(0.0f, 1.0f, 0.0f) + offset;
            vertices[vertexCount + 2] = new float3(1.0f, 1.0f, 0.0f) + offset;
            vertices[vertexCount + 3] = new float3(1.0f, 0.0f, 0.0f) + offset;

            normals[vertexCount] = new float3(0.0f, 0.0f, -1.0f);
            normals[vertexCount + 1] = new float3(0.0f, 0.0f, -1.0f);
            normals[vertexCount + 2] = new float3(0.0f, 0.0f, -1.0f);
            normals[vertexCount + 3] = new float3(0.0f, 0.0f, -1.0f);

            uvs[vertexCount] = new(0.0f, 0.0f, (float)block - 1.0f);
            uvs[vertexCount + 1] = new(0.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 2] = new(1.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 3] = new(1.0f, 0.0f, (float)block - 1.0f);

            indices[indexCount] = vertexCount;
            indices[indexCount + 1] = vertexCount + 1;
            indices[indexCount + 2] = vertexCount + 2;
            indices[indexCount + 3] = vertexCount;
            indices[indexCount + 4] = vertexCount + 2;
            indices[indexCount + 5] = vertexCount + 3;

            vertexCount += 4;
            indexCount += 6;

            // Forwards Side
            vertices[vertexCount] = new float3(1.0f, 0.0f, 1.0f) + offset;
            vertices[vertexCount + 1] = new float3(1.0f, 1.0f, 1.0f) + offset;
            vertices[vertexCount + 2] = new float3(0.0f, 1.0f, 1.0f) + offset;
            vertices[vertexCount + 3] = new float3(0.0f, 0.0f, 1.0f) + offset;

            normals[vertexCount] = new float3(0.0f, 0.0f, 1.0f);
            normals[vertexCount + 1] = new float3(0.0f, 0.0f, 1.0f);
            normals[vertexCount + 2] = new float3(0.0f, 0.0f, 1.0f);
            normals[vertexCount + 3] = new float3(0.0f, 0.0f, 1.0f);

            uvs[vertexCount] = new(0.0f, 0.0f, (float)block - 1.0f);
            uvs[vertexCount + 1] = new(0.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 2] = new(1.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 3] = new(1.0f, 0.0f, (float)block - 1.0f);

            indices[indexCount] = vertexCount;
            indices[indexCount + 1] = vertexCount + 1;
            indices[indexCount + 2] = vertexCount + 2;
            indices[indexCount + 3] = vertexCount;
            indices[indexCount + 4] = vertexCount + 2;
            indices[indexCount + 5] = vertexCount + 3;

            vertexCount += 4;
            indexCount += 6;

            // Left Side
            vertices[vertexCount] = new float3(0.0f, 0.0f, 1.0f) + offset;
            vertices[vertexCount + 1] = new float3(0.0f, 1.0f, 1.0f) + offset;
            vertices[vertexCount + 2] = new float3(0.0f, 1.0f, 0.0f) + offset;
            vertices[vertexCount + 3] = new float3(0.0f, 0.0f, 0.0f) + offset;

            normals[vertexCount] = new float3(-1.0f, 0.0f, 0.0f);
            normals[vertexCount + 1] = new float3(-1.0f, 0.0f, 0.0f);
            normals[vertexCount + 2] = new float3(-1.0f, 0.0f, 0.0f);
            normals[vertexCount + 3] = new float3(-1.0f, 0.0f, 0.0f);

            uvs[vertexCount] = new(0.0f, 0.0f, (float)block - 1.0f);
            uvs[vertexCount + 1] = new(0.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 2] = new(1.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 3] = new(1.0f, 0.0f, (float)block - 1.0f);

            indices[indexCount] = vertexCount;
            indices[indexCount + 1] = vertexCount + 1;
            indices[indexCount + 2] = vertexCount + 2;
            indices[indexCount + 3] = vertexCount;
            indices[indexCount + 4] = vertexCount + 2;
            indices[indexCount + 5] = vertexCount + 3;

            vertexCount += 4;
            indexCount += 6;

            // Right Side
            vertices[vertexCount] = new float3(1.0f, 0.0f, 0.0f) + offset;
            vertices[vertexCount + 1] = new float3(1.0f, 1.0f, 0.0f) + offset;
            vertices[vertexCount + 2] = new float3(1.0f, 1.0f, 1.0f) + offset;
            vertices[vertexCount + 3] = new float3(1.0f, 0.0f, 1.0f) + offset;

            normals[vertexCount] = new float3(1.0f, 0.0f, 0.0f);
            normals[vertexCount + 1] = new float3(1.0f, 0.0f, 0.0f);
            normals[vertexCount + 2] = new float3(1.0f, 0.0f, 0.0f);
            normals[vertexCount + 3] = new float3(1.0f, 0.0f, 0.0f);

            uvs[vertexCount] = new(0.0f, 0.0f, (float)block - 1.0f);
            uvs[vertexCount + 1] = new(0.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 2] = new(1.0f, 1.0f, (float)block - 1.0f);
            uvs[vertexCount + 3] = new(1.0f, 0.0f, (float)block - 1.0f);

            indices[indexCount] = vertexCount;
            indices[indexCount + 1] = vertexCount + 1;
            indices[indexCount + 2] = vertexCount + 2;
            indices[indexCount + 3] = vertexCount;
            indices[indexCount + 4] = vertexCount + 2;
            indices[indexCount + 5] = vertexCount + 3;

            vertexCount += 4;
            indexCount += 6;
        }
    }
}

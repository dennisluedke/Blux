using System;

namespace Blux
{
    public readonly struct Defs
    {
        public readonly struct Chunk
        {
            public static readonly int numChunksY = 16;
            public static readonly int voxelsPerStride = 16;
            public static readonly int voxelsPerLayer = voxelsPerStride * voxelsPerStride;
            public static readonly int voxelsTotal = voxelsPerStride * voxelsPerStride * voxelsPerStride;
        }

        public enum Blocks
        {
            Air = 0,
            Dirt = 1,
            Grass = 2,
            Stone = 3,
            Sand = 4,
            Gravel = 5
        }
    }
}

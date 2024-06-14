using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Blux
{
    [BurstCompile]
    public struct BlocksJob : IJob
    {
        public Area area;
        public int2 areaCoordinate;

        public void Execute()
        {
            Generator.GenerateArea(area, areaCoordinate);
        }
    }
}

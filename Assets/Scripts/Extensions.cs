using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public static class Extensions
    {
        public static Colors Random(this Colors col, bool onlyValidColors = false)
        {
            return (Colors)UnityEngine.Random.Range(onlyValidColors ? 1 : -1, 4);
        }

        public static RoadDirections OppositeDirection(this RoadDirections directions)
        {
            switch (directions)
            {
                case RoadDirections.North:
                    return RoadDirections.South;
                case RoadDirections.South:
                    return RoadDirections.North;
                case RoadDirections.East:
                    return RoadDirections.West;
                default:
                    return RoadDirections.East;
            }
        }

        public static RoadDirections RandomDirection()
        {
            var rnd = UnityEngine.Random.Range(0, 4);
            switch (rnd)
            {
                case 0:
                    return RoadDirections.North;
                case 1:
                    return RoadDirections.South;
                case 2:
                    return RoadDirections.East;
                default:
                    return RoadDirections.West;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    /// <summary>
    /// Time of day
    /// </summary>
    public enum GameState
    {
        Day,
        Night,
        None
    }

    /// <summary>
    /// Possible tile connections
    /// </summary>
    public enum RoadDirections
    {
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }

    /// <summary>
    /// Possible colors for flying sausers, houses and warehouses 
    /// </summary>
    public enum Colors
    {
        None = -1,
        Road = 0,
        Yellow = 1,
        Blue = 2,
        White = 3
    }
}

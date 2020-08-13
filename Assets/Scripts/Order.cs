using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    [System.Serializable]
    public class Order
    {
        public Colors Color { get; set; }
        public Tile Destination { get; set; }
        public AStar AssignedCar { get; set; }
        public WareHouse ThaWarehouse { get; set; }
    }
}


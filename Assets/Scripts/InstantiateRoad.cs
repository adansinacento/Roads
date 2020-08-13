using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class InstantiateRoad : MonoBehaviour
    {
        public GameObject prefab, CartPrefab;
        public Sprite Grass, Grass2, Road_Start_N, Road_Start_S, Road_Start_E, Road_Start_W, Road_NS, Road_EW, Road_NSEW, Road_NW, Road_SW, Road_SE, Road_NE, Road_NSE, Road_NSW, Road_EWN, Road_EWS, Crossroads, Crossroads_2, Road_Empty;
        public Sprite Yellow_House_Roadless, Yellow_House_N, Yellow_House_E, Yellow_House_S, Yellow_House_W, Blue_House_Roadless, Blue_House_N, Blue_House_E, Blue_House_S, Blue_House_W, White_House_Roadless, White_House_N, White_House_E, White_House_S, White_House_W;
        public Sprite Yellow_Warehouse_N, Yellow_Warehouse_E, Yellow_Warehouse_S, Yellow_Warehouse_W, Blue_Warehouse_N, Blue_Warehouse_E, Blue_Warehouse_S, Blue_Warehouse_W, White_Warehouse_N, White_Warehouse_E, White_Warehouse_S, White_Warehouse_W;
        public Grid grid;

        private void Start()
        {
            StaticManager.Map = this;

            if (grid == null)
                grid = GetComponent<Grid>();

        }

        public Sprite GetEmtyRandomSprite()
        {
            return Random.Range(0, 2) == 0 ? Grass : Grass2;
        }

        public Sprite GetSprite(int activeNeighs, bool IsWarehouse, bool IsHouse, Colors color)
        {
            Sprite sprt = Road_NS;
            switch (activeNeighs)
            {
                case 0:
                    {
                        sprt = Road_Empty;
                        break;
                    }
                case 1:
                    {
                        switch (color)
                        {
                            case Colors.Yellow:
                                sprt = (IsWarehouse) ? Yellow_Warehouse_N : Yellow_House_N;
                                break;
                            case Colors.Blue:
                                sprt = (IsWarehouse) ? Blue_Warehouse_N : Blue_House_N;
                                break;
                            case Colors.White:
                                sprt = (IsWarehouse) ? White_Warehouse_N : White_House_N;
                                break;
                            default:
                                sprt = Road_Start_N;
                                break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (color)
                        {
                            case Colors.Yellow:
                                sprt = (IsWarehouse) ? Yellow_Warehouse_S : Yellow_House_S;
                                break;
                            case Colors.Blue:
                                sprt = (IsWarehouse) ? Blue_Warehouse_S : Blue_House_S;
                                break;
                            case Colors.White:
                                sprt = (IsWarehouse) ? White_Warehouse_S : White_House_S;
                                break;
                            default:
                                sprt = Road_Start_S;
                                break;
                        }
                        break;
                    }
                case 3:
                    {
                        sprt = Road_NS;
                        break;
                    }
                case 4:
                    {
                        switch (color)
                        {
                            case Colors.Yellow:
                                sprt = (IsWarehouse) ? Yellow_Warehouse_E : Yellow_House_E;
                                break;
                            case Colors.Blue:
                                sprt = (IsWarehouse) ? Blue_Warehouse_E : Blue_House_E;
                                break;
                            case Colors.White:
                                sprt = (IsWarehouse) ? White_Warehouse_E : White_House_E;
                                break;
                            default:
                                sprt = Road_Start_E;
                                break;
                        }
                        break;
                    }
                case 5:
                    {
                        sprt = Road_NE;
                        break;
                    }
                case 6:
                    {
                        sprt = Road_SE;
                        break;
                    }
                case 7:
                    {
                        sprt = Road_NSE;
                        break;
                    }
                case 8:
                    {
                        switch (color)
                        {
                            case Colors.Yellow:
                                sprt = (IsWarehouse) ? Yellow_Warehouse_W : Yellow_House_W;
                                break;
                            case Colors.Blue:
                                sprt = (IsWarehouse) ? Blue_Warehouse_W : Blue_House_W;
                                break;
                            case Colors.White:
                                sprt = (IsWarehouse) ? White_Warehouse_W : White_House_W;
                                break;
                            default:
                                sprt = Road_Start_W;
                                break;
                        }
                        break;
                    }
                case 9:
                    {
                        sprt = Road_NW;
                        break;
                    }
                case 10:
                    {
                        sprt = Road_SW;
                        break;
                    }
                case 11:
                    {
                        sprt = Road_NSW;
                        break;
                    }
                case 12:
                    {
                        sprt = Road_EW;
                        break;
                    }
                case 13:
                    {
                        sprt = Road_EWN;
                        break;
                    }
                case 14:
                    {
                        sprt = Road_EWS;
                        break;
                    }
                case 15:
                    {
                        sprt = Random.Range(0, 2) == 0 ? Crossroads : Crossroads_2;
                        break;
                    }
            }

            return sprt;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class Tile : MonoBehaviour
    {
        public Tile North, South, East, West;

        public bool InmediatInstance = false;
        public bool IsActiveRoad = false;
        public bool IsWarehouse = false;
        public bool IsHouse = false;
        public bool IsBuilding
        {
            get
            {
                return IsWarehouse || IsHouse;
            }
        }
        public Colors TileColor = Colors.None;
        

        private void Start()
        {

        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButton(0))
                TryInstantiate();
            else if (Input.GetMouseButton(1))
                TryDelete();
        }

        private void OnMouseEnter()
        {
            if (Input.GetMouseButton(0))
                TryInstantiate();
            if (Input.GetMouseButton(1))
                TryDelete();
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                TryDelete();
            }
            else if (Input.GetMouseButtonDown(2))
            {
                if (StaticManager.currentCar != null)
                {
                    StaticManager.currentCar.Fin = this;
                    StaticManager.currentCar = null;
                    return;
                }

                StaticManager.currentCar = Instantiate(StaticManager.Map.CartPrefab, transform.position + (Vector3.back * 0.1f), Quaternion.identity, transform.parent).GetComponent<AStar>();
                StaticManager.currentCar.Init(this, Colors.Yellow);
            }
        }

        public float Heuristic(Tile meta)
        {
            var manhattan = Mathf.Abs(meta.transform.position.x - transform.position.x) + Mathf.Abs(meta.transform.position.y - transform.position.y);
            //var distanciafea = Vector3.Distance(transform.position, meta.transform.position);
            return manhattan;
        }

        void TryDelete()
        {
            if (IsHouse || IsWarehouse) return;
            if (IsActiveRoad)
            {
                if (!CutNeighBors()) return;
                IsActiveRoad = false;
                StaticManager.gameManager.roads++;
                GetComponent<SpriteRenderer>().sprite = StaticManager.Map.GetEmtyRandomSprite();

                
                InmediatInstance = false;
            }
        }

        public void TryInstantiate()
        {
            if (InmediatInstance) return;
            if (IsHouse || IsWarehouse) return;

            if (StaticManager.gameManager.roads > 0)
            {
                InmediatInstance = true;
                Destroy(gameObject);
                GameObject go = Instantiate(StaticManager.Map.prefab, transform.position, Quaternion.identity, transform.parent);

                Tile newTile = go.GetComponent<Tile>();
                StaticManager.gameManager.roads--;

                newTile.IsActiveRoad = true;
                newTile.InmediatInstance = true;
                newTile.NotifySorrounds();

                
            }
        }

        bool CutNeighBors()
        {
            //No se puede eliminar un nodo conectado a un edificio
            if (North != null)
                if (North.IsBuilding)
                    return false;
            if (South != null)
                if (South.IsBuilding)
                    return false;
            if (West != null)
                if (West.IsBuilding)
                    return false;
            if (East != null)
                if (East.IsBuilding)
                    return false;

            if (North != null)
            {
                North.South = null;
                North.ChangeSprite();
                North = null;
            }

            if (South != null)
            {
                South.North = null;
                South.ChangeSprite();
                South = null;
            }

            if (East != null)
            {
                East.West = null;
                East.ChangeSprite();
                East = null;
            }

            if (West != null)
            {
                West.East = null;
                West.ChangeSprite();
                West = null;
            }

            return true;
        }

        public void NotifyBuildingSorrounds(Tile N, RoadDirections dir)
        {
            switch (dir)
            {
                case RoadDirections.East:
                    East = N;
                    N.West = this;
                    break;
                case RoadDirections.West:
                    West = N;
                    N.East = this;
                    break;
                case RoadDirections.North:
                    North = N;
                    N.South = this;
                    break;
                case RoadDirections.South:
                    South = N;
                    N.North = this;
                    break;
            }

            ChangeSprite();
        }

        public void NotifySorrounds()
        {
            //north
            RaycastHit2D info = Physics2D.Raycast(transform.position + (Vector3.up * 0.7f), Vector2.up, 0.1f);
            Tile other = null;
            if (info.collider != null)
            {
                other = info.collider.gameObject.GetComponent<Tile>();

                if (other != null)
                {
                    if (other.IsActiveRoad)
                    {
                        North = other;
                        other.South = this;
                        other.ChangeSprite();
                    }
                    else if (other.IsHouse || other.IsWarehouse)
                    {
                        if (other.South == this)
                            North = other;
                    }
                }
            }

            //east
            info = Physics2D.Raycast(transform.position + (Vector3.right * 0.7f), Vector2.right, 0.1f);
            if (info.collider != null)
            {
                other = info.collider.gameObject.GetComponent<Tile>();

                if (other != null)
                {
                    if (other.IsActiveRoad)
                    {
                        East = other;
                        other.West = this;
                        other.ChangeSprite();
                    }
                    else if (other.IsHouse || other.IsWarehouse)
                    {
                        if (other.West == this)
                            East = other;
                    }
                }
            }

            //south
            info = Physics2D.Raycast(transform.position + (Vector3.down * 0.7f), Vector2.down, 0.1f);
            if (info.collider != null)
            {
                other = info.collider.gameObject.GetComponent<Tile>();

                if (other != null)
                {
                    if (other.IsActiveRoad)
                    {
                        South = other;
                        other.North = this;
                        other.ChangeSprite();
                    }
                    else if (other.IsHouse || other.IsWarehouse)
                    {
                        if (other.North == this)
                            South = other;
                    }
                }
            }

            //west
            info = Physics2D.Raycast(transform.position + (Vector3.left * 0.7f), Vector2.left, 0.1f);
            if (info.collider != null)
            {
                other = info.collider.gameObject.GetComponent<Tile>();

                if (other != null)
                {
                    if (other.IsActiveRoad)
                    {
                        West = other;
                        other.East = this;
                        other.ChangeSprite();
                    }
                    else if (other.IsHouse || other.IsWarehouse)
                    {
                        if (other.East == this)
                            West = other;
                    }
                }
            }

            ChangeSprite();
        }

        public Tile GetNeighbor(RoadDirections dir)
        {
            switch (dir)
            {
                case RoadDirections.North:
                    var info = Physics2D.Raycast(transform.position + (Vector3.up * 0.7f), Vector2.up, 0.1f);
                    return info.collider == null ? null : info.collider.gameObject.GetComponent<Tile>();
                case RoadDirections.East:
                    var info2 = Physics2D.Raycast(transform.position + (Vector3.right * 0.7f), Vector2.right, 0.1f);
                    return info2.collider == null ? null : info2.collider.gameObject.GetComponent<Tile>();
                case RoadDirections.South:
                    var info3 = Physics2D.Raycast(transform.position + (Vector3.down * 0.7f), Vector2.down, 0.1f);
                    return info3.collider == null ? null : info3.collider.gameObject.GetComponent<Tile>(); ;
                default:
                    var info4 = Physics2D.Raycast(transform.position + (Vector3.left * 0.7f), Vector2.left, 0.1f);
                    return info4.collider == null ? null : info4.collider.gameObject.GetComponent<Tile>();
            }
        }

        public void ChangeSprite()
        {

            int activeNeighs = 0;

            if (North != null)
                activeNeighs += (int)RoadDirections.North;

            if (South != null)
                activeNeighs += (int)RoadDirections.South;

            if (East != null)
                activeNeighs += (int)RoadDirections.East;

            if (West != null)
                activeNeighs += (int)RoadDirections.West;

            GetComponent<SpriteRenderer>().sprite = StaticManager.Map.GetSprite(activeNeighs, IsWarehouse, IsHouse, TileColor);
            
        }
    }

}

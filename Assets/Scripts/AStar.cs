using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace RTS
{
    public class AStar : MonoBehaviour
    {
        public static List<Tile> Visitados = new List<Tile>();
        public static List<Tile> Disponibles = new List<Tile>();
        bool EncontroMeta = false;
        public Tile Inicio, Fin, Home;
        [SerializeField]
        private float speed = 3f;
        Stack<Tile> Road = new Stack<Tile>();
        Order currOrder;
        public GameObject Indicator;

        #region variables para el GO
        SpriteRenderer render;
        public Sprite Blue, Yellow, White;
        public Colors Color;
        public bool IsAvailable = true;
        Vector3 parentoOffset;
        float offsetX = 0;
        float offsetY = 0;
        [SerializeField] LayerMask layer;
        #endregion

        // value es padre de key.
        Dictionary<Tile, Tile> Padres = new Dictionary<Tile, Tile>();

        private void Awake()
        {
            StaticManager.gameManager.units++;
            render = GetComponent<SpriteRenderer>();
            IsAvailable = true;
        }

        public void CalculateOffset()
        {
            parentoOffset = Home.transform.position - transform.position;
        }

        public void Init(Tile ini, Colors col)
        {
            Inicio = ini;
            Home = Inicio;
            Color = col;

            ChangeColor();
        }

        void ChangeColor()
        {
            switch (Color)
            {
                case Colors.Blue:
                    render.sprite = Blue;
                    break;
                case Colors.Yellow:
                    render.sprite = Yellow;
                    break;
                case Colors.White:
                    render.sprite = White;
                    break;
            }
        }

        bool SolveMaze()
        {
            Tile temp = Inicio;

            do
            {
                //Agregamos los vecinos al arreglo de disponibles

                //north
                if (temp.North != null)
                {
                    if (!Padres.ContainsKey(temp.North) && !Disponibles.Contains(temp.North))
                    {
                        Disponibles.Add(temp.North);
                        Padres.Add(temp.North, temp);
                    }
                }

                //south
                if (temp.South != null)
                {
                    if (!Padres.ContainsKey(temp.South) && !Disponibles.Contains(temp.South))
                    {
                        Disponibles.Add(temp.South);
                        Padres.Add(temp.South, temp);
                    }
                }

                //east
                if (temp.East != null)
                {
                    if (!Padres.ContainsKey(temp.East) && !Disponibles.Contains(temp.East))
                    {
                        Disponibles.Add(temp.East);
                        Padres.Add(temp.East, temp);
                    }
                }

                //west
                if (temp.West != null)
                {
                    if (!Padres.ContainsKey(temp.West) && !Disponibles.Contains(temp.West))
                    {
                        Disponibles.Add(temp.West);
                        Padres.Add(temp.West, temp);
                    }
                }

                if (Disponibles.Count < 1)
                {
                    break;
                }

                Disponibles.Sort((n1, n2) => n1.Heuristic(Fin).CompareTo(n2.Heuristic(Fin)));
                temp = Disponibles[0];

                if (temp == Fin)
                {
                    EncontroMeta = true;
                    break;
                }

                Visitados.Add(temp); //agregamos a nuestra lista
                Disponibles.Remove(temp); //ya no está disponible

            } while (temp != Fin);

            Visitados.Sort((n1, n2) => n1.Heuristic(Fin).CompareTo(n2.Heuristic(Fin)));

            if (!EncontroMeta)
            {
                Visitados.Clear();
                Disponibles.Clear();
                Padres.Clear();
                return false;
            }
            Tile temp2 = Fin;

            Road.Clear();
            while (temp2 != Inicio)
            {
                Road.Push(temp2);
                if (!Padres.ContainsKey(temp2))
                {
                    break;
                }
                temp2 = Padres[temp2];
            }
            Visitados.Clear();
            Disponibles.Clear();
            Padres.Clear();

            return true;
        }

        IEnumerator RoadDrive(bool returnHome)
        {
            while (Road.Count > 0) //avanzar todos los cuadros del camino
            {
                Vector3 desiredPosition = Road.Pop().transform.position - parentoOffset;

                //recalcular el ofset
                float offval = Mathf.Abs(parentoOffset.x);
                Vector3 currpos = transform.position + parentoOffset, nextPos = desiredPosition + parentoOffset;

                //Que se vayan por su carril segun la dirección que llevan
                if (nextPos.x > currpos.x)
                    offsetY = -offval;
                else if (nextPos.x < currpos.x)
                    offsetY = offval;

                if (nextPos.y > currpos.y)
                    offsetX = offval;
                else if (nextPos.y < currpos.y)
                    offsetX = -offval;


                parentoOffset = new Vector3(
                    offsetX,
                    offsetY,
                    parentoOffset.z
                    );

                desiredPosition = nextPos - parentoOffset;

                while (Vector3.Distance(transform.position, desiredPosition) > 0.01f)
                {
                    Vector3 direction = (desiredPosition - transform.position).normalized;
                    if (Physics.Raycast(transform.position, direction, out RaycastHit hit, layer))
                    {
                        if (hit.collider.gameObject == gameObject)
                        {
                            yield return new WaitForEndOfFrame();
                            continue;
                        }
                    }

                    transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * speed);
                    yield return new WaitForEndOfFrame();

                }
            }

            Road.Clear();

            if (returnHome)
            {
                yield return new WaitForSeconds(1f);
                currOrder.ThaWarehouse.OrderDelivered(currOrder);
                currOrder = null;
                yield return new WaitForSeconds(1f);
                Inicio = Fin;
                Fin = Home;
                SolveMaze();
                yield return StartCoroutine("RoadDrive", false);
                Inicio = Home;
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
                IsAvailable = true;
            }
        }

        private void Update()
        {
            if (IsAvailable && Inicio == Home && currOrder == null) //Si no tiene una meta buscamos a ver si hay una disponible
            {
                Indicator.GetComponent<Renderer>().material.color = UnityEngine.Color.red;
                for (int i = 0; i < WareHouse.AllOrders.Count; i++) //iteramos entre todas las ordenes
                {
                    var order = WareHouse.AllOrders[i];
                    if (order.Color == Home.TileColor && order.AssignedCar == null && IsAvailable) //hay una de mi color y s/nave asignada
                    {
                        Fin = order.Destination;
                        Inicio = Home;

                        if (SolveMaze())
                        {
                            currOrder = order;
                            order.AssignedCar = this;
                            IsAvailable = false;
                            StartCoroutine("RoadDrive", true);
                        }
                        else
                        {
                            Fin = null;
                            IsAvailable = true;
                        }
                        
                    }
                }
            }
            else
                Indicator.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
        }
    }

}

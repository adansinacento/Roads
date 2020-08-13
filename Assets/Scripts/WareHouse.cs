using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class WareHouse : MonoBehaviour
    {
        public GameObject OrderSprite;

        [SerializeField]
        Tile Nodo;
        public List<Order> Orders = new List<Order>();
        public static List<Order> AllOrders = new List<Order>();
        public List<GameObject> OrderSprites = new List<GameObject>();

        public void Awake()
        {
            OrderSprite = Resources.Load("OrderSprite") as GameObject;
        }

        public void Init(Tile n)
        {
            Nodo = n;
            StartCoroutine("NewOrder"); //Wow la banda jejeje
        }

        public void OrderDelivered(Order toRemove)
        {
            WareHouse.AllOrders.Remove(toRemove);
            Orders.Remove(toRemove);
            Destroy(OrderSprites[OrderSprites.Count - 1]);
            OrderSprites.RemoveAt(OrderSprites.Count - 1);
        }

        public void Update()
        {
            if(Orders.Count != OrderSprites.Count)
            {
                var SingleSprite = Instantiate(OrderSprite, this.transform);
                OrderSprites.Add(SingleSprite.gameObject);
                var newPos = new Vector3(-0.55f + (0.1f * OrderSprites.Count), 0.66f, -0.1f);
                SingleSprite.transform.localPosition = newPos;
            }
        }

        IEnumerator NewOrder()
        {
            yield return new WaitForSeconds(4f); //esperamos antes de epmezar a hacer pedidos

            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (Orders.Count < 11)
                {
                    yield return new WaitForSeconds(2.0f);

                    if (Random.Range(0, 3) == 0)
                    //if  (true)
                    {
                        var newOrder = RequestOrders();
                        Orders.Add(newOrder);
                        AllOrders.Add(newOrder);
                    }
                }
            }
        }

        Order RequestOrders()
        {
            return new Order() { Color = Nodo.TileColor, Destination = Nodo, ThaWarehouse = this };
        }
    }
}


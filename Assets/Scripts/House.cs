using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class House : MonoBehaviour
    {
        [SerializeField]
        Tile Nodo;
        AStar Nave1, Nave2;
        public void Init(Tile n)
        {
            Nodo = n;

            Nave1 = Instantiate(StaticManager.Map.CartPrefab, transform.position + Vector3.back, Quaternion.identity, transform).GetComponent<AStar>();
            Nave2 = Instantiate(StaticManager.Map.CartPrefab, transform.position + Vector3.back, Quaternion.identity, transform).GetComponent<AStar>();

            Nave1.Init(Nodo, Nodo.TileColor);
            Nave2.Init(Nodo, Nodo.TileColor);

            Nave1.transform.localPosition = new Vector3(-0.3f, 0.3f, -1);
            Nave2.transform.localPosition = new Vector3(0.3f, -0.3f, -1);

            Nave1.CalculateOffset();
            Nave2.CalculateOffset();
        }
    }
}


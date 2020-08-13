using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

namespace RTS
{
    public class GameManager : MonoBehaviour
    {
        public int units = 0;
        public int days = 0;
        public int roads = 0;
        public float time = 0;
        [SerializeField] int DayLength = 50;
        
        public TMP_Text text_time, text_roads, text_units, text_days;
        public Image panel;
        public Text instructions;

        private int seconds = 0;

        public GameState gameState;

        IEnumerator DisplayInstructions()
        {
            float initialAlpha = panel.color.a;
            yield return new WaitForSeconds(3);
            while (panel.color.a >= 0)
            {
                yield return new WaitForEndOfFrame();
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - (Time.deltaTime * 3));
                instructions.color = new Color(instructions.color.r, instructions.color.g, instructions.color.b, instructions.color.a - (Time.deltaTime * 3));
            }
            instructions.text = "Try to connect all houses and warehouses using the minimum ammount of roads.\nTry not to accumulate too much work on a single warehouse.";
            while (panel.color.a < initialAlpha)
            {
                yield return new WaitForEndOfFrame();
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a + (Time.deltaTime * 3));
                instructions.color = new Color(instructions.color.r, instructions.color.g, instructions.color.b, instructions.color.a + (Time.deltaTime * 6));
            }
            yield return new WaitForSeconds(3);
            while (panel.color.a >= 0)
            {
                yield return new WaitForEndOfFrame();
                panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, panel.color.a - (Time.deltaTime * 3));
                instructions.color = new Color(instructions.color.r, instructions.color.g, instructions.color.b, instructions.color.a - (Time.deltaTime * 3));
            }
            panel.enabled = false;
            instructions.enabled = false;
        }


        void Awake()
        {
            StartCoroutine("GameStart");
            StartCoroutine("DisplayInstructions");
        }

        IEnumerator GameStart()
        {
            yield return new WaitForEndOfFrame();

            var house = FindObjectOfType<House>();
            var Tile = house.gameObject.GetComponent<Tile>();
            house.Init(Tile);

            var warehouse = FindObjectOfType<WareHouse>();
            var wTile = warehouse.gameObject.GetComponent<Tile>();
            warehouse.Init(wTile);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (StaticManager.gameManager == null)
                StaticManager.gameManager = this;
            InvokeRepeating("spawnWareHouses", 12.0f, 13.0f);
            gameState = GameState.Day;
        }

        // Update is called once per frame
        void Update()
        {

            switch (gameState)
            {
                case GameState.Day:
                    if (time > DayLength) { gameState = GameState.Night; }
                    time += Time.deltaTime;
                    break;

                case GameState.Night:
                    time = 0;
                    roads += 8;
                    gameState = GameState.Day;
                    days++;
                    break;

                case GameState.None:
                    break;

                default:
                    break;
            }

            seconds = Mathf.FloorToInt(time);
            UpdateGUI();
        }


        void UpdateGUI()
        {
            text_time.text = "Time: " + seconds.ToString();
            text_roads.text = "Roads: " + roads.ToString();
            text_units.text = "Units: " + units.ToString();
            text_days.text = "Days: " + days.ToString();
        }

        void spawnWareHouses()
        {
            if(gameState == GameState.Day)
            {
                var Tiles = FindObjectsOfType<Tile>();
                var TileList = Tiles.ToList();

                var RandomNumber = Random.Range(0, TileList.Count);
                var tile = TileList[RandomNumber];
                var dir = Extensions.RandomDirection();
                var inmediateRoad = tile.GetNeighbor(dir); //el camino que va a estar unido al edificio
                if (!tile.IsBuilding && !tile.IsActiveRoad && inmediateRoad != null)
                {
                    if (inmediateRoad.IsBuilding) return; //no puede unirse a otro edificio

                    if (Random.Range(0, 5) < 2) // muchas mas casas que almacenes
                    {
                        tile.IsWarehouse = true;
                        tile.TileColor = tile.TileColor.Random(true);
                        tile.ChangeSprite();
                        WareHouse wh = tile.gameObject.AddComponent<WareHouse>();
                        wh.Init(tile);
                    }
                    else
                    {
                        tile.IsHouse = true;
                        tile.TileColor = tile.TileColor.Random(true);
                        tile.ChangeSprite();
                        House h = tile.gameObject.AddComponent<House>();
                        h.Init(tile);
                    }

                    inmediateRoad.IsActiveRoad = true;
                    inmediateRoad.InmediatInstance = true;

                    tile.NotifyBuildingSorrounds(inmediateRoad, dir);
                    inmediateRoad.NotifySorrounds();
                    //StaticManager.CameraMovement.AddPoint(tile.transform.position);
                }
            }
        }
    }

}

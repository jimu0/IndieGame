using System.Collections.Generic;
using Script.MVC.Module.Frame.ObjectPool;
using Unity.VisualScripting;
using UnityEngine;

namespace GameOfLife
{
    public class GameOfListMod : MonoBehaviour
    {
        public GameObject childObj;
        private ObjectPool<GameObject> lifePool;
        public int gridSizeX;
        public int gridSizeY;
        private int[,] runningMap = new int[19,19];

        void Start()
        {
            lifePool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestory,
                true, 10, 1000);
            RulesOfLifeGrowthSys.Instance.InitializeGroundMap();
            RandomGeneration();
        }
        GameObject OnCreate()
        {
            return Instantiate(childObj);
        }
        void OnGet(GameObject gameObj)
        {
            gameObj.SetActive(true);
            //Debug.Log("OnGet");
        }
        void OnRelease(GameObject gameObj)
        {
            gameObj.SetActive(false);
            //Debug.Log("OnRelease");
        }
        void OnDestory(GameObject gameObj)
        {
            gameObj.SetActive(false);
            //Debug.Log("OnDestroy");
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Redraw();
                //Debug.Log("AAA");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RandomGeneration();
                //Debug.Log("BBB");
            }
        }
        
        /// <summary>
        /// 随机生成
        /// </summary>
        private void RandomGeneration()
        {
            for (int y = 0; y < 19; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    int randomNumber = Random.Range(0, 4);
                    randomNumber = randomNumber < 3 ? 0 : 1;
                    RulesOfLifeGrowthSys.Instance.GroundMap[x, y] = randomNumber;
                }
            }
            TraversalPool();
        }

        /// <summary>
        /// 绘制
        /// </summary>
        private void Redraw()
        {
            TraversalMap();
            //TraversalPool();
        }
        
        /// <summary>
        /// 计算状态
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void CalculateState(int x,int y)
        {
            int a = 0;
            foreach (Vector2Int v2Int in GetSurroundingGrids(x, y))
            {
                if (RulesOfLifeGrowthSys.Instance.GroundMap[v2Int.x, v2Int.y] > 0) a++;
            }
            switch (a)
            {
                case 0:
                case 1:
                    runningMap[x, y] = -1;
                    break;
                case 2:
                    runningMap[x, y] = 0;
                    break;
                case 3 when RulesOfLifeGrowthSys.Instance.GroundMap[x, y] == 0:
                    runningMap[x, y] = 1;
                    break;
                case 3:
                    runningMap[x, y] = 0;
                    break;
                default:
                    runningMap[x, y] = -1;
                    break;
            }
            
        }
        
        /// <summary>
        /// 遍历地图
        /// </summary>
        private void TraversalMap()
        {
            if (runningMap == null) return; 
            for (int y = 0; y < 19; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    CalculateState(x, y);
                }
            }
            for (int y = 0; y < 19; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    int i = RulesOfLifeGrowthSys.Instance.GroundMap[x, y] + runningMap[x, y];
                    //Debug.Log($"{RulesOfLifeGrowthSys.Instance.GroundMap[x, y]} + {runningMap[x, y]} = {i}");
                    RulesOfLifeGrowthSys.Instance.Change(x, y, i,TraversalObj(x, y));
                }
            }
        }

        /// <summary>
        /// 遍历缓存池
        /// </summary>
        private void TraversalPool()
        {
            for (int y = 0; y < 19; y++)
            {
                for (int x = 0; x < 19; x++)
                {
                    TraversalObj(x, y);
                }
            }

        }        
        /// <summary>
        /// 缓存obj
        /// </summary>
        private GameObject TraversalObj(int x, int y)
        {
            GameObject obj;
            if (RulesOfLifeGrowthSys.Instance.GroundMap[x, y] > 0)
            {
                obj = RulesOfLifeGrowthSys.Instance.GetObj(x, y) ? RulesOfLifeGrowthSys.Instance.GetObj(x, y) : lifePool.Get();
                Transform objTfm = obj.transform;
                Vector3 newPos;
                newPos.x = x;
                newPos.y = objTfm.position.y;
                newPos.z = y;
                objTfm.position = newPos;
            }
            else
            {
                obj = RulesOfLifeGrowthSys.Instance.GetObj(x, y) ? RulesOfLifeGrowthSys.Instance.GetObj(x, y) : null;
                if (obj) lifePool.Release(obj);
                obj = null;
            }
            RulesOfLifeGrowthSys.Instance.SetObj(x, y, obj);
            return obj;
        }

        /// <summary>
        /// 获取当前格子周围的八个格子的坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        List<Vector2Int> GetSurroundingGrids(int x, int y)
        {
            List<Vector2Int> surroundingGrids = new List<Vector2Int>();

            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    // 排除中心格子
                    if (xOffset == 0 && yOffset == 0)
                        continue;

                    int newX = x + xOffset;
                    int newY = y + yOffset;

                    // 检查坐标是否在范围内
                    if (newX >= 0 && newX < 19 && newY >= 0 && newY < 19)
                    {
                        surroundingGrids.Add(new Vector2Int(newX, newY));
                    }
                }
            }

            return surroundingGrids;
        }
    }
}

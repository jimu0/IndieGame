using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameOfLife
{
    public class RulesOfLifeGrowthSys
    {
        private static RulesOfLifeGrowthSys instance;
        public static RulesOfLifeGrowthSys Instance => instance ??= new RulesOfLifeGrowthSys();
        
        public Dictionary<Vector2Int, GameObject> chessboard = new Dictionary<Vector2Int, GameObject>();
        public int[,] GroundMap = new int[19,19];
        public Vector2Int v2Int = new Vector2Int();
        public void InitializeGroundMap()
        {
            chessboard.Clear();
            for (int y = 0; y < GroundMap.GetLength(0); y++)
            {
                for (int x = 0; x < GroundMap.GetLength(1); x++)
                {
                    GroundMap[x, y] = 0;
                    v2Int.x = x;
                    v2Int.y = y;
                    chessboard.Add(v2Int, null);
                }
            }
        }

        public void Change(int x,int y,int hp,GameObject obj)
        {
            GroundMap[x, y] = hp;
            if (GroundMap[x, y] <= 0) GroundMap[x, y] = 0;
            else GroundMap[x, y] = 1;
            SetObj(x, y, obj);
        }

        public GameObject GetObj(int x,int y)
        {
            v2Int.x = x;
            v2Int.y = y;
            return chessboard[v2Int];
        }

        public void SetObj(int x, int y, GameObject obj)
        {
            v2Int.x = x;
            v2Int.y = y;
            chessboard[v2Int] = obj;
        }
    }
}

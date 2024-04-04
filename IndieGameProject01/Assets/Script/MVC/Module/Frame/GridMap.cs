using System.Collections.Generic;
using Script.MVC.Module.Class;
using UnityEngine;

namespace Script.MVC.Module.Frame
{
    public class GridMap : Actor
    {
        public Class.Character _Player;
        public Vector2Int MapSize = new Vector2Int(25,25);
        public List<int> Collisions = new List<int>();
        public GameObject Tile;
        public PlanePos planePos;
        public setPlanePos setPlanePos;
        public Dictionary<Vector2Int, int> KMK_mapdata = new Dictionary<Vector2Int, int>();
        public GameObject PlaneMesh;

        private void Awake()
        {
            Vector2Int v = new();
            for (int iy = 0; iy < MapSize.y; iy++)
            {
                v.y = iy;
                for (int ix = 0; ix < MapSize.x; ix++)
                {
                    v.x = ix;
                    KMK_mapdata.Add(v, 0);
                    //ErgodicCollision(v);
                }
            }

        
            KMK_mapdata[new Vector2Int(1, 1)] = 22;
            KMK_mapdata[new Vector2Int(3, 1)] = 22;
            KMK_mapdata[new Vector2Int(3, 2)] = 22;
            KMK_mapdata[new Vector2Int(4, 2)] = 22;
        }
        // Start is called before the first frame update
        void Start()
        {
            Instantiate(PlaneMesh);
            /*
        Vector2Int v = new Vector2Int();
        int i = 0;
        for (int iy = 0; iy < MapSize.y; iy++)
        {
            v.y = iy;
            for (int ix = 0; ix < MapSize.x; ix++)
            {
                v.x = ix;
                
                //�߶���Ϣ��¼���ԣ���ʱ��
                i++;
                int H_id = MapSize.x * v.y + v.x;
                if (i == 22)
                { planePos.MapData_Height[H_id] = -1f; }
                else if (i == 1)
                { planePos.MapData_Height[H_id] = 1f; }
                else if (i == 2)
                { planePos.MapData_Height[H_id] = 2f; }
                else if (i == 3)
                { planePos.MapData_Height[H_id] = 3f; }
                else
                { planePos.MapData_Height[H_id] = 0; }

                planePos.setPlaneMeshPos();//��������
                
            }
        }
        */
        }
    

        // Update is called once per frame
        void Update()
        {
            if (CoreFrame.KMK_update)
            {
                CoreFrame.KMK_update = false;
                //ErgodicCollision(_Player.Pos2Int); 
            }


        }

        private void ErgodicCollision(Vector2Int vi) 
        {
            int CoID = MapSize.x * vi.y + vi.x;
            if (CoID >= 0 && CoID < Collisions.Count && Collisions[CoID] > 0) 
            {
                GameObject tile = Instantiate(Tile);
                FLb.SetPosition(tile, new Vector3(vi.x, 0f, vi.y));
            }
        }
    }
}

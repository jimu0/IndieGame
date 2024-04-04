using System.Collections;
using System.Collections.Generic;
using Script.MVC.Module.Frame;
using Script.MVC.View;
using UnityEngine;

public class PlanePos : MonoBehaviour
{
    public DataManager dataManager;
    public GridMap gridMap;
    private Vector2Int mapsize = new Vector2Int();
    public GameObject player;
    public GameObject[] PlaneMesh;
    private Plane26Mesh[] Plane26M = new Plane26Mesh[4];
    public List<float> MapData_Height = new List<float>();
    public int Px, Py;
    private int PxO, PyO;
    private Vector3[] Pos = new Vector3[4];
    // Start is called before the first frame update
    private void Awake()
    {
        dataManager = GameObject.Find("Canvas").GetComponent<DataManager>();
        gridMap = GameObject.Find("BP_GridMap(Clone)").GetComponent<GridMap>();
        gridMap.planePos = this;
        for (int i = 0; i < 4; i++)
        {
            Plane26M[i] = PlaneMesh[i].GetComponent<Plane26Mesh>();
        }

        for (int i = 0; i < gridMap.MapSize.x * gridMap.MapSize.y; i++)
        {
            MapData_Height.Add(i*0.01f);
        }

    }
    void Start()
    {
        player = GameObject.Find("BP_Player(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        Px =(int)Mathf.Floor(transform.localPosition.x / 25);
        Py = (int)Mathf.Floor(transform.localPosition.z / 25);
        if (Px != PxO || Py != PyO) 
        {
            if (gridMap.MapSize.x > 1 && gridMap.MapSize.y > 1) 
            {
                float h;//临时测试
                Vector2Int v = new Vector2Int();
                if (mapsize != gridMap.MapSize) 
                { //如果地图尺寸改变了，需要重新装载初始化高度List数组
                    mapsize = gridMap.MapSize;
                    
                    MapData_Height.Clear();
                    for (int i = 0; i < mapsize.y; i++)
                    {
                        v.y = i;
                        for (int j = 0; j < mapsize.x; j++)
                        {
                            v.x = j;
                            if (gridMap.KMK_mapdata[v] == 22)
                            {
                                h = 2f;
                            }
                            else { h = 0; }
                            
                            MapData_Height.Add(h);
                        }

                    }
                }

                setPlaneMeshPos();//分配数据
            }

            PxO = Px; PyO = Py;
        }
    }

    public void setPlaneMeshPos() 
    {
        if (FLb.IsOdd(Px))
        {
            Pos[0].x = (Px + 1) * 25;
            Pos[1].x = Px * 25;
            Pos[2].x = (Px + 1) * 25;
            Pos[3].x = Px * 25;
        }
        else 
        {
            Pos[0].x = Px * 25;
            Pos[1].x = (Px + 1) * 25;
            Pos[2].x = Px * 25;
            Pos[3].x = (Px + 1) * 25;
        }
        if (FLb.IsOdd(Py))
        {
            Pos[0].z = (Py + 1) * 25;
            Pos[1].z = (Py + 1) * 25;
            Pos[2].z = Py * 25;
            Pos[3].z = Py * 25;
        }
        else
        {
            Pos[0].z = Py * 25;
            Pos[1].z = Py * 25;
            Pos[2].z = (Py + 1) * 25;
            Pos[3].z = (Py + 1) * 25;
        }
        for (int i = 0; i < 4; i++)
        {
            FLb.SetLocalPosition(PlaneMesh[i], Pos[i]);
            Plane26M[i].param = setPlane26M_param(FLb.WorldFloat2GridMapInt(Pos[i].x / 25, 1), FLb.WorldFloat2GridMapInt(Pos[i].z / 25, 1), mapsize.x);
            Plane26M[i].setMeshParam();
        }
    }
    private List<float> setPlane26M_param(int x,int y,int sizeX) 
    {
        int N;
        List<float> Lf = new List<float>();
        int Vx, Vy;
        for (int i = 0; i < 26; i++)
        {
            Vy = y * 25 + i;
            for (int j = 0; j < 26; j++)
            {
                Vx = x * 25 + j;
                N = sizeX * Vy + Vx;
                if (N >= 0 && N < MapData_Height.Count && (Vx >= 0 && Vx < mapsize.x && Vy >= 0 && Vy < mapsize.y)) 
                { Lf.Add(MapData_Height[N]); }
                else { Lf.Add(0); }
            }
        }
        return Lf;
    }

}

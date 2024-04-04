using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.MVC.Module.Frame
{
    public class CoreFrame : MonoBehaviour
    {
        public float KMK_Size = 5;//视域尺寸
        public int KMK_SizeInt;//视域尺寸
        private int KMK_SizeO;
        public List<Vector2Int> KMK_View = new List<Vector2Int>();//视域
        private List<Vector2Int> KMK_ViewN = new List<Vector2Int>();//视域新
        private List<Vector2Int> KMK_ViewO = new List<Vector2Int>();//视域旧
        public Vector2Int KMK_Pos = new Vector2Int(0,0);//当前视域位置
        private Vector2Int KMK_PosO;
        private Vector2Int KMK_v;
        public static bool KMK_update = false;

        //-----------------------------------------------------------------------------------------

        public int[] DP_ViewPort;

        private void Awake()
        {
            KMK_SizeInt= (int)Math.Round(KMK_Size, MidpointRounding.AwayFromZero);
            KMK_View = FLb.GetRilesRegion_Circle(KMK_Pos, KMK_SizeInt, KMK_View, KMK_v);
            KMK_ViewN.AddRange(KMK_View);
            KMK_ViewO.AddRange(KMK_ViewN);
            KMK_ViewN.Clear();
            KMK_PosO = KMK_Pos;
        
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }

        //调试程序_画线
        public void OnDrawGizmos()
        {
            Gizmos.DrawLine(Vector3.zero, new Vector3(0, 3f, 0));
        }

        /// <summary>
        /// 获取视域
        /// </summary>
        /// <param name="pos">目标位置</param>
        public void GetKMK_View(Vector3Int pos)
        {
            if (KMK_Pos.x != pos.x) { KMK_Pos.x = pos.x; }
            if (KMK_Pos.y != pos.z) { KMK_Pos.y = pos.z; }

            KMK_SizeInt = (int)Math.Round(KMK_Size, MidpointRounding.AwayFromZero);
            if (KMK_Pos != KMK_PosO)
            {
                KMK_update = true;
                //KMK_View = FLb.GetRilesRegion_Circle(KMK_Pos, KMK_SizeInt, KMK_View, KMK_v);
                GetNewKMK_View();
                KMK_PosO = KMK_Pos;
            }
            if (KMK_SizeInt != KMK_SizeO)
            {
                KMK_SizeInt = (int)Math.Round(KMK_Size, MidpointRounding.AwayFromZero);
                GetNewKMK_View();
                KMK_SizeO = KMK_SizeInt;
                //KMK_View = FLb.GetRilesRegion_Circle(KMK_Pos, KMK_SizeInt, KMK_View, KMK_v);
            }
            Debug.DrawLine(new Vector3(KMK_Pos.x, 0, KMK_Pos.y), new Vector3(KMK_Pos.x, 0.3f, KMK_Pos.y), Color.white);

            for (int i = 0; i < KMK_View.Count; i++)
            {
                Vector3 v = new Vector3(KMK_View[i].x, 0, KMK_View[i].y);
                Vector3 v2 = new Vector3(KMK_View[i].x, 0.3f, KMK_View[i].y);
                if (KMK_ViewN.Contains(KMK_View[i]))
                { Debug.DrawLine(v, v2, Color.green); }
                else { Debug.DrawLine(v, v2, Color.yellow); }
            

            }
        }

        public void GetNewKMK_View()
        {
            //KMK_ViewN = FLb.GetRilesRegion_Circle(KMK_Pos, KMK_SizeInt, KMK_View, KMK_v);
            KMK_ViewO.Clear();
            KMK_ViewO.AddRange(KMK_View);
            KMK_ViewN.Clear();//清空新坐标组缓存池
            Vector2Int pos = new Vector2Int();
            for (int y = 0; y < KMK_SizeInt*2+1; y++)
            {
                pos.y = y + (KMK_Pos.y - KMK_SizeInt);
                for (int x = 0; x < KMK_SizeInt*2+1; x++)
                {
                    pos.x = x + (KMK_Pos.x - KMK_SizeInt);

                    if (FLb.FindTheDistance(KMK_Pos, pos) <= KMK_Size)
                    {
                        if (KMK_ViewO.Contains(pos)) 
                        { KMK_ViewO.Remove(pos); } 
                        else 
                        { KMK_ViewN.Add(pos); }
                    }
                }
            }
            KMK_View.RemoveAll(it => KMK_ViewO.Contains(it));
            KMK_View.AddRange(KMK_ViewN);
        }



    }
}

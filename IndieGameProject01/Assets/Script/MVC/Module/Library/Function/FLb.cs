using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.EventSystems;


public static class FLb
{

    //int转化为byte[]:
    public static byte[] intToBytes(int value)
    {
        byte[] src = new byte[4];
        src[3] = (byte)((value >> 24) & 0xFF);
        src[2] = (byte)((value >> 16) & 0xFF);
        src[1] = (byte)((value >> 8) & 0xFF);//高8位
        src[0] = (byte)(value & 0xFF);//低位
        return src;
    }


    //byte[] 转化为int:
    public static int bytesToInt(byte[] src, int offset)
    {
        int value;
        value = (int)((src[offset] & 0xFF)
                | ((src[offset + 1] & 0xFF) << 8)
                | ((src[offset + 2] & 0xFF) << 16)
                | ((src[offset + 3] & 0xFF) << 24));
        return value;
    }


    /// <summary>
    ///设置位置（局部位置） 
    /// </summary>
    public static void SetLocalPosition(GameObject obj, Vector3 pos)
    {
        obj.transform.localPosition = pos;
    }

    /// <summary>
    ///设置位置（世界位置）
    /// </summary>
    public static void SetPosition(GameObject obj, Vector3 pos)
    {
        obj.transform.position = pos;
    }
    /// <summary>
    ///设置位置（世界位置）
    /// </summary>
    public static void SetPosition(GameObject obj, Vector2Int pos)
    {
        Vector3 v;v.x = pos.x;v.y = 0;v.z = pos.y;
        obj.transform.position = v;
    }

    /// <summary>
    ///创建PaperSprite组件
    /// </summary>
    public static GameObject CreatePaperGameObjectComponent(GameObject Obj, Transform transform, Transform target)
    {
        var returnValue = UnityEngine.Object.Instantiate(Obj, target);
        SetPosition(returnValue, transform.position);
        return returnValue;
    }
    public static GameObject CreatePaperGameObjectComponent(GameObject Obj, Vector3 pos, Transform target)
    {
        GameObject returnValue = UnityEngine.Object.Instantiate(Obj, target);
        SetPosition(returnValue, pos);
        return returnValue;
    }
    
    /*
    /// <summary>
    ///获取路径下的所有Prefabs
    /// </summary>
    public static string[] GetAllPrefabsPath(string path)
    {
        if (string.IsNullOrEmpty(path) || !path.StartsWith("Assets"))
            throw new ArgumentException("folderPath");

        string[] subFolders = Directory.GetDirectories(path);
        string[] guids = null;
        string[] assetPaths = null;
        int i = 0, iMax = 0;
        foreach (var folder in subFolders)
        {
            guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folder });
            assetPaths = new string[guids.Length];
            for (i = 0, iMax = assetPaths.Length; i < iMax; ++i)
            {

                assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                //Debug.Log(assetPaths[i]);
            }
            return assetPaths;
        }
        return assetPaths;
    }
    */
    
    /// <summary>
    ///通过起点和终点作为矩形范围，获取范围内所有tile坐标
    /// </summary>
    public static void GetRilesRegion(List<Vector2Int> constituencyTile, Vector2Int statePos, Vector2Int EndPos)
    {
        constituencyTile.Clear();
        if (statePos.x >= EndPos.x)
        {
            for (int x = EndPos.x; x <= statePos.x; x++)
            {
                if (statePos.y >= EndPos.y)
                {
                    for (int y = EndPos.y; y <= statePos.y; y++)
                    {
                        constituencyTile.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    for (int y = statePos.y; y <= EndPos.y; y++)
                    {
                        constituencyTile.Add(new Vector2Int(x, y));
                    }
                }

            }
        }
        else
        {
            for (int x = statePos.x; x <= EndPos.x; x++)
            {
                if (statePos.y >= EndPos.y)
                {
                    for (int y = EndPos.y; y <= statePos.y; y++)
                    {
                        constituencyTile.Add(new Vector2Int(x, y));
                    }
                }
                else
                {
                    for (int y = statePos.y; y <= EndPos.y; y++)
                    {
                        constituencyTile.Add(new Vector2Int(x, y));
                    }
                }

            }
        }
    }

    /// <summary>
    /// 通过中心点和半径作为圆形范围，获取范围内所有tile坐标（半径为整数）
    /// </summary>
    /// <param name="CentralPos">中心点</param>
    /// <param name="d">半径（单位格）</param>
    /// <returns>坐标集合</returns>
    public static List<Vector2Int> GetRilesRegion_Circle( Vector2Int CentralPos, int d)
    {
        List<Vector2Int> constituencyTile = new List<Vector2Int>();
        Vector2Int statePos = new Vector2Int(CentralPos.x - d, CentralPos.y - d);
        Vector2Int EndPos = new Vector2Int(CentralPos.x + d, CentralPos.y + d);
        Vector2Int v = new Vector2Int();

        for (int y = statePos.y; y <= EndPos.y; y++)
        {
            v.y = y;
            for (int x = statePos.x; x <= EndPos.x; x++)
            {
                 v.x = x;
                if (FindTheDistance(CentralPos, v) <= d) { constituencyTile.Add(v); }
            }
        }
        return constituencyTile;
    }
    /// <summary>
    /// 通过中心点和半径作为圆形范围，获取范围内所有tile坐标（半径为实数）
    /// </summary>
    /// <param name="CentralPos">中心点</param>
    /// <param name="d">半径</param>
    /// <returns>坐标集合</returns>
    public static List<Vector2Int> GetRilesRegion_Circle(Vector2Int CentralPos, float d)
    {
        int D = (int)Math.Round(d, MidpointRounding.AwayFromZero);
        List<Vector2Int> constituencyTile = new List<Vector2Int>();
        Vector2Int StatePos = new Vector2Int(CentralPos.x - D, CentralPos.y - D);
        Vector2Int EndPos = new Vector2Int(CentralPos.x + D, CentralPos.y + D);
        Vector2Int v = new Vector2Int();

        for (int y = StatePos.y; y <= EndPos.y; y++)
        {
            v.y = y;
            for (int x = StatePos.x; x <= EndPos.x; x++)
            {
                v.x = x;
                if (FindTheDistance(CentralPos, v) <= d) { constituencyTile.Add(v); }
            }
        }
        return constituencyTile;
    }
    /// <summary>
    /// 通过中心点和半径作为圆形范围，获取范围内所有tile坐标（半径为实数）(优化)
    /// </summary>
    /// <param name="CentralPos">中心点</param>
    /// <param name="d">半径</param>
    /// <returns>坐标集合</returns>
    public static List<Vector2Int> GetRilesRegion_Circle(Vector2Int CentralPos, int d, List<Vector2Int> Tiles, Vector2Int v)
    {
        if (Tiles == null || Tiles.Count == 0)
        {
            for (int y = CentralPos.y - d; y <= CentralPos.y + d; y++)
            {
                v.y = y;
                for (int x = CentralPos.x - d; x <= CentralPos.x + d; x++)
                {
                    v.x = x;
                    if (FindTheDistance(CentralPos, v) <= d)
                    { Tiles.Add(v); }
                }
            }
        }
        else
        {
            Tiles.Clear();
            for (int y = CentralPos.y - d; y <= CentralPos.y + d; y++)
            {
                v.y = y;
                for (int x = CentralPos.x - d; x <= CentralPos.x + d; x++)
                {
                    v.x = x;
                    if (FindTheDistance(CentralPos, v) <= d)
                    { Tiles.Add(v); }
                }
            }
        }
        return Tiles;
    }

    /// <summary>
    /// 以左下致右上获取以中心点为中心的方形范围内tile坐标
    /// </summary>
    /// <param name="CentralPos">中心点</param>
    /// <param name="d">半径（单位格）</param>
    /// <returns>坐标集合</returns>
    public static List<Vector2Int> GetRilesRegion_Square(Vector2Int CentralPos, int d)
    {
        List<Vector2Int> constituencyTile = new List<Vector2Int>();
        Vector2Int v = new Vector2Int();
        for (int iy = 0; iy <= d * 2; iy++)
        {
            v.y = CentralPos.y - d + iy;
            for (int ix = 0; ix <= d * 2; ix++)
            {
                v.x = CentralPos.x - d + ix;
                constituencyTile.Add(v);
            }
        }
        return constituencyTile;
    }

    /// <summary>
    /// 以中心点螺旋式获取周围tile坐标
    /// </summary>
    /// <param name="o">装箱</param>
    /// <param name="CentralPos">中心点</param>
    /// <param name="L">长度</param>
    /// <returns>o</returns>
    public static void GetRilesRegion_Screw(List<Vector2Int> o,Vector2Int CentralPos, int L)
    {
        o.Clear();
        bool N = true;
        bool B = true;
        Vector2Int v = CentralPos;
        o.Add(v);
        int a = 1;
        while (o.Count < L)
        {
            int iB = 0;
            while (iB < 2 && o.Count < L)
            {
                if (B)
                {
                    int ii = 0;
                    while (ii < a && o.Count < L)
                    {
                        if (N) { v.x++; } else { v.x--; }
                        o.Add(v); //Debug.Log(v);
                        ii++;
                    }
                }
                else
                {
                    int ii = 0;
                    while (ii < a && o.Count < L)
                    {
                        if (N) { v.y++; } else { v.y--; }
                        o.Add(v); //Debug.Log(v);
                        ii++;
                    }
                }
                B = !B;
                iB++;
            }
            a++;
            N = !N;
        }
    }

    /// <summary>
    /// 生成逐渐不稳定的排列数据
    /// </summary>
    /// <param name="Rs">临时槽</param>
    /// <param name="T">数量</param>
    /// <param name="A">区间起始</param>
    /// <param name="B">区间扩展</param>
    /// <param name="C">区间张量</param>
    /// <returns></returns>
    public static void GraduallyUnstableArrange(List<int>Rs, int T, int A, float B, float C) 
    {
        Rs.Clear();
        if (T > 0)
        {
            int Distance = 0;
            Rs.Add(Distance);
            for (int i = 1; i < T; i++)
            {
                int a = A + 1;
                float c = i * C;
                int b = a + (int)Mathf.Floor(Mathf.Abs(B + c));
                int r = new System.Random(Seed()).Next(a, b);
                Distance += r;
                Rs.Add(Distance);
            }
        }
    }

    /// <summary>
    ///获取笔刷范围内的tile坐标
    /// </summary>
    public static void GetTheTilesPosInTheConstantRegion(List<Vector2Int> constituencyTile, Vector2Int statePos, List<Vector2Int[]> brushs, int brushID)
    {
        constituencyTile.Clear();
        for (int i = 0; i <= brushs[brushID].Length - 1; i++)
        {
            constituencyTile.Add(statePos + brushs[brushID][i]);
        }
    }
    /// <summary>
    ///获取图章范围内的tile坐标
    /// </summary>
    public static void GetTheTilesPosInTheConstantRegion(List<Vector2Int> constituencyTile, Vector2Int statePos, List<Vector2Int> localTile)
    {
        constituencyTile.Clear();
        for (int i = 0; i < localTile.Count; i++)
        {
            constituencyTile.Add(statePos + localTile[i]);
        }
    }

    /// <summary>
    ///得到一个范围内的随机值
    /// </summary>
    //public static int getRandomInt(int min, int max)
    //{
    //var r = Random.nextInt(max - min + 1) + min;
    //return r;
    //}

    /// <summary>
    ///判断交互对象是否是UI
    ///</summary>
    public static bool IsPointerOverUIObject()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }
    public static bool IsPointerOverUIObject(GameObject ObjUI)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results.Count > 0) 
            {
                foreach (var item in results)
                {
                    if (ObjUI != null)
                    {
                        if (item.gameObject.transform.IsChildOf(ObjUI.transform))
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        return false;

    }

    /// <summary>
    /// 取段值，根据长度获得当前值浮点位置的整数段位置
    /// </summary>
    /// <param name="r">长度值</param>
    /// <param name="a">当前浮点值</param>
    /// <returns>当前整数段位置</returns>
    public static float RoundT(float r,float a)
    {
        return Mathf.Round(a / r) * r;
    }

    /// <summary>
    /// 世界float转换为GridMapInt
    /// </summary>
    /// <param name="v">浮点值</param>
    /// <returns>整数值</returns>
    public static int WorldFloat2GridMapInt(float v)
    {
        return (int)Mathf.Round(v);
    }
    /// <summary>
    /// 世界float转换为GridMapInt（带比例）
    /// </summary>
    /// <param name="v">浮点值</param>
    /// <param name="p">世界比例</param>
    /// <returns>整数值</returns>
    public static int WorldFloat2GridMapInt(float v,float p) 
    {
        return (int)Mathf.Round(v / p);
    }
    /// <summary>
    /// 世界Vector3转换为GridMapInt（带比例）
    /// </summary>
    /// <param name="v"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector2Int WorldVector3ToVector2Int(Vector3 v,float p) 
    {
        Vector2Int vi = new Vector2Int();
        vi.x = WorldFloat2GridMapInt(v.x, p);
        vi.y = WorldFloat2GridMapInt(v.z, p);
        return vi;
    }
    /// <summary>
    /// GridMapInt转换为Vector3（带比例）
    /// </summary>
    /// <param name="vi"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector3 Vector2Int2WorldVector3(Vector2Int vi,float p) 
    {
        return new Vector3(vi.x*p, 0, vi.y*p);
    }
    public static Vector3Int SetPosInt(Vector3 pos)
    {
        return new Vector3Int(WorldFloat2GridMapInt(pos.x), WorldFloat2GridMapInt(pos.y), WorldFloat2GridMapInt(pos.z));
    }

    /// <summary>
    ///设置Gameobject缩放
    /// </summary>
    public static void setLocalScale(GameObject obj, float p)
    {
        obj.transform.localScale = new Vector3(p, p, p);
    }

    /// <summary>
    /// 求两点间距
    /// </summary>
    /// <param name="a">位置1</param>
    /// <param name="b">位置2</param>
    /// <returns></returns>
    public static float FindTheDistance(Vector2Int a,Vector2Int b ) 
    {
        return (float)Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2));
    }
    /// <summary>
    /// 求两点间距(优化)
    /// </summary>
    /// <param name="ax">位置1X</param>
    /// <param name="ay">位置1Y</param>
    /// <param name="bx">位置2X</param>
    /// <param name="by">位置2Y</param>
    /// <returns></returns>
    public static float FindTheDistance(int ax,int ay, int bx,int by)
    {
        return (float)Math.Sqrt(Math.Pow((ax - bx), 2) + Math.Pow((ay - by), 2));
    }

    /// <summary>
    /// 通过0-1的概率基数获得bool值
    /// </summary>
    /// <param name="f">概率基数(0-1)</param>
    /// <returns></returns>
    public static bool ProbabilitySieve(float f) 
    {
        if (f > 0 & f < 1)
        {
            int a = (int)(Math.Round(f, 2) * 100);//比率范围，取小数点后两位
            int r = new System.Random(Seed()).Next(1,100);//当此值小于a时，即为中标
            if (r < a) { return true; } else { return false; }
        }
        else if (f >= 1) { return true; } else { return false; }//如果大于等于1只为true，小于等于0只为false
    }

    /// <summary>
    /// 并发随机数种子
    /// </summary>
    /// <returns></returns>
    public static int Seed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return Math.Abs(BitConverter.ToInt32(bytes, 0));
    }

    /// <summary>
    /// 判断奇偶
    /// </summary>
    /// <param name="n">int</param>
    /// <returns></returns>
    public static bool IsOdd(int n)
    {
        return Convert.ToBoolean(n & 1);
    }
}

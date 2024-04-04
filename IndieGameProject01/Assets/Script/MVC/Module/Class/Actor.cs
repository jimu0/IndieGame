using UnityEngine;

namespace Script.MVC.Module.Class
{
    public class Actor : MonoBehaviour
    {
        public Vector3Int PosInt;//栅格位置
        void Awake()
        {
        
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetPosInt(Vector3 pos)
        {
            PosInt.x = FLb.WorldFloat2GridMapInt(pos.x);
            PosInt.y = FLb.WorldFloat2GridMapInt(pos.y);
            PosInt.z = FLb.WorldFloat2GridMapInt(pos.z);
        }
    }
}

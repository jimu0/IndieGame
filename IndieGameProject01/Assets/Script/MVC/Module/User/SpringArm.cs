using UnityEngine;
namespace Script.MVC.Module.User
{
    public class SpringArm : MonoBehaviour
    {

        //目标臂长度
        //摄像机偏移
        //滞后（启用摄像机延迟，摄像机延迟速度、最大延迟距离）
        //private GameObject self = SpringArm.self;
        public GameObject _follower;
        public float _rockerArmLength;
        public Vector3 _Offset = new Vector3(0, 0, 0);
        private Vector3 _position;

        void Start() { }

        public void MouseScrollWheel(float ral)
        {
            _rockerArmLength = ral;
            _position.z = -_rockerArmLength;
            _follower.transform.localPosition = _position;
        }
    }
}

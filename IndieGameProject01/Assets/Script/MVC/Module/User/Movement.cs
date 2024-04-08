using UnityEngine;
namespace Script.MVC.Module.User
{
    public class Movement : MonoBehaviour
    {

        public GameObject owner;
        public Vector3 locationVector;
        public Vector3 offsetVector;
        public GameObject springArm;
        public GameObject mainCameraObj;
        //public GameObject UnitTT_Frame;
        public Camera camera0;
        //private Transform SpringArm_Tsf;
        //private Transform UnitTT_Frame_Tsf;

        void Start()
        {
            owner = gameObject;
            //SpringArm_Tsf = SpringArm.transform;
            //UnitTT_Frame_Tsf = UnitTT_Frame.transform;
            camera0 = mainCameraObj.GetComponent<Camera>();
        }

        void Update()
        {
            
        }

        public void setVal(GameObject ow, Vector3 ol, Vector3 of)
        {
            owner = ow;
            locationVector = ol;
            offsetVector = of;
        }
        public void moveOffset(GameObject ow, Vector3 of)
        {
            owner = ow;
            offsetVector = of;

            owner.transform.position += of;
        }
        public void moveTo(GameObject ow, Vector3 ol)
        {
            owner = ow;
            locationVector = ol;

            owner.transform.position = ol;
        }

        public void MoveUp(float speed) 
        {
            owner.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        public void MoveDown(float speed)
        {
            owner.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        public void MoveLeft(float speed)
        {
            owner.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        public void MoveRight(float speed)
        {
            owner.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        /// <summary>
        /// 设置单位朝向
        /// </summary>
        /// <param name="y">朝向（Rotation.y）</param>
        public void SetRotationY(float y) 
        {
            Quaternion rotation = owner.transform.rotation;
            Vector3 tfmRotation = rotation.eulerAngles;
            tfmRotation.y = y;
            Quaternion quit = rotation;
            quit.eulerAngles = tfmRotation;
            rotation = quit;
            owner.transform.rotation = rotation;
        }
        /// <summary>
        /// 设置镜头属性
        /// </summary>
        /// <param name="rx">镜头摇臂的俯度（Rocation.x）</param>
        /// <param name="ry">鼠标指针Icon的方向(-Rocation.y)</param>
        /// <param name="fov">摄像机FOV</param>
        public void SetCameraMod(float rx,float ry,float fov) 
        {
            SetRotationY(ry);
            Vector3 r;
            //改镜头摇臂的俯度
            Quaternion localRotation = springArm.transform.localRotation;
            r = localRotation.eulerAngles;
            r.x = rx;
            Quaternion q = localRotation;
            q.eulerAngles = r;
            localRotation = q;
            springArm.transform.localRotation = localRotation;

            //改摄像机FOV(FOV=10f或FOV=60f)
            camera0.fieldOfView = fov;
        }
    }
}

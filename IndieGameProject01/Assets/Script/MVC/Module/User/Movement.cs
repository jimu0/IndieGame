using UnityEngine;
namespace Script.MVC.Module.User
{
    public class Movement : MonoBehaviour
    {

        public GameObject owner;
        public Vector3 locationVector;
        public Vector3 offsetVector;
        public GameObject SpringArm;
        public GameObject MainCamera;
        //public GameObject UnitTT_Frame;
        public Camera MainCamera_camera;
        //private Transform SpringArm_Tsf;
        //private Transform UnitTT_Frame_Tsf;
        // Start is called before the first frame update

        void Start()
        {
            owner = this.gameObject;
            //SpringArm_Tsf = SpringArm.transform;
            //UnitTT_Frame_Tsf = UnitTT_Frame.transform;
            MainCamera_camera = MainCamera.GetComponent<Camera>();
        }

        // Update is called once per frame
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
        /// <param name="y">朝向（Rocation.y）</param>
        public void SetRocationY(float y) 
        {
            Vector3 tfm_Rotation = owner.transform.rotation.eulerAngles;
            tfm_Rotation.y = y;
            Quaternion Quat = owner.transform.rotation;
            Quat.eulerAngles = tfm_Rotation;
            owner.transform.rotation = Quat;
        }
        /// <summary>
        /// 设置镜头属性
        /// </summary>
        /// <param name="Rx">镜头摇臂的俯度（Rocation.x）</param>
        /// <param name="Ry">鼠标指针Icon的方向(-Rocation.y)</param>
        /// <param name="FOV">摄像机FOV</param>
        public void SetCameraMod(float Rx,float Ry,float FOV) 
        {
            SetRocationY(Ry);
            Vector3 R;
            Quaternion Q;
            //改镜头摇臂的俯度
            R = SpringArm.transform.localRotation.eulerAngles;
            R.x = Rx;
            Q = SpringArm.transform.localRotation;
            Q.eulerAngles = R;
            SpringArm.transform.localRotation = Q;

            //改摄像机FOV(FOV=10f或FOV=60f)
            MainCamera_camera.fieldOfView = FOV;
        }
    }
}

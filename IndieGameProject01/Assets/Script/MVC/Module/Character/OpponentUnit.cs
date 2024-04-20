using Script.MVC.Controller.Interface;
using Script.MVC.Module.Class;
using Script.MVC.Module.Ejector;
using UnityEngine;

namespace Script.MVC.Module.Character
{
    public class OpponentUnit : Biota, I_OpponentUnit
    {
        private void Awake()
        {
            gun = GetComponent<Gun>();
            gun.owner = this;
            reactionSpeed = 0.1f;
            attackSpeed = 0.2f;
            
            pos_gunStart = transform.Find("mod/pos_gunStart").gameObject;
            pos_gunEnd = transform.Find("mod/pos_gunEnd").gameObject;
        }

        // void Start()
        // {
        // }

        void Update()
        {
            Vector3 position = gameObject.transform.position;
            posGunStart = position;
            posGunEnd.x = position.x + orient_Preset;
            posGunEnd.y = position.y + 1;
            pos_gunStart.transform.SetPositionAndRotation(posGunStart,pos_gunStart.transform.rotation);
            pos_gunEnd.transform.SetPositionAndRotation(posGunEnd,pos_gunEnd.transform.rotation);

        }



        public void Move(float x)
        {
            float mSpeed = 4f;
            //transform.Translate(Vector3.forward * vertical * m_speed * Time.deltaTime);//?? ??
            transform.Translate(Vector3.right * x * mSpeed * Time.deltaTime);//????
            //Debug.Log(x.ToString());
        }

        public void Squat(float y)
        {
            judgeTheSquat(y);
        }

        public void Defend()
        {
            ReadyDefend();
            longPress_Defend = true;
            //Debug.Log("??????????");
        }
        public void Defend_Cancel()
        {
            CancelDefend();
            longPress_Defend = false;
        }
        public void Attack()
        {
            ReadyAttack();
        }
        public void Attack_Cancel()
        {
            //??????????????

            //timer_AttackBack.ReStart(attackSpeed);
        }

        public void Flash()
        {
            //Debug.Log("????????");
        }

        //-------------------------------------------------------------------------------------------------


    }
}

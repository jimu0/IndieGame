using Script.MVC.Module.Class;
using Script.MVC.Other.Interface;
using UnityEngine;

namespace Script.MVC.Module.Character
{
    public class OpponentUnit : Gladiatus, I_OpponentUnit
    {


        private void Awake()
        {
            _Tsf_ams = _Obj_ams.transform;
            _ams_SpR = _Obj_ams.GetComponent<SpriteRenderer>();
            reactionSpeed = 0.1f;
            attackSpeed = 0.2f;
            //if (behavior == Behavior.stand) { }
        }

        // Start is called before the first frame update
        void Start()
        {
            ConstructionTimer();
        }

        // Update is called once per frame
        void Update()
        {
            _Tsf_ams.localPosition = _ams_pos;
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

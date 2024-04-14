using Script.MVC.Module.Class;
using UnityEngine;

namespace Script.MVC.Module.Character
{
    public class PlayerUnit : Gladiatus,I_PlayerUnit
    {

        private Rigidbody2D rig;
    

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
            rig = GetComponent<Rigidbody2D>();
            ConstructionTimer();
        }

        // Update is called once per frame
        void Update()
        {
            _Tsf_ams.localPosition = _ams_pos;
            //landing = 
            Land();
        }
    


        public void Move(float x)
        {
            float mSpeed = 4f;
            //transform.Translate(Vector3.forward * vertical * m_speed * Time.deltaTime);//�� ��
            if (behavior == Behavior.Attack)
            {
                if (!isGround)
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * Move_SpeedAttenuation)); //����
                }
            }
            else
            {
                transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * Move_SpeedAttenuation)); //����
            }


            SetOrient(x, TargetLocked);
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
            //取消攻击

            //timer_AttackBack.ReStart(attackSpeed);
        }

        public void Flash()
        {
            //Debug.Log("闪现");
        }

        public void Jump() 
        {
            if (!isGround) { return; }
            ReadyJump(rig);
        }

        //-------------------------------------------------------------------------------------------------

        public void CC_isGround() 
        {
            isGround = true;
        }
        public void CC_isNotGround() 
        {
            isGround = false;
        }
    }
}

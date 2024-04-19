using System.Collections;
using Script.MVC.Module.Class;
using Script.MVC.Module.Ejector;
using Script.MVC.Other.Timer2;
using UnityEngine;

namespace Script.MVC.Module.Character
{
    
    public class PlayerUnit : Biota,I_PlayerUnit
    {

        private Rigidbody2D rig;
        private Timer timer_Jump;
        private Vector2 flyHw = Vector2.zero;
        public float flySpeed = 5f; // 推进器推力大小
        private float jumpForce;
        private bool isJumpD;
        private bool isFlying;
        public float ux;
        private void Awake()
        {
            
            rig = GetComponent<Rigidbody2D>();
            gun = GetComponent<Gun>();
            gun.owner = this;
            //_Tsf_ams = _Obj_ams.transform;
            //_ams_SpR = _Obj_ams.GetComponent<SpriteRenderer>();
            reactionSpeed = 0.1f;
            attackSpeed = 0.05f;
            //if (behavior == Behavior.stand) { }
            pos_gunStart = transform.Find("mod/pos_gunStart").gameObject;
            pos_gunEnd = transform.Find("mod/pos_gunEnd").gameObject;

        }

        void Start()
        {
            ConstructionTimer();
            // 跳跃蓄力时间
            timer_Jump = Timer.Start(1f, (float timeUpdata) => { jumpForce = timeUpdata * 6f;
            }, () => {}, 0.01f);
            
        }

        void Update()
        {
            //_Tsf_ams.localPosition = _ams_pos;
            Vector3 position = gameObject.transform.position;
            posGunStart = position;
            posGunEnd.x = position.x + (orient_Preset * (1 - Mathf.Abs(posGunEnd0))) + (ux * Mathf.Abs(posGunEnd0));
            posGunEnd.y = position.y + posGunEnd0;
            pos_gunStart.transform.SetPositionAndRotation(posGunStart,pos_gunStart.transform.rotation);
            pos_gunEnd.transform.SetPositionAndRotation(posGunEnd,pos_gunEnd.transform.rotation);

            //landing = 
            Land();
            //flying();
        }
    


        public void Move(float x)
        {
            ux = x;
            Moving(x,jumpForce);
        }

        public void Squat(float y)
        {
            if(isFlying) flyHw.y = y;
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

        public void JumpD()
        {
            isJumpD = true;
            if (isGround)
            {
                timer_Jump.ReStart();
            }
        }
        public void Jump()
        {
            if (isJumpD && isGround)
            {
                isJumpD = false;
                timer_Jump.ReStart();
            }
        }
        public void JumpU()
        {
            isJumpD = false;
            timer_Jump.Pause();
            if (isGround) ReadyJump(rig, jumpForce);
            jumpForce = 0;
            
        }

        public void Skill1(bool kd)
        {
            //handgun
        }

        public void Skill2()
        {
            
        }

        /// <summary>
        /// 飞行
        /// </summary>
        public void Skill3()
        {
            //Debug.Log("飞行");//forceMagnitude
            isFlying = true; 
            rig.velocity = Vector2.zero;
            //timer_fly.ReStart(0.4f);
            Vector2 thrustDirection = new Vector2(1, 1); // 推力方向
            rig.AddForce(thrustDirection.normalized * 20, ForceMode2D.Impulse);
        }

        public void Skill4()
        {
            
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
        
        
        
        // IEnumerator ApplyForce()
        // {
        //     // 持续 2 秒
        //     float duration = 2.0f;
        //     float elapsedTime = 0.0f;
        //     
        //     while (elapsedTime < duration)
        //     {
        //         // 等待下一帧
        //         yield return null;
        //     }
        //     isFlying = false;
        // }

        // void flying()
        // {
        //     if (isFlying)
        //     {
        //         if (flyHw != Vector2.zero)
        //         {
        //             // 根据方向键输入来控制飞行方向
        //             Vector2 movement = new Vector2(flyHw.x, flyHw.y).normalized * flySpeed;
        //             rig.velocity = movement;
        //         }
        //         else
        //         {
        //             rig.velocity = Vector2.zero;
        //         }
        //     }
        // }


    }
}

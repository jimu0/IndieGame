using System;
using System.Collections;
using Script.MVC.Module.Collision;
using Script.MVC.Module.Ejector;
using Script.MVC.Other.Timer2;
using UnityEngine;
namespace Script.MVC.Module.Class
{
    public class Biota : Character
    {
        /// <summary>
        /// ?????
        /// </summary>
        public enum Behavior
        {
            /// <summary>
            /// ???
            /// </summary>
            Stand,
            /// <summary>
            /// ????
            /// </summary>
            Defend,
            /// <summary>
            /// ????
            /// </summary>
            Attack,
            /// <summary>
            /// ???
            /// </summary>
            BeAttackde,
            /// <summary>
            /// ?ßﬁ?
            /// </summary>
            Parry,
            /// <summary>
            /// ???
            /// </summary>
            Rigidity,
            /// <summary>
            /// ????
            /// </summary>
            Die
        }
        /// <summary>
        /// ??¶À??
        /// </summary>
        public enum Buff { }
        /// <summary>
        /// ???
        /// </summary>
        public GameObject _targetObject = null;
        /// <summary>
        /// ????????
        /// </summary>
        public Behavior behavior = Behavior.Stand;

        //????
        /// <summary>
        /// ?????
        /// </summary>
        public float _HP;
        /// <summary>
        /// ?????
        /// </summary>
        public float _SP;
        /// <summary>
        /// ????
        /// </summary>
        public float strength;
        /// <summary>
        /// ????
        /// </summary>
        public float armor;
        /// <summary>
        /// //????
        /// </summary>
        public float agility;
        /// <summary>
        /// ????
        /// </summary>
        public float attackSpeed = 0.1f;
        /// <summary>
        /// ????
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// ??????????
        /// </summary>
        public float reactionSpeed = 0.1f;
        /// <summary>
        /// ????????
        /// </summary>
        public struct Attr { }



        //????????????

        //public GameObject _Obj_ams;//????????????????
        //public Transform _Tsf_ams;//??????????Transform
        //public Vector3 _ams_pos = new Vector3(0.4f, 0, 0);//????????????????box???pos?
        //public SpriteRenderer _ams_SpR;//????????????????????????
        //Color DefendForward_color = new Color(255, 255, 255, 1);//?????????????????_???
        public int squatValue = 0;//????ID
        public bool longPress_Defend = false;//???????????
        private float squatYO;//squatUpdate?ß÷?????ßÿ?
        public Timer timer_SquatUp;//????????????
        public Timer timer_SquatDown;//????????????
        public Timer timer_Squat;//????????????
        public Timer timer_AttackForward;//?????????
        public Timer timer_AttackBack;//??????????
        public Timer timer_DefendForward;//?????????

        public GameObject _Obj_ColTrigger;//?????????Obj
        public CollisionTrigger _CollisionTrigger;//??????????????????
        public bool Attack_Trigger = false;//????????????
        public float Move_SpeedAttenuation = 1;//?????????????
        private int orient = 1;//???????
        public int orient_Preset = 1;//???????????
        private Vector3 OrientValue = new Vector3(1,1,1);//?????
        public bool OrientSwitch = false;//????????
        public bool TargetLocked = false;//?????????

        public float force = 8; //??????
        //private bool isGrounded = false;//??????
        public Gun gun;//?
        public Vector2 posGunStart = Vector2.zero;
        public Vector2 posGunEnd = Vector2.one;
        public float posGunEnd0;
        public GameObject pos_gunStart;
        public GameObject pos_gunEnd;
        public Rigidbody2D rig;
        
        private void Awake()
        {
            //_Tsf_ams = _Obj_ams.transform;
            //_ams_SpR = _Obj_ams.GetComponent<SpriteRenderer>();
            //reactionSpeed = 0.1f;
            //attackSpeed = 0.2f;
            ////if (behavior == Behavior.stand) { }
        }

        void Start()
        {
            //ConstructionTimer();


        }




        

        void Update()
        {
            //Land();
            //_Tsf_ams.localPosition = _ams_pos;
            //if (Col_AttackTrigger) { }
        }

        private void TimerReStart_SquatUp()
        {
            if (timer_SquatUp == null)
            {
                timer_SquatUp = Timer.Start(reactionSpeed, (float timeUpdata) =>
                {
                    posGunEnd0 = timeUpdata / reactionSpeed;
                }, () => { squatValue = 1; }, 0.01f);
            }
            else
            {
                timer_SquatUp.ReStart();
            }
            // Debug.Log("????????"); 
        }
        private void TimerReStart_SquatDown() 
        {
            if (timer_SquatDown == null)
            {
                timer_SquatDown = Timer.Start(reactionSpeed, (float timeUpdata) => 
                    {
                        posGunEnd0 = -(timeUpdata / reactionSpeed);
                    }, () =>
                    {
                        squatValue = -1;
                    },
                    0.01f);
            }
            else
            {
                timer_SquatDown.ReStart();
            }
            // Debug.Log("????????");
        }
        private void TimerReStart_Squat()
        {
            if (timer_Squat == null)
            {
                timer_Squat = Timer.Start(reactionSpeed, (float timeUpdata) =>
                {
                    posGunEnd0 =  squatValue * ((reactionSpeed-timeUpdata) / reactionSpeed);
                }, () =>
                {
                    squatValue = 0;
                    posGunEnd0 = 0;
                }, 0.01f);
            }
            else
            {
                timer_Squat.ReStart();
            }
            // Debug.Log("???????"); 
        }
        private void TimerReStart_DefendForward()
        {
            if (timer_DefendForward == null)
            {
                timer_DefendForward = Timer.Start(attackSpeed, (float timeUpdata) => 
                    {
                    
                        //timer_SquatUp?.Pause(); timer_Squat?.Pause(); timer_SquatDown?.Pause();
                    },
                    () =>
                    {
                    
                    },
                    0.01f);
            }
            else
            {
                timer_DefendForward.ReStart();
            }
            // Debug.Log("??????????"); 
        }
        private void TimerReStart_AttackBack()
        {
            if (timer_AttackBack == null)
            {
                timer_AttackBack = Timer.Start(attackSpeed, (float timeUpdata) =>
                    {
                        timer_SquatUp?.Resume(); timer_Squat?.Resume(); timer_SquatDown?.Resume();
                    },
                    () =>
                    {
                        OrientSwitch = true;
                        Move_SpeedAttenuation = 1;
                        if (longPress_Defend) {  SetOrient(orient_Preset, false); ReadyDefend(); } else { behavior = Behavior.Stand; }
                    },
                    0.01f);
            }
            else
            {
                timer_AttackBack.ReStart();
            }
            //Debug.Log("???????????"); 
        }
        private void TimerReStart_AttackForward()
        {
            if (timer_AttackForward == null)
            {
                timer_AttackForward = Timer.Start(attackSpeed, (float timeUpdata) =>
                    {
                        Move_SpeedAttenuation = 1;
                        timer_SquatUp?.Pause(); timer_Squat?.Pause(); timer_SquatDown?.Pause();
                    },
                    () =>
                    {
                        TriggerAttack(gun, posGunStart, posGunEnd);
                        TimerReStart_AttackBack();
                    },
                    0.01f);
            }
            else
            {
                timer_AttackForward.ReStart();
            }
            // Debug.Log("??????????"); 
        }

        private void StopTimer()
        {
            timer_SquatUp?.Pause();
            timer_SquatDown?.Pause();
            timer_Squat?.Pause();
            timer_AttackForward?.Pause();
            timer_AttackBack?.Pause();
            timer_DefendForward?.Pause();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="jumpForce"></param>
        /// <param name="jumpDamping">??????</param>
        /// <param name="attaackDamping">?????????????</param>
        public void Moving(float x, float jumpForce,float jumpDamping,float  attaackDamping)
        {
            //if(isFlying) flyHw.x = x;
            float mSpeed = 4f;
            Vector2 pos = Vector2.zero;
            //transform.Translate(Vector3.forward * vertical * m_speed * Time.deltaTime);//?? ??
            if (behavior == Behavior.Attack)
            {
                if (isGround)
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation/attaackDamping)));//???????????????
                    //pos.x = x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation / 3.6f);
                    //var b = transform.position.x + x*10;
                    //pos.x = b;
                }
                else
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * Move_SpeedAttenuation));//????????ß·??????
                    //var b = transform.position.x + x*10;
                    //pos.x = b;

                }
            }
            else
            {
                if (jumpForce > 0)
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation*jumpDamping))); //????
                    //pos.x = x * mSpeed * Time.deltaTime * (Move_SpeedAttenuation / 3.6f);

                }
                else
                {
                    transform.Translate(Vector3.right * (x * mSpeed * Time.deltaTime * Move_SpeedAttenuation)); //????
                    //pos.x = x * mSpeed * Time.deltaTime * Move_SpeedAttenuation;

                }

            }
            SetOrient(x, TargetLocked);
            //Debug.Log(x.ToString());
        }
        
        /// <summary>
        /// ???????
        /// </summary>
        /// <param name="i">X???????</param>
        /// <param name="TargetLocked">?????????</param>
        public void SetOrient(float i,bool TargetLocked)
        {
            if (TargetLocked) 
            {
                orient_Preset = orient; 
            } 
            else 
            {
                //????(???????????????????????????,?????????????????longPress_Defend?????true??????????ŸÇ????????)
                if (!longPress_Defend)
                {
                    if (i > 0) { orient_Preset = 1; }
                    else if (i < 0) { orient_Preset = -1; }
                    else { orient_Preset = 1; }
                }


                if (OrientSwitch)
                {
                    //if (orient != orient_Preset) { orient = orient_Preset; }
                    if (i > 0)
                    { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                    else if (i < 0)
                    { orient = -1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                    //else { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }

                    
                    //GameObject muzzle = gameObject.transform.Find("mod/muzzle").gameObject;
                    //if(gunMuzzle) gunMuzzle.transform.localScale = OrientValue;
                }
            }

        }

        /// <summary>
        /// ????????ßÿ?
        /// </summary>
        /// <param name="y">y??????</param>
        public void judgeTheSquat(float y)
        {
            if (y >= 0.34)
            { //?????ßÿ?
                if (squatYO >= 0.34) { }
                else
                {
                    timer_Squat?.Pause();
                    timer_SquatDown?.Pause();
                    TimerReStart_SquatUp();
                    squatYO = y;
                }
            }
            else if (y <= -0.34)
            { //?????ßÿ?
                if (squatYO <= -0.34) { }
                else
                {
                    timer_SquatUp?.Pause();
                    timer_Squat?.Pause();
                    TimerReStart_SquatDown();
                    squatYO = y;
                }
            }
            else
            { //?ßﬁ??ßÿ?
                if (squatYO > -0.34 && squatYO < 0.34) { }
                else
                {
                    timer_SquatUp?.Pause();
                    timer_SquatDown?.Pause();
                    TimerReStart_Squat();
                    squatYO = y;
                }
            }
            //Debug.Log(y.ToString());
        }

        /// <summary>
        /// ??????
        /// </summary>
        public void ReadyJump(Rigidbody2D rig, float jumpForce) 
        {
            //if (float.Parse(string.Format("{0:F1}", transform.position.y)) > 0.5f) return;
            if (behavior == Behavior.Stand || behavior == Behavior.Attack) 
            {
                //Debug.Log("jump!");
                rig.velocity += Vector2.up * (force + jumpForce);
            }
        }


        /// <summary>
        /// ???????
        /// </summary>
        public void ReadyAttack()
        {
            //DefendForward_color.b = 255;
            //_ams_SpR.color = DefendForward_color;
            if (behavior == Behavior.Stand || behavior == Behavior.Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Attack;
                TimerReStart_AttackForward();
                //Move_SpeedAttenuation = 0.01f;
            }
        }
        /// <summary>
        /// ???????
        /// </summary>
        public void ReadyDefend()
        {
        
            if (behavior == Behavior.Stand)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                TimerReStart_DefendForward();
            }
            if (behavior == Behavior.Attack && longPress_Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                TimerReStart_DefendForward();
            }
        }
        /// <summary>
        /// ???????
        /// </summary>
        public void CancelDefend()
        {

            if (behavior == Behavior.Defend)
            {
                timer_DefendForward?.Resume();
                OrientSwitch = true;
                behavior = Behavior.Stand;
                //DefendForward_color.b = 255;
                //_ams_SpR.color = DefendForward_color;
            }
            //Debug.Log("?????????????");
        }

        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="weapon">????/Gun</param>
        /// <param name="weaponStart">???????¶À??</param>
        /// <param name="weaponEnd">???????¶À??</param>
        private void TriggerAttack(Gun weapon,Vector2 weaponStart,Vector2 weaponEnd)
        {
            if (!weapon) return;
            Attack_Trigger = true;
            weapon.SetGunPos(weaponStart,weaponEnd);
            weapon.Fire();
            _CollisionTrigger.Col_ON(2.1f*orient,0,4.2f,0.8f);
            StartCoroutine(CancelAttack(0.1f));
        }
        /// <summary>
        /// ???ßø????????????????
        /// </summary>
        /// <param name="time">???????/s</param>
        /// <returns>??????</returns>
        private IEnumerator CancelAttack(float time) 
        {
            yield return new WaitForSecondsRealtime(time);
            //yield return null;
            _CollisionTrigger.Col_OFF();
            Attack_Trigger = false;
        }
        /// <summary>
        /// ?????????????
        /// </summary>
        /// <param name="collision">???????</param>
        public void Attack_Hit(Collider2D collision)
        {
        
            if (Attack_Trigger)//Attack_Trigger
            {
                //_targetObject = collision.gameObject;//?????ßÿ???????????¶À
                //Debug.Log("??");
                Attack_Trigger = false;

            }
        }
        /// <summary>
        /// ????????????
        /// </summary>
        /// <param name="source">????????¶À</param>
        /// <param name="damage">????</param>
        public void Be_Hit(GameObject source,float damage) 
        {
            if (_HP-damage <= 0)
            {
                _HP = 0;
                Die();
                return;
            }
            _HP -= damage;
            Debug.Log("????????"+ source.name+"???????????????????"+_HP.ToString()+"??");
        }
        /// <summary>
        /// ????????
        /// </summary>
        public void Die()
        {
            StopTimer();
            behavior = Behavior.Die;
            if(!CompareTag("Player")) gameObject.SetActive(false);
            Debug.Log(this.name+"????");
        }
        /// <summary>
        /// ????????
        /// </summary>
        public void Resurrection()
        {
            behavior = Behavior.Stand;
            gameObject.transform.SetLocalPositionAndRotation(new Vector3(0,6,0),gameObject.transform.rotation);
            gameObject.SetActive(true);
            Debug.Log(this.name+"????");
        }

        
        void OnTriggerEnter2D(Collider2D other)
        {
            // ???????????????????????????
            if (other.CompareTag("Ground"))
            {
                isGround = true;
                //Debug.Log("OnTriggerExit2D:Grounded");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // ????????????????
            if (other.CompareTag("Ground"))
            {
                isGround = false;
                //Debug.Log("OnTriggerExit2D:Floating");
            }
        }

        void FixedUpdate()
        {
            // ??FixedUpdate?ßﬂ???????????
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!rig) return;
            // ???????????????
            ContactPoint2D[] contacts = collision.contacts;
            foreach (ContactPoint2D contact in contacts)
            {
                // ???????????????
                Vector2 relativeVelocity = contact.relativeVelocity;
                relativeVelocity.y *= 0.3f;
                // ????????
                rig.AddForce(relativeVelocity, ForceMode2D.Impulse);
            }
        }
        
        
        
        public LayerMask groundLayer; // ??1????7¶À??????????7???
        public void Land()
        {
            Vector2 pos = Vector2.down;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.down, 0.6f);
            isGround = false;
            //isPlatform = false;
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == 6)
                {
                    isGround = true; // ????????Layer 6
                    if (hit.collider.CompareTag("Platform"))
                    {
                        boxCollider2D = (BoxCollider2D)hit.collider;
                        isPlatform = true;
                    }
                    else
                    {
                        boxCollider2D = null;
                        isPlatform = false;
                    }

                    break; // ????????Layer 6?????»…???????????
                }
            }
            Color rayColor = isGround ? Color.red : Color.green;
            Debug.DrawRay(pos, Vector2.down * 0.6f, rayColor);
        }

    }
}

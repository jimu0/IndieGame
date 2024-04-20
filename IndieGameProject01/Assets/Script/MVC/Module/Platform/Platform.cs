using System;
using Script.MVC.Module.Class;
using Script.MVC.Module.Collision;
using Script.MVC.Other.Timer2;
using UnityEngine;

namespace Script.MVC.Module.Platform
{
    public class Platform : Actor
    {
        public GameObject obj;
        public BoxCollider2D boxCollider;
        public Rigidbody2D rig;
        public CollisionEnter2D collisionEnter;
        private Vector2 pos;//最新位置
        private Vector2 anchor;//锚点
        public enum MovMode { Null,Linear,Circling,Rectangle }//移动方式(线性,圆形,矩形)
        public MovMode movMode = MovMode.Null;
        public bool reBackSwitch;//巡回开关
        private bool reBack;//巡回或重置
        public float movSpeed = 2;//速度
        public float waitStartTime;//开始前的等待时间
        public float recoveryTime= 3;//复活的生命周期
        public Transform point0;
        public Transform point1;
        public bool destructible;//可破坏物
        public bool dieMode;//脆弱的死亡重生模式
        public bool disposable;//一次性的
        private Timer timerMoving;
        public Timer TimerLifeTime;//消失、复活的生命周期
        public Timer TimerLifeTime2;
        public Timer TimerLifeTime3;
        private int timerLifeTimeId;
        public Animation anim;
        public float lifeTimeDie = 2;
        public string dieAnim="Ani_platform_unsteadiness";
        public GameObject mesh;
        public GameObject dieMesh;
        void Start()
        {
            switch (movMode)
            {
                case MovMode.Null:
                    
                    break;
                case MovMode.Linear:
                    TimerStart_Moving(MovMode.Linear);
                    break;
                case MovMode.Circling:
                    break;
                case MovMode.Rectangle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //collisionEnter.

        }

        void Update()
        {
            if (movMode == MovMode.Linear) LineDrawer();
            
            //obj.transform.SetPositionAndRotation(pos,obj.transform.rotation);
            
        
        }
        
        void OnDestroy()
        {
            if(timerMoving!=null) timerMoving.Cancel();
            if(TimerLifeTime!=null)TimerLifeTime.Cancel();
            if(TimerLifeTime2!=null)TimerLifeTime2.Cancel();
            if(TimerLifeTime3!=null)TimerLifeTime3.Cancel();
        }
        //Ani_platform_unsteadiness
        public void TimerStart_LifeTime(float time,string animName)
        {
            if (TimerLifeTime == null)
            {
                TimerLifeTime = Timer.Start(time, (_) => {}, () =>
                {
                    float length;
                    if (anim)
                    {
                        anim.Play(animName);
                        AnimationState state = anim[animName];
                        length = state.length;
                    }
                    else
                    {
                        length = 0.01f;
                    }
                    TimerStart_LifeTime2(length);
                },0.01f);
            }
            else
            {
                TimerLifeTime.ReStart();
            }
        }
        void TimerStart_LifeTime2(float time)
        {
            if (TimerLifeTime2 == null)
            {
                TimerLifeTime2 = Timer.Start(time, (_) => {}, () =>
                {
                    //boxCollider.enabled = false;
                    boxCollider.isTrigger = true;
                    mesh.SetActive(false);
                    dieMesh.SetActive(true);
                    TimerStart_LifeTime3(recoveryTime);
                },0.01f);
            }
            else
            {
                TimerLifeTime2.ReStart();
            }
        }
        void TimerStart_LifeTime3(float time)
        {
            if (TimerLifeTime3 == null)
            {
                TimerLifeTime3 = Timer.Start(time, (_) => {}, () =>
                {
                    //boxCollider.enabled = true;
                    boxCollider.isTrigger = false;
                    mesh.SetActive(true);
                    dieMesh.SetActive(false);
                },0.01f);
            }
            else
            {
                TimerLifeTime3.ReStart();
            }
        }

        void TimerStart_Moving(MovMode mode)
        {
            if (timerMoving == null)
            {
                timerMoving = Timer.Start(movSpeed, (update) => SetMovStart(mode,update), () => SetMovEnd(mode),0.01f);
            }
            else
            {
                timerMoving.ReStart(waitStartTime);
            }
        }

        private void SetMovStart(MovMode mode,float update)
        {
            switch (mode)
            {
                case MovMode.Null:
                    break;
                case MovMode.Linear:
                    break;
                case MovMode.Circling:
                    break;
                case MovMode.Rectangle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            pos = reBack ? Vector2.Lerp(point0.position, point1.position, update/movSpeed) : Vector2.Lerp(point1.position, point0.position, update/movSpeed);
            if(rig) rig.MovePosition(pos);
        }
        private void SetMovEnd(MovMode mode)
        {
            if(reBackSwitch) reBack = !reBack;
            TimerStart_Moving(mode);
        }












        void LineDrawer()
        {
            if (point0 != null && point1 != null)
            {
                Debug.DrawLine(point0.position, point1.position, Color.gray);
            }
        }
    }
}

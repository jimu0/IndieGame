using System.Buffers;
using Script.MVC.Controller.Interface;
using Script.MVC.Module.Class;
using Unity.VisualScripting;
using UnityEngine;
using Timer = Script.MVC.Other.Timer2.Timer;

namespace Script.MVC.Controller.PawnController
{
    public class AIController : MonoBehaviour
    {
        private I_OpponentUnit opponentUnit;
        public Biota owner;
        [SerializeField] private bool auto = true;//自动触发
        [SerializeField] private float fireDeviationAngle;//发射角度偏移
        [SerializeField] private float fireRate = 2.0f; // 发射频率
        [SerializeField] private float nextFireTime; // 下一次发射时间
        private Timer attackTimer;
        private bool cancelTimerLock = true;//判断角色死亡后，批准执行的锁
        private void Awake()
        {
            opponentUnit = GetComponent<I_OpponentUnit>();
        }
        void Start()
        {
            attackTimer = Timer.Start(fireRate, (float timeUpdata) => {}, Attack, 0.01f);
        }

        void OnEnable()
        {
            cancelTimerLock = true;
        }

        void Update()
        {
            if (owner.behavior == Biota.Behavior.Die)
            {
                if(cancelTimerLock) attackTimer.Cancel();
                cancelTimerLock = false;
                return;//角色死了
            }
            
            if (auto && attackTimer.currentTimerState == Timer.TimerState.Stop)
            {
                attackTimer.ReStart();
            }
            
        }

        void Attack()
        {
            opponentUnit.Attack();
        }
    }
}

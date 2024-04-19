using Script.MVC.Module.Class;
using UnityEngine;

namespace Script.MVC.Controller.PawnController
{
    public class PlayerController2D : MonoBehaviour
    {
        private I_PlayerUnit playerUnit;
        public Biota owner;
        private float horizontal;//x
        private float vertical;//y
        //public Pawn BP_Player;
        private bool cancelTimerLock = true;//判断角色死亡后，批准执行的锁
        private void Awake()
        {
            playerUnit = GetComponent<I_PlayerUnit>();
        }
        // void Start()
        // {
        //
        // }
        
        void OnEnable()
        {
            cancelTimerLock = true;
        }
        void Update()
        {
            if (owner.behavior == Biota.Behavior.Die)
            {
                if (cancelTimerLock)
                {
                    cancelTimerLock = false;
                }
                return;//角色死了
            }
            
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            if (horizontal != 0)
            {
                playerUnit.Move(horizontal);
            }
            if (vertical != 0) 
            {
                playerUnit.Squat(vertical);
            }

            if (Input.GetKey(KeyCode.H))
            {
                playerUnit.Attack();
            }
            if (Input.GetKeyUp(KeyCode.H))
            {
                playerUnit.Attack_Cancel();
            }

            if (Input.GetKeyDown(KeyCode.U)) 
            {
                playerUnit.Defend();
            }
            if (Input.GetKeyUp(KeyCode.U))
            {
                playerUnit.Defend_Cancel();
            }

            if (Input.GetKeyDown(KeyCode.J)) 
            {
                playerUnit.JumpD();
            }
            if (Input.GetKey(KeyCode.J)) 
            {
                playerUnit.Jump();
            }
            if (Input.GetKeyUp(KeyCode.J)) 
            {
                playerUnit.JumpU();
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                playerUnit.Flash();
            }

            if (Input.GetKey(KeyCode.K))
            {
                playerUnit.Skill1(true);
            }
            if (Input.GetKeyUp(KeyCode.K))
            {
                playerUnit.Skill1(false);
            }
        
            if (Input.GetKeyDown(KeyCode.Y))
            {
                playerUnit.Skill2();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                playerUnit.Skill3();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                playerUnit.Skill4();
            }
        }
    }
}

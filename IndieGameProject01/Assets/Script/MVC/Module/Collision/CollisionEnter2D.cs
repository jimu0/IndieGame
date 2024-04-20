using Script.MVC.Other.Timer2;
using UnityEngine;
namespace Script.MVC.Module.Collision
{
    public class CollisionEnter2D : MonoBehaviour
    {
        public Platform.Platform platform;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="collision">被碰撞者</param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            if(!platform.dieMode)return;
            // 检查碰撞的 GameObject 是否属于 biology或player
            if (collision.gameObject.layer == LayerMask.NameToLayer("biology")||collision.gameObject.layer == LayerMask.NameToLayer("player"))
            {
                if(platform.TimerLifeTime?.currentTimerState != Timer.TimerState.Pause) platform.TimerLifeTime?.Pause();
                if(platform.TimerLifeTime?.currentTimerState != Timer.TimerState.Pause) platform.TimerLifeTime2?.Pause();
                if(platform.TimerLifeTime?.currentTimerState != Timer.TimerState.Pause) platform.TimerLifeTime3?.Pause();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        { 
            platform.TimerLifeTime?.Resume();
            platform.TimerLifeTime2?.Resume();
            platform.TimerLifeTime3?.Resume();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(!platform.dieMode)return;
            // 检查碰撞的 GameObject 是否属于 biology或player
            if (collision.gameObject.layer == LayerMask.NameToLayer("biology")||collision.gameObject.layer == LayerMask.NameToLayer("player"))
            {
                platform.TimerStart_LifeTime(platform.lifeTimeDie,platform.dieAnim);
            }
        }
        
    }
}

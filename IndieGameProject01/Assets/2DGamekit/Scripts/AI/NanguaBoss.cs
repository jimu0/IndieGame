using System.Collections;
using UnityEngine;

namespace _2DGamekit.Scripts.AI
{
    public class NanguaBoss : MonoBehaviour
    {
        public GameObject headObj1;
        public GameObject headObj2;
        public GameObject headObj3;
        public GameObject headObj4;
        public GameObject headObj5;

        public Transform headPos1;
        public Transform headPos2;
        public Transform headPos3;
        public Transform headPos4;
        public Transform headPos5;
        public bool head1Alive=true;
        public bool head2Alive=true;
        public bool head3Alive=true;
        public bool head4Alive=true;
        public bool head5Alive=true;
        private int head = 5;
        public GameObject dieAinObj;
        public GameObject aliveObj;

        public float deathDelay = 8.0f; // 死亡后延迟时间
        private Coroutine deathCoroutine;
        public GameObject hit1;
        private void Start()
        {
            head = 5;
        }

        private void Update()
        {
            if(head1Alive) headObj1.transform.SetPositionAndRotation(headPos1.position,Quaternion.Euler(0,0,0));
            if(head2Alive) headObj2.transform.SetPositionAndRotation(headPos2.position,Quaternion.Euler(0,0,0));
            if(head3Alive) headObj3.transform.SetPositionAndRotation(headPos3.position,Quaternion.Euler(0,0,0));
            if(head4Alive) headObj4.transform.SetPositionAndRotation(headPos4.position,Quaternion.Euler(0,0,0));
            if(head5Alive) headObj5.transform.SetPositionAndRotation(headPos5.position,Quaternion.Euler(0,0,0));

        }

        public void CutOffAHead()
        {
            head--;
            if (head > 0) return;
            head = 0;
            Die();
        }

        private void Die()
        {
            aliveObj.SetActive(false);
            dieAinObj.SetActive(true);
            StartDeathTimer();
        }
    
    
        //启动计时器
        private void StartDeathTimer()
        {
            if (deathCoroutine != null)
                StopCoroutine(deathCoroutine);

            deathCoroutine = StartCoroutine(DeathCountdown());
        }
    
        private IEnumerator DeathCountdown()
        {
            yield return new WaitForSeconds(deathDelay);

            goHome();
        }
    
        // 死亡后执行方法
        private void goHome()
        {
            Debug.Log("Player is going home!");
        }

        public void Jineng(GameObject obj,bool b)
        {
            if (head <= 0) return;
            obj.SetActive(b);
            deathCoroutine = StartCoroutine(DeathJineng(obj));
        }
        
        private IEnumerator DeathJineng(GameObject obj)
        {
            yield return new WaitForSeconds(4);
            obj.SetActive(false);
        }
    }
}

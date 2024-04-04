using UnityEngine;

namespace Script.MVC.Other.Timer
{
    public class TimerTest : MonoBehaviour
    {

        public float timerL=5;
        public float timerS=1;
        public bool isjump = true;

        // Start is called before the first frame update
        void Start()
        {

            GlobalTimer.Instance.AddIntervalTimer(timerL, timerS, abc, (float time) => { Debug.Log(time); });
            //() => { Debug.Log("wanshi"); }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void abc() 
        {
            Debug.Log("������");
        }
    }
}

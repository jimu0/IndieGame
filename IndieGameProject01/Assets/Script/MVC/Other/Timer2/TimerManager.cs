using UnityEngine;

namespace Script.MVC.Other.Timer2
{
    [AutoSingleton(true, "TimerManager")]
    public class TimerManager : MonoSingleton<TimerManager>
    {
        /*
    private void Awake()
    { 
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        Debug.Log("TimerManager,Awake!!");
    }
    */
        /// <summary>
        /// ������ʱ������
        /// </summary>
        /// <param name="data"></param>
        public TimerDriver CreateTimerDriver(Timer data)
        {
            GameObject driverTarget = new GameObject();
            driverTarget.name = "TimeDriver";
            driverTarget.transform.SetParent(transform);
            TimerDriver driver = driverTarget.AddComponent<TimerDriver>();
            driver.InitTimerDriver(data);
            return driver;
        }

    }
}

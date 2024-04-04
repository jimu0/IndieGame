using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.MVC.Other.Timer
{
    public class GlobalTimer : MonoBehaviour
    {
        private static GlobalTimer instance;
        public static GlobalTimer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<GlobalTimer>();
                }
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(GlobalTimer).ToString());
                    instance = obj.AddComponent<GlobalTimer>();
                }
                return instance;
            }
        }


        private List<TimerData> mUseTimerDatas = new List<TimerData>(); //正在使用的TimerData
        private List<TimerData> mNotUseTimerDatas = new List<TimerData>(); //空闲的TimerData


        void Update()
        {
            for (int i = 0; i < mUseTimerDatas.Count; ++i)
            {
                if (!mUseTimerDatas[i].Update())   //mUseTimerDatas[i].Update()这个函数return  true说明没更新成功
                {
                    //没更新成功，mUseTimerDatas长度减1，所以需要--i
                    --i;
                }
            }
        }


        //尝试从空闲池中取一个TimerData
        private TimerData GetTimerData()
        {
            TimerData data = null;
            if (mNotUseTimerDatas.Count <= 0)  //先检测未使用队列中是否存在
            {
                data = new TimerData();
            }
            else
            {
                data = mNotUseTimerDatas[0]; //存在的话就使用，然后从未使用队列中删除
                mNotUseTimerDatas.RemoveAt(0);
            }

            mUseTimerDatas.Add(data); //添加到使用队列

            return data;
        }

        //创建一个计时器
        public TimerData AddTimer(float _duration, Action endCallBack, bool _isIgnoreTime = false)
        {
            TimerData data = GetTimerData();
            data.Init(_duration, endCallBack, _isIgnoreTime);

            return data;
        }

        //创建一个重复型计时器
        public TimerData AddIntervalTimer(float _duration, float _interval, Action _endCallBack, Action<float> _intervalCallBack, bool _isIgnoreTime = false)
        {
            TimerData data = GetTimerData();
            data.Init(_duration, _endCallBack, _isIgnoreTime, _interval, _intervalCallBack);

            return data;
        }

        protected void Clear(TimerData data) //这个函数用来将使用过的TimeData从mUseTimerDatas中Remove，放进mNotUseTimerDatas
        {
            if (mUseTimerDatas.Remove(data))
            {
                mNotUseTimerDatas.Add(data);
            }
            else
            {
                Debug.LogWarning("GlobalTimer not find TimerData");
            }
        }



        public class TimerData
        {
            private float mDuration;
            private float mInterval;
            private Action mEndCallBack;
            private Action<float> mIntervalCallBack;
            private bool isIgnoreTime;
            private float mRunTime;
            private float mRunIntervalTime;


            //初始化
            public void Init(float _duration, Action _endCallBack, bool _isIgnoreTime = false, float _interval = -1f, Action<float> _intervalCallBack = null)
            {
                mDuration = _duration; //持续时间
                mInterval = _interval; //重复间隔
                mEndCallBack = _endCallBack; //结束回调
                mIntervalCallBack = _intervalCallBack; //每次重复回调
                isIgnoreTime = _isIgnoreTime;//是否忽略时间
                mRunTime = 0;  //计时器
                mRunIntervalTime = 0;//间隔计时器
            }

            //更新
            public bool Update()
            {
                float deltaTime = isIgnoreTime ? Time.unscaledDeltaTime : Time.deltaTime;  //判断在运用timescale时，时间的计算方法

                mRunTime += deltaTime;
                mRunIntervalTime += deltaTime;

                if (mIntervalCallBack != null) //用于重复间隔调用时
                {
                    if (mRunIntervalTime >= mInterval)
                    {
                        mRunIntervalTime -= mInterval;
                        mIntervalCallBack(mDuration - mRunTime);
                    }
                }

                if (mRunTime >= mDuration)  //用于单次调用时
                {
                    if (mEndCallBack != null)
                    {
                        mEndCallBack();
                    }
                    Clear();
                    return false;
                }
                return true;
            }

            public void Clear()
            {
                instance.Clear(this);
            }

            public void AddEndCallBack(Action _endCallBack)
            {
                mEndCallBack += _endCallBack;
            }
        }
    }
}

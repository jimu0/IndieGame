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


        private List<TimerData> mUseTimerDatas = new List<TimerData>(); //����ʹ�õ�TimerData
        private List<TimerData> mNotUseTimerDatas = new List<TimerData>(); //���е�TimerData


        void Update()
        {
            for (int i = 0; i < mUseTimerDatas.Count; ++i)
            {
                if (!mUseTimerDatas[i].Update())   //mUseTimerDatas[i].Update()�������return  true˵��û���³ɹ�
                {
                    //û���³ɹ���mUseTimerDatas���ȼ�1��������Ҫ--i
                    --i;
                }
            }
        }


        //���Դӿ��г���ȡһ��TimerData
        private TimerData GetTimerData()
        {
            TimerData data = null;
            if (mNotUseTimerDatas.Count <= 0)  //�ȼ��δʹ�ö������Ƿ����
            {
                data = new TimerData();
            }
            else
            {
                data = mNotUseTimerDatas[0]; //���ڵĻ���ʹ�ã�Ȼ���δʹ�ö�����ɾ��
                mNotUseTimerDatas.RemoveAt(0);
            }

            mUseTimerDatas.Add(data); //��ӵ�ʹ�ö���

            return data;
        }

        //����һ����ʱ��
        public TimerData AddTimer(float _duration, Action endCallBack, bool _isIgnoreTime = false)
        {
            TimerData data = GetTimerData();
            data.Init(_duration, endCallBack, _isIgnoreTime);

            return data;
        }

        //����һ���ظ��ͼ�ʱ��
        public TimerData AddIntervalTimer(float _duration, float _interval, Action _endCallBack, Action<float> _intervalCallBack, bool _isIgnoreTime = false)
        {
            TimerData data = GetTimerData();
            data.Init(_duration, _endCallBack, _isIgnoreTime, _interval, _intervalCallBack);

            return data;
        }

        protected void Clear(TimerData data) //�������������ʹ�ù���TimeData��mUseTimerDatas��Remove���Ž�mNotUseTimerDatas
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


            //��ʼ��
            public void Init(float _duration, Action _endCallBack, bool _isIgnoreTime = false, float _interval = -1f, Action<float> _intervalCallBack = null)
            {
                mDuration = _duration; //����ʱ��
                mInterval = _interval; //�ظ����
                mEndCallBack = _endCallBack; //�����ص�
                mIntervalCallBack = _intervalCallBack; //ÿ���ظ��ص�
                isIgnoreTime = _isIgnoreTime;//�Ƿ����ʱ��
                mRunTime = 0;  //��ʱ��
                mRunIntervalTime = 0;//�����ʱ��
            }

            //����
            public bool Update()
            {
                float deltaTime = isIgnoreTime ? Time.unscaledDeltaTime : Time.deltaTime;  //�ж�������timescaleʱ��ʱ��ļ��㷽��

                mRunTime += deltaTime;
                mRunIntervalTime += deltaTime;

                if (mIntervalCallBack != null) //�����ظ��������ʱ
                {
                    if (mRunIntervalTime >= mInterval)
                    {
                        mRunIntervalTime -= mInterval;
                        mIntervalCallBack(mDuration - mRunTime);
                    }
                }

                if (mRunTime >= mDuration)  //���ڵ��ε���ʱ
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

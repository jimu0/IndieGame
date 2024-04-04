using UnityEngine;

namespace Script.MVC.Other.Timer2
{
    public class TimerDriver : MonoBehaviour
    {

        private Timer m_currentTimer;

        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        private float CurrentTime
        {
            get { return m_currentTimer.ignoreTimeScale ? Time.realtimeSinceStartup : Time.time; }
        }

        /// <summary>
        /// ��ʼ��ʱʱ��
        /// </summary>
        private float m_startTime;
        /// <summary>
        /// ��ǰ��ʱʱ��
        /// </summary>
        private float m_currentTime;
        /// <summary>
        /// �Ѽ�ʱ��ʱ��
        /// </summary>
        private float m_passedTime;
        /// <summary>
        /// ��ʱʱ����
        /// </summary>
        private float m_duration;

        /// <summary>
        /// �ϴμ��ʱ��
        /// </summary>
        private float m_lastTime;

        /// <summary>
        /// �Ƿ�ʼ��ʱ
        /// </summary>
        private bool m_isTimerPrepared;

        /// <summary>
        /// �Ѽ�ʱ����
        /// </summary>
        private int m_passedCount;

        /// <summary>
        /// �Ƿ��ʱ
        /// </summary>
        private bool IsTiming
        {
            get
            {
                return m_currentTimer.currentTimerState != Timer.TimerState.Pause &&
                       m_currentTimer.currentTimerState != Timer.TimerState.Stop;
            }
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="data"></param>
        public void InitTimerDriver(Timer data)
        {
            m_currentTimer = data;
            m_currentTimer.InitDriverAction(OnPause, OnResume);
        }

        private void Update()
        {
            if (m_currentTimer == null)
                return;
            if (!IsTiming)
            {
                if (m_currentTimer.currentTimerState == Timer.TimerState.Stop)//������ʱ
                    CloseTimerDriver();
                return;
            }
            if (m_currentTimer.currentTimerState == Timer.TimerState.Start)//��ʼ��ʱ����
            {
                m_startTime = CurrentTime + m_currentTimer.delayTime;
                m_passedTime = 0;
                m_lastTime = 0;
                m_isTimerPrepared = false;
                m_passedCount = 0;
                m_currentTimer.SetTimerState(Timer.TimerState.Prepare);
            }
            else //��ʼ��ʱ
            {
                m_duration = CurrentTime - m_startTime; //��ʱ��ʱ��
                if (m_duration < 0) //�ȴ���ʱ
                {

                }
                else //��ʽ��ʱ
                {
                    if (!m_isTimerPrepared)
                    {
                        m_isTimerPrepared = true;
                        m_currentTimer.OnStart();
                        m_currentTimer.SetTimerState(Timer.TimerState.Timing);
                    }

                    if (m_duration - m_lastTime >= m_currentTimer.intervalTime) //��ʱ���
                    {
                        m_lastTime = m_duration;
                        m_currentTime = m_duration + m_passedTime; //��ʱʱ��
                        m_currentTimer.OnUpdate(m_currentTime);
                        if (m_currentTime >= m_currentTimer.duration) //��ʱ���
                        {
                            m_currentTimer.OnStop();
                            if (m_currentTimer.repeatCount < 0) //���޴μ�ʱ
                            {
                                OnRepeat();
                                m_currentTimer.SetTimerState(Timer.TimerState.Prepare);
                            }
                            else if (m_currentTimer.repeatCount > 0) //�ظ���ʱ
                            {
                                OnRepeat();
                                m_passedCount++;
                                if (m_passedCount >= m_currentTimer.repeatCount) //�ﵽ�ظ�����
                                    CloseTimerDriver();
                                else
                                    m_currentTimer.SetTimerState(Timer.TimerState.Prepare);
                            }
                            else
                            {
                                CloseTimerDriver();
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// ��ͣ
        /// </summary>
        private void OnPause()
        {
            m_passedTime = m_duration + m_passedTime;
            m_lastTime = m_duration - m_lastTime;
        }

        /// <summary>
        /// ����
        /// </summary>
        private void OnResume()
        {
            if (!m_isTimerPrepared)
                m_currentTimer.SetTimerState(Timer.TimerState.Prepare);
            else
                m_currentTimer.SetTimerState(Timer.TimerState.Timing);

            m_startTime = CurrentTime;
            m_lastTime = -m_lastTime;
        }

        /// <summary>
        /// �ظ���ʱ
        /// </summary>
        private void OnRepeat()
        {
            m_startTime = CurrentTime + m_currentTimer.repeatTime;
            m_passedTime = 0;
            m_lastTime = 0;
            m_isTimerPrepared = false;
        }

        /// <summary>
        /// �رռ�ʱ��
        /// </summary>
        private void CloseTimerDriver()
        {
            m_currentTimer = null;
            Destroy(gameObject);
        }
        /// <summary>
        /// ��ͣ��Ϸʱ
        /// </summary>
        /// <param name="isPause"></param>
        private void OnApplicationPause(bool isPause)
        {
            if (isPause)
                m_currentTimer.Pause();
            else
                m_currentTimer.Resume();
        }

    }
}

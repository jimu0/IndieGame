using System;

namespace Script.MVC.Other.Timer2
{
    public class Timer
    {

        /// <summary>
        /// ??????
        /// </summary>
        public float delayTime { get; private set; }
        /// <summary>
        /// ???????
        /// </summary>
        public float duration { get; private set; }
        /// <summary>
        /// ????????
        /// </summary>
        internal float intervalTime { get; private set; }
        /// <summary>
        /// ???????
        /// </summary>
        public int repeatCount { get; private set; }
        /// <summary>
        /// ??????
        /// </summary>
        public float repeatTime { get; private set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public int repeatedCount { get; private set; }
        /// <summary>
        /// ????? ???????
        /// </summary>
        public float passedTime { get; private set; }

        internal enum TimerState
        {
            /// <summary>
            /// ??????
            /// </summary>
            Start,
            /// <summary>
            /// ??????
            /// </summary>
            Prepare,
            /// <summary>
            /// ?????
            /// </summary>
            Timing,
            /// <summary>
            /// ??????
            /// </summary>
            Pause,
            /// <summary>
            /// ?????
            /// </summary>
            Stop,
        }
        /// <summary>
        /// ???????
        /// </summary>
        internal TimerState currentTimerState { get; private set; }

        /// <summary>
        /// ?????????????
        /// </summary>
        internal bool ignoreTimeScale { get; private set; }

        /// <summary>
        /// ???
        /// </summary>
        private Action onStart;
        /// <summary>
        /// ???
        /// </summary>
        private Action onPause;
        /// <summary>
        /// ????
        /// </summary>
        private Action onResume;
        /// <summary>
        /// ???
        /// </summary>
        private Action onCompleted;
        /// <summary>
        /// ???
        /// </summary>
        private Action onCancel;
        /// <summary>
        /// ????????
        /// </summary>
        private Action<float> onTiming;

        /// <summary>
        /// ????
        /// </summary>
        private Action onReStart;

        /// <summary>
        /// ?????? ???
        /// </summary>
        private Action onDriverPause;
        /// <summary>
        /// ?????? ????
        /// </summary>
        private Action onDriverResume;

        private Timer()
        {
            repeatedCount = 0;
            passedTime = 0;
        }

        ~Timer()
        {
            onStart = null;
            onPause = null;
            onResume = null;
            onCompleted = null;
            onCancel = null;
            onTiming = null;
            onReStart = null;
            onDriverPause = null;
            onDriverResume = null;
        }

        /// <summary>
        /// ??????(????)
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, 0, intervalTime, 0, 0, null, null, completed, null, null, null, null, ignoreTimeScale);
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, float delayTime, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, delayTime, intervalTime, 0, 0, null, null, completed, null, null, null, null, ignoreTimeScale);
        }

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="completed">????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, Action<float> update, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, 0, intervalTime, 0, 0, null, update, completed, null, null, null, null, ignoreTimeScale);
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, float delayTime, Action<float> update, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, delayTime, intervalTime, 0, 0, null, update, completed, null, null, null, null, ignoreTimeScale);
        }

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="start">?????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="pause">?????????</param>
        /// <param name="resume">??????????</param>
        /// <param name="cancel">?????????</param>
        /// <param name="restart">??????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, Action start, Action<float> update, Action completed, Action pause, Action resume, Action cancel, Action restart, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, 0, intervalTime, 0, 0, start, update, completed, pause, resume, cancel, restart, ignoreTimeScale);
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="start">?????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="pause">?????????</param>
        /// <param name="resume">??????????</param>
        /// <param name="cancel">?????????</param>
        /// <param name="restart">??????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, float delayTime, Action start, Action<float> update, Action completed, Action pause, Action resume, Action cancel, Action restart, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, delayTime, intervalTime, 0, 0, start, update, completed, pause, resume, cancel, restart, ignoreTimeScale);
        }

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="repeatCount">?????????? value <0 ????? =0/1 1??</param>
        /// <param name="repeatTime">????????????</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, int repeatCount, float repeatTime, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, 0, intervalTime, repeatCount, repeatTime, null, null, completed, null, null, null, null, ignoreTimeScale);
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="repeatCount">?????????? value <0 ????? =0/1 1??</param>
        /// <param name="repeatTime">????????????</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        public static Timer Start(float duration, float delayTime, int repeatCount, float repeatTime, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, delayTime, intervalTime, repeatCount, repeatTime, null, null, completed, null, null, null, null, ignoreTimeScale);
        }

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="repeatCount">?????????? value <0 ????? =0/1 1??</param>
        /// <param name="repeatTime">????????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, int repeatCount, float repeatTime, Action<float> update, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, 0, intervalTime, repeatCount, repeatTime, null, update, completed, null, null, null, null, ignoreTimeScale);
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="repeatCount">?????????? value <0 ????? =0/1 1??</param>
        /// <param name="repeatTime">????????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        public static Timer Start(float duration, float delayTime, int repeatCount, float repeatTime, Action<float> update, Action completed, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, delayTime, intervalTime, repeatCount, repeatTime, null, update, completed, null, null, null, null, ignoreTimeScale);
        }

        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="repeatCount">?????????? value <0 ????? =0/1 1??</param>
        /// <param name="repeatTime">????????????</param>
        /// <param name="start">?????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="pause">?????????</param>
        /// <param name="resume">??????????</param>
        /// <param name="cancel">?????????</param>
        /// <param name="restart">??????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, int repeatCount, float repeatTime, Action start, Action<float> update, Action completed, Action pause, Action resume, Action cancel, Action restart, float intervalTime = 1, bool ignoreTimeScale = false)
        {
            return Start(duration, 0, intervalTime, repeatCount, repeatTime, start, update, completed, pause, resume, cancel, restart, ignoreTimeScale);
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="duration">??????(/s)</param>
        /// <param name="delayTime">?????????(/s)</param>
        /// <param name="intervalTime">?????(/s)</param>
        /// <param name="repeatCount">?????????? value <0 ????? =0/1 1??</param>
        /// <param name="repeatTime">????????????</param>
        /// <param name="start">?????????</param>
        /// <param name="update">????§Ý??</param>
        /// <param name="completed">????????</param>
        /// <param name="pause">?????????</param>
        /// <param name="resume">??????????</param>
        /// <param name="cancel">?????????</param>
        /// <param name="restart">??????????</param>
        /// <param name="ignoreTimeScale">?????????????</param>
        /// <returns></returns>
        public static Timer Start(float duration, float delayTime, float intervalTime, int repeatCount, float repeatTime, Action start, Action<float> update, Action completed, Action pause, Action resume, Action cancel, Action restart, bool ignoreTimeScale = false)
        {
            Timer newTimer = new Timer
            {
                duration = duration,
                delayTime = delayTime > 0 ? delayTime : 0,
                intervalTime = intervalTime,
                repeatCount = repeatCount,
                repeatTime = repeatTime,
                ignoreTimeScale = ignoreTimeScale,
                onStart = start,
                onTiming = update,
                onCompleted = completed,
                onPause = pause,
                onResume = resume,
                onCancel = cancel,
                onReStart = restart,
            };
            newTimer.currentTimerState = TimerState.Start;
            TimerManager.Instance.CreateTimerDriver(newTimer);
            return newTimer;
        }

        /// <summary>
        /// ???
        /// </summary>
        public void Pause()
        {
            if (currentTimerState != TimerState.Timing && currentTimerState != TimerState.Prepare)
                return;
            currentTimerState = TimerState.Pause;
            if (onDriverPause != null)
                onDriverPause.Invoke();
            if (onPause != null)
                onPause.Invoke();
        }
        /// <summary>
        /// ????
        /// </summary>
        public void Resume()
        {
            if (currentTimerState != TimerState.Pause)
                return;
            if (onDriverResume != null)
                onDriverResume.Invoke();
            if (onResume != null)
                onResume.Invoke();
        }
        /// <summary>
        /// ??????
        /// </summary>
        /// <param name="delay">????????</param>
        public void ReStart(float delay = 0)
        {
            delayTime = delay;
            repeatedCount = 0;
            passedTime = 0;
            if (currentTimerState == TimerState.Stop)
                TimerManager.Instance.CreateTimerDriver(this);
            currentTimerState = TimerState.Start;
            if (onReStart != null)
                onReStart.Invoke();
        }
        /// <summary>
        /// ??????
        /// </summary>
        public void Cancel()
        {
            currentTimerState = TimerState.Stop;
            if (onCancel != null)
                onCancel.Invoke();
        }

        /// <summary>
        /// ??????????
        /// </summary>
        /// <param name="time"></param>
        public void AddDuration(float time)
        {
            duration += time;
        }
        /// <summary>
        /// ???????????
        /// </summary>
        /// <param name="count"></param>
        public void AddRepeatCount(int count)
        {
            repeatCount += count;
        }

        /// <summary>
        /// ??????
        /// </summary>
        internal void OnStart()
        {
            if (onStart != null)
                onStart.Invoke();
        }
        /// <summary>
        /// ????
        /// </summary>
        /// <param name="time">??????</param>
        internal void OnUpdate(float time)
        {
            passedTime = time;
            if (onTiming != null)
                onTiming.Invoke(time);
        }
        /// <summary>
        /// ???????
        /// </summary>
        internal void OnStop()
        {
            repeatedCount++;
            currentTimerState = TimerState.Stop;
            if (onCompleted != null)
                onCompleted.Invoke();
        }

        /// <summary>
        /// ????????
        /// </summary>
        internal void SetTimerState(TimerState state)
        {
            currentTimerState = state;
        }

        /// <summary>
        /// ?????????Action
        /// </summary>
        /// <param name="pause"></param>
        /// <param name="resume"></param>
        internal void InitDriverAction(Action pause, Action resume)
        {
            onDriverPause = pause;
            onDriverResume = resume;
        }

    }
}

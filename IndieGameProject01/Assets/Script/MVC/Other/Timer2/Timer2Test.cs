using UnityEngine;

//[AutoSingleton(true, "timer2CeShi")] 
namespace Script.MVC.Other.Timer2
{
    public class Timer2CeShi : MonoBehaviour
    {

        private Timer timer;
        private Timer timer1;

        private void OnEnable()
        {

            timer = Timer.Start(10, 2, 1, 1, 1, 
                () =>
                {
                    print("��ʼ��ʱ");
                }, 
                (float time) =>
                {
                    print("��ʱ��:" + time);
                }, 
                () =>
                {
                    print("��ʱ����" + timer.repeatedCount);
                }, 
                () =>
                {
                    print("��ͣ��ʱ");
                }, 
                () =>
                {
                    print("������ʱ");
                }, 
                () =>
                {
                    print("ȡ����ʱ");
                }, 
                () => 
                {
                    print("�ؿ�ʼ");
                }, false);

            //timer1 = Timer.Start(30, 2, 1, 1, 0, () =>
            //{
            //    print("2��ʼ��ʱ");
            //}, (float time) =>
            //{
            //    print("2��ʱ��:" + time);
            //}, () =>
            //{
            //    print("2��ʱ����");
            //}, () =>
            //{
            //    print("2��ͣ��ʱ");
            //}, () =>
            //{
            //    print("2������ʱ");
            //}, () =>
            //{
            //    print("2ȡ����ʱ");
            //}, () => {
            //    print("2�ؿ�ʼ");
            //}, false);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("1��ͣ", GUILayout.Width(100), GUILayout.Height(50)))
            {
                timer.Pause();
            }
            if (GUILayout.Button("1����", GUILayout.Width(100), GUILayout.Height(50)))
            {
                timer.Resume();
            }
            if (GUILayout.Button("1ȡ��", GUILayout.Width(100), GUILayout.Height(50)))
            {
                timer.Cancel();
            }
            if (GUILayout.Button("1�ؿ�ʼ", GUILayout.Width(100), GUILayout.Height(50)))
            {
                timer.ReStart(10);
            }
            /*
        if (GUILayout.Button("2��ͣ", GUILayout.Width(100), GUILayout.Height(50)))
        {
            timer1.Pause();
        }
        if (GUILayout.Button("2����", GUILayout.Width(100), GUILayout.Height(50)))
        {
            timer1.Resume();
        }
        if (GUILayout.Button("2ȡ��", GUILayout.Width(100), GUILayout.Height(50)))
        {
            timer1.Cancel();
        }
        if (GUILayout.Button("2�ؿ�ʼ", GUILayout.Width(100), GUILayout.Height(50)))
        {
            timer1.ReStart(10);
        }
        */
        }

    }
}

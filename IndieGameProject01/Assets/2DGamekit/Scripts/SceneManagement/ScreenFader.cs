using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gamekit2D
{
    public class ScreenFader : MonoBehaviour
    {
        public enum FadeType
        {
            Black, //黑色
            Loading, //加载中
            GameOver, //游戏结束
        }
        
        public static ScreenFader Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<ScreenFader> ();

                if (s_Instance != null)
                    return s_Instance;

                Create ();

                return s_Instance;
            }
        }

        public static bool IsFading
        {
            get { return Instance.m_IsFading; }
        }

        protected static ScreenFader s_Instance;

        public static void Create ()
        {
            ScreenFader controllerPrefab = Resources.Load<ScreenFader> ("ScreenFader");
            s_Instance = Instantiate (controllerPrefab);
        }

        //用于管理淡入淡出效果
        public CanvasGroup faderCanvasGroup;
        public CanvasGroup loadingCanvasGroup;
        public CanvasGroup gameOverCanvasGroup;
        public float fadeDuration = 1f;//淡入淡出持续时间

        protected bool m_IsFading;//当前是否正在进行淡入淡出效果
    
        const int k_MaxSortingLayer = 32767;

        void Awake ()
        {
            if (Instance != this)
            {
                Destroy (gameObject);
                return;
            }
        
            DontDestroyOnLoad (gameObject);
        }

        /// <summary>
        /// 控制淡入淡出效果
        /// </summary>
        /// <param name="finalAlpha">衔接时间</param>
        /// <param name="canvasGroup">画布组</param>
        /// <returns></returns>
        protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
        {
            m_IsFading = true;
            canvasGroup.blocksRaycasts = true;
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
            while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                    fadeSpeed * Time.deltaTime);
                yield return null;
            }
            canvasGroup.alpha = finalAlpha;
            m_IsFading = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 设置淡入淡出效果的透明度
        /// </summary>
        /// <param name="alpha">透明度</param>
        public static void SetAlpha (float alpha)
        {
            Instance.faderCanvasGroup.alpha = alpha;
        }
        
        public static IEnumerator FadeSceneIn ()
        {
            CanvasGroup canvasGroup;
            if (Instance.faderCanvasGroup.alpha > 0.1f)
                canvasGroup = Instance.faderCanvasGroup;
            else if (Instance.gameOverCanvasGroup.alpha > 0.1f)
                canvasGroup = Instance.gameOverCanvasGroup;
            else
                canvasGroup = Instance.loadingCanvasGroup;
            
            yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));

            canvasGroup.gameObject.SetActive (false);
        }

        public static IEnumerator FadeSceneOut (FadeType fadeType = FadeType.Black)
        {
            CanvasGroup canvasGroup;
            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.faderCanvasGroup;
                    break;
                case FadeType.GameOver:
                    canvasGroup = Instance.gameOverCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    break;
            }
            
            canvasGroup.gameObject.SetActive (true);
            
            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        }
    }
}
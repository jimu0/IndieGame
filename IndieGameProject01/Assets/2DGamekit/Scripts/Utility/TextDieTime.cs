using System.Collections;
using UnityEngine;

namespace _2DGamekit.Scripts.Utility
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TextDieTime : MonoBehaviour
    {
        protected Coroutine m_DeactivationCoroutine;

        // 不再需要 public BoxCollider2D boxCollider;

        IEnumerator SetGameObjectActive(float delay, bool b)
        {
            yield return new WaitForSeconds(delay);
            GetComponent<BoxCollider2D>().enabled = b; // 直接获取 BoxCollider2D 组件并设置其 enabled 属性
        }

        public void OpenTime(float delay)
        {
            if (m_DeactivationCoroutine != null)
                StopCoroutine(m_DeactivationCoroutine);

            m_DeactivationCoroutine = StartCoroutine(SetGameObjectActive(delay, true));
        }

        public void CloseTime(float delay)
        {
            if (m_DeactivationCoroutine != null)
                StopCoroutine(m_DeactivationCoroutine);

            m_DeactivationCoroutine = StartCoroutine(SetGameObjectActive(delay, false));
        }
        
    }
}

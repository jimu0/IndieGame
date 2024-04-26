using Cinemachine;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Gamekit2D
{
    [RequireComponent(typeof(Collider2D))]
    public class TransitionPoint : MonoBehaviour
    {
        public enum TransitionType
        {
            DifferentZone, //不同区域
            DifferentNonGameplayScene, //不同非游戏场景
            SameScene, //同一场景
        }


        public enum TransitionWhen
        {
            ExternalCall, //外部调用时
            InteractPressed, //交互按下时
            OnTriggerEnter, //触发器进入时
        }

    
        [Tooltip("这是要转换的游戏对象.如，玩家 Player.")]
        public GameObject transitioningGameObject;
        [Tooltip("过渡是否会在这个Scene内、不同的区域或非游戏Scene.")]
        public TransitionType transitionType;
        [SceneName]
        public string newSceneName;
        [Tooltip("Scene中SceneTransitionDestination脚本的标签.")]
        public SceneTransitionDestination.DestinationTag transitionDestinationTag;
        [Tooltip("这个Scene中的Transform是过度中的gameObject将被传送到的位置")]
        public TransitionPoint destinationTransform;
        [Tooltip("什么是触发过度的开始.")]
        public TransitionWhen transitionWhen;
        [Tooltip("当过渡发生时，玩家将失去控制权，但在此时，轴和按钮值是否应重置为默认值.")]
        public bool resetInputValuesOnTransition = true;
        [Tooltip("此过渡是否仅在库存中具有特定项目时才可能?")]
        public bool requiresInventoryCheck;
        [Tooltip("要进行检查的库存.")]
        public InventoryController inventoryController;
        [Tooltip("所需项目.")]
        public InventoryController.InventoryChecker inventoryCheck;
        //过度中的gameObject是否存在
        bool m_TransitioningGameObjectPresent;

        void Start ()
        {
            if (transitionWhen == TransitionWhen.ExternalCall)
                m_TransitioningGameObjectPresent = true;
        }

        void OnTriggerEnter2D (Collider2D other)
        {
            if (other.gameObject == transitioningGameObject)
            {
                m_TransitioningGameObjectPresent = true;

                if (ScreenFader.IsFading || SceneController.Transitioning)
                    return;

                if (transitionWhen == TransitionWhen.OnTriggerEnter)
                    TransitionInternal ();
            }
        }

        void OnTriggerExit2D (Collider2D other)
        {
            if (other.gameObject == transitioningGameObject)
            {
                m_TransitioningGameObjectPresent = false;
            }
        }

        void Update ()
        {
            if (ScreenFader.IsFading || SceneController.Transitioning)
                return;

            if(!m_TransitioningGameObjectPresent)
                return;

            if (transitionWhen == TransitionWhen.InteractPressed)
            {
                if (PlayerInput.Instance.Interact.Down)
                {
                    TransitionInternal ();
                }
            }
        }

        protected void TransitionInternal ()
        {
            if (requiresInventoryCheck)
            {
                if(!inventoryCheck.CheckInventory (inventoryController))
                    return;
            }
        
            if (transitionType == TransitionType.SameScene)
            {
                GameObjectTeleporter.Teleport (transitioningGameObject, destinationTransform.transform);
            }
            else
            {
                SceneController.TransitionToScene (this);
            }
        }

        public void Transition ()
        {
            if(!m_TransitioningGameObjectPresent)
                return;

            if(transitionWhen == TransitionWhen.ExternalCall)
                TransitionInternal ();
        }
    }
}
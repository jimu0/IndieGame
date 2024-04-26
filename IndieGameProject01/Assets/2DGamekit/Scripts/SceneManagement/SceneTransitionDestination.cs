using UnityEngine;
using UnityEngine.Events;

namespace Gamekit2D
{
    public class SceneTransitionDestination : MonoBehaviour
    {
        public enum DestinationTag
        {
            A, B, C, D, E, F, G,
        }


        public DestinationTag destinationTag;    //这与作为目的地的TransitionPoint上选择的标记相匹配。
        [Tooltip("这是通过转换的游戏对象.如 玩家player.")]
        public GameObject transitioningGameObject;
        public UnityEvent OnReachDestination;
    }
}
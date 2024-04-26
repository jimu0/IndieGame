using Script.MVC.Module.Character;
using Script.MVC.Module.Frame.ObjectPool;
using UnityEngine;

namespace Script.MVC.Module.Incubator
{
    public class MonsterIncubator : MonoBehaviour
    {
        private ObjectPool<OpponentUnit> opponentPool;

        void Start()
        {
            opponentPool = new ObjectPool<OpponentUnit>(OpponentOnCreate, OpponentOnGet, OpponentOnRelease, OpponentOnDestroy, true, 10, 40);

        }

        void Update()
        {
        
        }
    
        private OpponentUnit OpponentOnCreate()
        {
            OpponentUnit opponent = opponentPool.Get();
            return opponent;
        }
        private void OpponentOnGet(OpponentUnit opponent)
        {
            opponent.gameObject.SetActive(true);
        }

        private void OpponentOnRelease(OpponentUnit opponent)
        {
            opponent.gameObject.SetActive(false);
        }

        private void OpponentOnDestroy(OpponentUnit opponent)
        {
            opponent.gameObject.SetActive(false);
        }
    }
}

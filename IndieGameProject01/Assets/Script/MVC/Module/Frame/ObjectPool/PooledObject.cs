using System;

namespace Script.MVC.Module.Frame.ObjectPool
{
    public struct PooledObject<T> : IDisposable where T : class
    {
        private readonly T mToReturn;
        private readonly IObjectPool<T> mPool;

        internal PooledObject(T value, IObjectPool<T> pool)
        {
            mToReturn = value;
            mPool = pool;
        }

        void IDisposable.Dispose() => mPool.Release(mToReturn);
    }
}
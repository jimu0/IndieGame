using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.MVC.Module.Frame.ObjectPool
{
  /// <summary>
  ///   <para>基于堆栈的Pool.IObjectPool_1.</para>
  /// </summary>
  public class ObjectPool<T> : IDisposable, IObjectPool<T> where T : class
  {
    internal readonly Stack<T> MStack;
    private readonly Func<T> mCreateFunc;
    private readonly Action<T> mActionOnGet;
    private readonly Action<T> mActionOnRelease;
    private readonly Action<T> mActionOnDestroy;
    private readonly int mMaxSize;
    internal bool MCollectionCheck;

    public int CountAll { get; private set; }

    public int CountActive => CountAll - CountInactive;

    public int CountInactive => MStack.Count;

    public ObjectPool(
      Func<T> createFunc,
      Action<T> actionOnGet = null,
      Action<T> actionOnRelease = null,
      Action<T> actionOnDestroy = null,
      bool collectionCheck = true,
      int defaultCapacity = 10,
      int maxSize = 10000)
    {
      if (maxSize <= 0)
        throw new ArgumentException("最大尺寸必须大于0", nameof (maxSize));
      MStack = new Stack<T>(defaultCapacity);
      mCreateFunc = createFunc ?? throw new ArgumentNullException(nameof (createFunc));
      mMaxSize = maxSize;
      mActionOnGet = actionOnGet;
      mActionOnRelease = actionOnRelease;
      mActionOnDestroy = actionOnDestroy;
      MCollectionCheck = collectionCheck;
    }

    public T Get()
    {
      T obj;
      if (MStack.Count == 0)
      {
        obj = mCreateFunc();
        ++CountAll;
      }
      else
        obj = MStack.Pop();
      Action<T> actionOnGet = mActionOnGet;
      if (actionOnGet != null)
        actionOnGet(obj);
      return obj;
    }

    public PooledObject<T> Get(out T v) => new PooledObject<T>(v = Get(),this);

    public void Release(T element)
    {
      if (MCollectionCheck && MStack.Count > 0 && MStack.Contains(element))
      {
        throw new InvalidOperationException("试图释放一个已经被释放到池中的对象");
      }
        
      Action<T> actionOnRelease = mActionOnRelease;
      if (actionOnRelease != null && element!=null) actionOnRelease(element);
      if (CountInactive < mMaxSize)
      {
        MStack.Push(element);
      }
      else
      {
        Action<T> actionOnDestroy = mActionOnDestroy;
        if (actionOnDestroy != null && element!=null) actionOnDestroy(element);
      }
    }

    public void Clear()
    {
      if (mActionOnDestroy != null)
      {
        foreach (T obj in MStack)
          mActionOnDestroy(obj);
      }
      MStack.Clear();
      CountAll = 0;
    }
    
    public void Dispose() => Clear();
  }
}
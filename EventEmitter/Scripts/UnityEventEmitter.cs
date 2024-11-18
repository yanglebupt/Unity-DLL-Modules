using System;
using UnityEngine;

namespace YLCommon.Unity
{
  using Callback = Action<object>;

  public class UnityEventEmitter<T> : MonoBehaviour
  {
    private readonly EventEmitter<T> bus = new();

    protected virtual void Awake()
    {
      bus.Init();
    }
    protected virtual void Update()
    {
      bus.Tick();
    }
    protected virtual void OnDestroy()
    {
      bus.UnInit();
    }

    public void EmitImmediate(T evt, object payload = null)
    {
      bus.EmitImmediate(evt, payload);
    }
    public void Emit(T evt, object payload = null)
    {
      bus.Emit(evt, payload);
    }

    public void On(T evt, Callback cb)
    {
      bus.On(evt, cb);
    }
    public void Off(T evt)
    {
      bus.Off(evt);
    }
    public void Off(object target)
    {
      bus.Off(target);
    }
  }
}

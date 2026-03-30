using UnityEngine;


using System;
using System.Collections.Generic;
using GGG.Tool.Singleton;

public class GameEventManager : SingletonNonMono<GameEventManager>
{
    private readonly Dictionary<string, Delegate> _eventDic = new Dictionary<string, Delegate>();

    #region AddEvent

    public void AddEvent(string eventName, Action action)
    {
        AddDelegate(eventName, action);
    }

    public void AddEvent<T>(string eventName, Action<T> action)
    {
        AddDelegate(eventName, action);
    }

    public void AddEvent<T1, T2>(string eventName, Action<T1, T2> action)
    {
        AddDelegate(eventName, action);
    }

    public void AddEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
    {
        AddDelegate(eventName, action);
    }

    public void AddEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
    {
        AddDelegate(eventName, action);
    }

    #endregion

    #region RemoveEvent

    public void RemoveEvent(string eventName, Action action)
    {
        RemoveDelegate(eventName, action);
    }

    public void RemoveEvent<T>(string eventName, Action<T> action)
    {
        RemoveDelegate(eventName, action);
    }

    public void RemoveEvent<T1, T2>(string eventName, Action<T1, T2> action)
    {
        RemoveDelegate(eventName, action);
    }

    public void RemoveEvent<T1, T2, T3>(string eventName, Action<T1, T2, T3> action)
    {
        RemoveDelegate(eventName, action);
    }

    public void RemoveEvent<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> action)
    {
        RemoveDelegate(eventName, action);
    }

    #endregion

    #region Call

    public void Call(string eventName)
    {
        if (_eventDic.TryGetValue(eventName, out Delegate del))
        {
            if (del is Action action)
            {
                action.Invoke();
            }
            else
            {
                Debug.LogError($"事件 {eventName} 的参数签名不匹配，期望: Action");
            }
        }
    }

    public void Call<T>(string eventName, T arg)
    {
        if (_eventDic.TryGetValue(eventName, out Delegate del))
        {
            if (del is Action<T> action)
            {
                action.Invoke(arg);
            }
            else
            {
                Debug.LogError($"事件 {eventName} 的参数签名不匹配，期望: Action<{typeof(T).Name}>");
            }
        }
    }

    public void Call<T1, T2>(string eventName, T1 arg1, T2 arg2)
    {
        if (_eventDic.TryGetValue(eventName, out Delegate del))
        {
            if (del is Action<T1, T2> action)
            {
                action.Invoke(arg1, arg2);
            }
            else
            {
                Debug.LogError($"事件 {eventName} 的参数签名不匹配，期望: Action<{typeof(T1).Name}, {typeof(T2).Name}>");
            }
        }
    }

    public void Call<T1, T2, T3>(string eventName, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_eventDic.TryGetValue(eventName, out Delegate del))
        {
            if (del is Action<T1, T2, T3> action)
            {
                action.Invoke(arg1, arg2, arg3);
            }
            else
            {
                Debug.LogError($"事件 {eventName} 的参数签名不匹配，期望: Action<{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}>");
            }
        }
    }

    public void Call<T1, T2, T3, T4>(string eventName, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (_eventDic.TryGetValue(eventName, out Delegate del))
        {
            if (del is Action<T1, T2, T3, T4> action)
            {
                action.Invoke(arg1, arg2, arg3, arg4);
            }
            else
            {
                Debug.LogError($"事件 {eventName} 的参数签名不匹配，期望: Action<{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}>");
            }
        }
    }

    #endregion

    #region Private

    private void AddDelegate(string eventName, Delegate newDelegate)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("事件名不能为空");
            return;
        }

        if (newDelegate == null)
        {
            Debug.LogError($"事件 {eventName} 添加失败，委托为空");
            return;
        }

        if (_eventDic.TryGetValue(eventName, out Delegate oldDelegate))
        {
            if (oldDelegate.GetType() != newDelegate.GetType())
            {
                Debug.LogError($"事件 {eventName} 添加失败，已存在不同参数签名的事件");
                return;
            }

            _eventDic[eventName] = Delegate.Combine(oldDelegate, newDelegate);
        }
        else
        {
            _eventDic.Add(eventName, newDelegate);
        }
    }

    private void RemoveDelegate(string eventName, Delegate removeDelegate)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            Debug.LogError("事件名不能为空");
            return;
        }

        if (removeDelegate == null)
        {
            Debug.LogError($"事件 {eventName} 移除失败，委托为空");
            return;
        }

        if (_eventDic.TryGetValue(eventName, out Delegate oldDelegate))
        {
            if (oldDelegate.GetType() != removeDelegate.GetType())
            {
                Debug.LogError($"事件 {eventName} 移除失败，参数签名不一致");
                return;
            }

            Delegate currentDelegate = Delegate.Remove(oldDelegate, removeDelegate);

            if (currentDelegate == null)
            {
                _eventDic.Remove(eventName);
            }
            else
            {
                _eventDic[eventName] = currentDelegate;
            }
        }
    }

    #endregion

    #region Clear

    public void ClearEvent(string eventName)
    {
        if (_eventDic.ContainsKey(eventName))
        {
            _eventDic.Remove(eventName);
        }
    }

    public void ClearAllEvent()
    {
        _eventDic.Clear();
    }

    #endregion
}

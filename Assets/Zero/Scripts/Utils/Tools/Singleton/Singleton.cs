using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGG.Tool.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        private static object _lock = new object();

        public static T MainInstance
        {
            get
            {
                if (_instance == null) // 检查是否要上锁 有实例直接返回 无需锁
                {
                    lock (_lock) // 加锁
                    {
                        _instance = FindObjectOfType<T>() as T; 
                    
                        if (_instance == null)
                        {
                            GameObject go = new GameObject(typeof(T).Name);
                            _instance = go.AddComponent<T>();
                        }
                    }
                }

                return _instance;
            }
        }
        

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void OnApplicationQuit()//程序退出时，将instance清空
        {
            _instance = null;
        }
    }
    
}
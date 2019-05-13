using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace RDFW
{
    public class RDSingleTon<T> where T:RDSingleTon<T>
    {
        private static T ins;
        protected RDSingleTon(){}
        public static T Ins
        {    
            get
            {
                if(ins==null)
                {
                    ConstructorInfo[] ctorInfos=typeof(T).GetConstructors(BindingFlags.Instance|BindingFlags.NonPublic);
                    //找到无参的构造函数
                    ConstructorInfo ctor=Array.Find(ctorInfos, a =>a.GetParameters().Length == 0);
                    if (ctor == null)
                    {
                       Debug.LogError("SingleTonWrong,please check "+ typeof(T).ToString());
                       return ins;
                    }
                    ins=ctor.Invoke(null) as T;
                }
                Debug.Log(ins.ToString());
                return ins;
            }
        }
    }

    public class RDSingleTon_Mono<T> : MonoBehaviour where T:RDSingleTon_Mono<T>
    {
        private static T ins;
        protected RDSingleTon_Mono() { }
        public static T Ins
        {
            get
            {
                if(ins==null)
                {
                    ins= FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError("there are more than one " + typeof(T).ToString());
                        return ins;
                    }
                    if (ins == null)
                    {
                        GameObject insGameObject = new GameObject(typeof(T).ToString());
                        ins = insGameObject.AddComponent<T>();
                        DontDestroyOnLoad(insGameObject);
                    }  
                }
                return ins;
            }
        }

        private void OnDestroy()
        {
            if (ins != null)
            {
                Destroy(ins.gameObject);
                ins = null;
            }  
        }
    }
}


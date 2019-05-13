using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RDFW;

namespace RDFW
{
    public class RDResManager
    {

        private static Dictionary<string, Object> resDict = new Dictionary<string, Object>();

        public static T LoadWithCache<T>(string path) where T : Object
        {
            if (!resDict.ContainsKey(path))
            {
                T resObject = Resources.Load<T>(path);
                if (resObject == null)
                {
                    Debug.LogErrorFormat("Load Error,please check {0}", path);
                }
                resDict.Add(path, resObject);
            }
            return resDict[path] as T;
        }

    }
}


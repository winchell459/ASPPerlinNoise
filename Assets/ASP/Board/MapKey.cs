using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    [System.Serializable]
    public class MapKey<T> 
    {
        public string widthKey = "width";
        public string heightKey = "height";
        public string pixelKey = "block";
        public int xIndex = 0;
        public int yIndex = 2;
        public int pixelTypeIndex = 3;
        public MapObjectKey<T> dict;
    }

    [System.Serializable]
    public class MapObjectKey<T>
    {
        public MapObject<T>[] mapObjects;
        public T this[string key]
        {
            get => FindObject(key);
        }
        T FindObject(string key)
        {
            T obj = default;
            foreach (MapObject<T> mapObject in mapObjects)
            {
                if (key == mapObject.key) obj = mapObject.obj;
            }
            return obj;
        }
    }

    [System.Serializable]
    public class MapObject<T>
    {
        public string key;
        public T obj;
    }



}


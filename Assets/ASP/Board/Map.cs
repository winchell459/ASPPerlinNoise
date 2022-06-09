using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    public abstract class Map : MonoBehaviour
    {
        protected int width, height;
        //[SerializeField] protected MapKey mapKey;
        public abstract void DisplayMap(Clingo.AnswerSet answerset);
        //public abstract void DisplayMap(Clingo.AnswerSet answerset, MapKey<T> mapKey);
        public abstract void AdjustCamera();
    }

}




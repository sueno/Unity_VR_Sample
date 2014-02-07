using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unity_VR.Global
{

    enum Enemy
    {
        Skeleton1,
    }

    public class EnemyList
    {

        private static IList<GameObject> enemyList = new List<GameObject>();

        static EnemyList()
        {

        }

        public GameObject Get(int index)
        {
            return null;
        }

    }
}
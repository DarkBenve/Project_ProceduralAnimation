using System;
using System.Collections.Generic;
using Script.Extension;
using Script.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Generator
{
    public class SpawnerEnemy : SingletonBehaviour<SpawnerEnemy>
    {
        [SerializeField] private List<Transform> point;
        [SerializeField] private GameObject prefabEnemy;
        [SerializeField] private int nPlayerSpawn;
        

        public void InitSpawnEnemy()
        {
            for (int i = 0; i < nPlayerSpawn; i++)
            {
                Instantiate(prefabEnemy, point[Random.Range(0, point.Count)]);
            }
        }
    }
}
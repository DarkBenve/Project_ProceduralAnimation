using System;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Generator
{
    public class SpawnersPlayer : MonoBehaviour
    {
        [SerializeField] private List<Transform> spawner;
        [SerializeField] private GameObject inGamePlayer;

        private void Awake()
        {
            GameManager.Instance.Started += SpawnPlayer;
        }

        private void SpawnPlayer()
        {
            int randomIndex = Random.Range(0, spawner.Count);
            inGamePlayer.transform.position = spawner[randomIndex].position;
        }
    }
}
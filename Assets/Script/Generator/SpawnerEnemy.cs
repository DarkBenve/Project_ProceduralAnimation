using System.Collections;
using Script.Extension;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script.Generator
{
    public class SpawnerEnemy : SingletonBehaviour<SpawnerEnemy>
    {
        [SerializeField] private GameObject prefabEnemy;
        [SerializeField] private int nPlayerSpawn;
        [SerializeField] private float delaySpawn;
        [SerializeField] private Transform parentNpc;

        private Vector3[] navMeshVertices;
        private int[] navMeshIndices;

        void Start()
        {
            CacheNavMeshData();  // Cache della triangolazione
            StartCoroutine(InitSpawnEnemy());
        }

        private void CacheNavMeshData()
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
            navMeshVertices = navMeshData.vertices;
            navMeshIndices = navMeshData.indices;
        }

        private IEnumerator InitSpawnEnemy()
        {
            for (int i = 0; i < nPlayerSpawn; i++)
            {
                // Spawna un batch di nemici in una volta sola
                SpawnEnemiesBatch(5);  // Ad esempio, spawna 5 nemici per batch
                yield return new WaitForSeconds(delaySpawn);
            }
        }

        private void SpawnEnemiesBatch(int batchSize)
        {
            for (int i = 0; i < batchSize; i++)
            {
                Vector3 spawnPosition = GetRandomNavMeshPoint();
                if (spawnPosition != Vector3.zero)
                {
                    var npc = Instantiate(prefabEnemy, spawnPosition, quaternion.identity);
                    npc.transform.SetParent(parentNpc);
                }
            }
        }

        public Vector3 GetRandomNavMeshPoint()
        {
            // Scegli un triangolo casuale
            int triangleIndex = Random.Range(0, navMeshIndices.Length / 3) * 3;

            // Prendi i vertici del triangolo
            Vector3 vertex1 = navMeshVertices[navMeshIndices[triangleIndex]];
            Vector3 vertex2 = navMeshVertices[navMeshIndices[triangleIndex + 1]];
            Vector3 vertex3 = navMeshVertices[navMeshIndices[triangleIndex + 2]];

            // Genera un punto casuale all'interno del triangolo
            return RandomPointInTriangle(vertex1, vertex2, vertex3);
        }

        private Vector3 RandomPointInTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            float r1 = Random.Range(0f, 1f);
            float r2 = Random.Range(0f, 1f);

            if (r1 + r2 >= 1f)
            {
                r1 = 1f - r1;
                r2 = 1f - r2;
            }

            return v1 + r1 * (v2 - v1) + r2 * (v3 - v1);
        }
    }
}

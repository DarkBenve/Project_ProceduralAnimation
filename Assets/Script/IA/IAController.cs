using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script.IA
{
    public class IAController : MonoBehaviour
    {
        [SerializeField] private Transform point;
        [SerializeField] private float radius = 10.0f;
        
        private NavMeshAgent _navMeshAgent;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            SetRandomDestination();
        }

        private void Update()
        {
            if (_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
            {
                // Controlla se l'agente ha raggiunto la destinazione
                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        // L'agente ha raggiunto la destinazione, imposta una nuova destinazione casuale
                        SetRandomDestination();
                    }
                }
            }
            // else
            // {
            //     Debug.LogWarning("NavMeshAgent non attivo o non posizionato sulla NavMesh.");
            // }
        }

        private void SetRandomDestination()
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                Vector3 finalPosition = hit.position;
                _navMeshAgent.SetDestination(finalPosition);
            }
            else
            {
                // Non è stato trovato un punto valido sulla NavMesh, riprova
                SetRandomDestination();
            }
        }
        
        
    }
}
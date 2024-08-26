using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script.IA
{
    public class IAController : MonoBehaviour
    {
        [SerializeField] private float radius = 7.0f; // Raggio per la destinazione casuale
        [SerializeField] private float checkInterval = 0.5f; // Intervallo di tempo per controllare lo stato del percorso
        [SerializeField] private float movementThreshold = 0.05f; // Soglia minima di movimento per considerare il personaggio bloccato
        [SerializeField] private float stuckTimeout = 1.5f; // Tempo massimo di stallo prima di ricalcolare
        [SerializeField] private float minIdleTime = 0.1f; // Tempo minimo di attesa prima di ripartire
        [SerializeField] private float maxIdleTime = 0.5f; // Tempo massimo di attesa prima di ripartire
        [SerializeField] private float navMeshBorderBuffer = 0.5f; // Distanza minima dal bordo della NavMesh

        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Vector3 _lastPosition;
        private float _stuckTimer;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            SetRandomDestination();
            _lastPosition = transform.position;
            StartCoroutine(CheckMovement());
        }

        private IEnumerator CheckMovement()
        {
            while (true)
            {
                yield return new WaitForSeconds(checkInterval);

                if (_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
                {
                    if (!_navMeshAgent.pathPending)
                    {
                        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                        {
                            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                            {
                                yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));
                                SetRandomDestination();
                            }
                        }
                        else if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid || IsPathTooLong())
                        {
                            SetRandomDestination();
                        }
                        else
                        {
                            var distanceMoved = Vector3.Distance(transform.position, _lastPosition);
                            if (distanceMoved < movementThreshold)
                            {
                                _stuckTimer += checkInterval;
                                if (_stuckTimer >= stuckTimeout)
                                {
                                    SetRandomDestination();
                                }
                            }
                            else
                            {
                                _stuckTimer = 0f;
                            }

                            _lastPosition = transform.position;
                        }
                    }

                    UpdateMovementAnimation();  // Aggiorna l'animazione ad ogni ciclo
                }
            }
        }

        private void UpdateMovementAnimation()
        {
            // Aggiorna l'animazione non appena il personaggio inizia a muoversi
            bool isMoving = _navMeshAgent.velocity.sqrMagnitude > 0.01f; // Soglia ridotta per rilevare il movimento
            _animator.SetBool("IsMoving", isMoving);
        }

        private void SetRandomDestination()
        {
            Vector3 finalPosition = Vector3.zero;
            NavMeshPath path = new NavMeshPath();

            do
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection += transform.position;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
                {
                    finalPosition = hit.position;
                }
                else
                {
                    continue;
                }
            }
            while (IsNearNavMeshEdge(finalPosition) || !IsPathValid(finalPosition, path));

            _navMeshAgent.SetDestination(finalPosition);

            // Aggiorna immediatamente l'animazione al momento di impostare la destinazione
            UpdateMovementAnimation();
        }

        private bool IsNearNavMeshEdge(Vector3 position)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, navMeshBorderBuffer, NavMesh.AllAreas))
            {
                return (hit.position - position).sqrMagnitude > navMeshBorderBuffer * navMeshBorderBuffer;
            }
            return true;
        }

        private bool IsPathValid(Vector3 targetPosition, NavMeshPath path)
        {
            _navMeshAgent.CalculatePath(targetPosition, path);

            // Restituisci true se il percorso è completo e non tagliato
            return path.status == NavMeshPathStatus.PathComplete;
        }

        private bool IsPathTooLong()
        {
            if (_navMeshAgent.path.status == NavMeshPathStatus.PathComplete)
            {
                float pathLength = 0.0f;
                Vector3[] corners = _navMeshAgent.path.corners;

                for (int i = 1; i < corners.Length; i++)
                {
                    pathLength += Vector3.Distance(corners[i - 1], corners[i]);
                }

                return pathLength > (radius * 2);
            }
            return false;
        }
    }
}

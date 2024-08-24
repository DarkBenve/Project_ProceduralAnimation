using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script.IA
{
    public class IAController : MonoBehaviour
    {
        [SerializeField] private float radius = 10.0f; // Raggio per la destinazione casuale
        [SerializeField] private float checkInterval = 1.0f; // Intervallo di tempo per controllare lo stato del percorso
        [SerializeField] private float recalculateDelay = 2.0f; // Ritardo prima di ricalcolare in caso di fallimento
        [SerializeField] private float movementThreshold = 0.1f; // Soglia minima di movimento per considerare il personaggio bloccato
        [SerializeField] private float stuckTimeout = 3.0f; // Tempo massimo di stallo prima di ricalcolare

        private NavMeshAgent _navMeshAgent;
        private Animator _animator; // Variabile per l'Animator
        private float _checkTimer;
        private float _recalculateTimer;
        private Vector3 _lastPosition;
        private float _stuckTimer;

        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>(); // Inizializza l'Animator
            SetRandomDestination();
            _lastPosition = transform.position;
        }

        private void Update()
        {
            // Aggiorna i timer
            _checkTimer += Time.deltaTime;

            if (_checkTimer >= checkInterval)
            {
                _checkTimer = 0f;

                if (_navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
                {
                    // Aggiorna lo stato dell'animazione in base al movimento
                    UpdateMovementAnimation();

                    // Controlla se il personaggio è vicino alla destinazione
                    if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    {
                        if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            SetRandomDestination();
                        }
                    }
                    else if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
                    {
                        // Ricalcola il percorso se è invalido
                        SetRandomDestination();
                    }
                    else
                    {
                        // Controlla se il personaggio è bloccato (non si muove verso la destinazione)
                        if (Vector3.Distance(transform.position, _lastPosition) < movementThreshold)
                        {
                            _stuckTimer += checkInterval;
                            if (_stuckTimer >= stuckTimeout)
                            {
                                // Ricalcola il percorso se il personaggio è bloccato
                                SetRandomDestination();
                                _stuckTimer = 0f;
                            }
                        }
                        else
                        {
                            // Se il personaggio si sta muovendo, resetta il timer
                            _stuckTimer = 0f;
                        }

                        // Aggiorna la posizione per il prossimo controllo
                        _lastPosition = transform.position;
                    }
                }
                else
                {
                    // Incrementa il timer di ricalcolo quando il personaggio non è su NavMesh o NavMeshAgent è disabilitato
                    _recalculateTimer += checkInterval;
                    if (_recalculateTimer >= recalculateDelay)
                    {
                        SetRandomDestination();
                        _recalculateTimer = 0f;
                    }
                }
            }
        }

        private void UpdateMovementAnimation()
        {
            // Controlla la velocità dell'agente e aggiorna l'animatore
            bool isMoving = _navMeshAgent.velocity.sqrMagnitude > 0.1f;
            _animator.SetBool("IsMoving", isMoving);
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
                // Non è stato trovato un punto valido sulla NavMesh, riprova dopo un breve intervallo
                Invoke(nameof(SetRandomDestination), recalculateDelay);
            }
        }
    }
}

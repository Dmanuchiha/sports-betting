using FPSPrototype.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace FPSPrototype.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private float detectionRange = 22f;
        [SerializeField] private float attackRange = 16f;
        [SerializeField] private float fireRate = 2f;
        [SerializeField] private float bulletDamage = 12f;
        [SerializeField] private LayerMask lineOfSightMask = ~0;

        private NavMeshAgent agent;
        private Transform player;
        private HealthShieldSystem playerHealth;
        private int patrolIndex;
        private float nextFire;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                playerHealth = playerObject.GetComponent<HealthShieldSystem>();
            }

            SetNextPatrolPoint();
        }

        private void Update()
        {
            if (player == null)
            {
                Patrol();
                return;
            }

            float distance = Vector3.Distance(transform.position, player.position);
            bool seesPlayer = HasLineOfSight();

            if (distance <= detectionRange && seesPlayer)
            {
                agent.SetDestination(player.position);

                if (distance <= attackRange)
                {
                    transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                    TryShootPlayer();
                }
            }
            else
            {
                Patrol();
            }
        }

        private void Patrol()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                return;
            }

            if (!agent.pathPending && agent.remainingDistance < 0.6f)
            {
                SetNextPatrolPoint();
            }
        }

        private void SetNextPatrolPoint()
        {
            if (patrolPoints == null || patrolPoints.Length == 0)
            {
                return;
            }

            agent.SetDestination(patrolPoints[patrolIndex].position);
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        private bool HasLineOfSight()
        {
            Vector3 origin = transform.position + Vector3.up * 1.4f;
            Vector3 toPlayer = (player.position + Vector3.up) - origin;
            if (Physics.Raycast(origin, toPlayer.normalized, out RaycastHit hit, detectionRange, lineOfSightMask, QueryTriggerInteraction.Ignore))
            {
                return hit.transform.CompareTag("Player");
            }

            return false;
        }

        private void TryShootPlayer()
        {
            if (Time.time < nextFire || playerHealth == null)
            {
                return;
            }

            nextFire = Time.time + 1f / fireRate;
            playerHealth.ApplyDamage(bulletDamage);
        }
    }
}

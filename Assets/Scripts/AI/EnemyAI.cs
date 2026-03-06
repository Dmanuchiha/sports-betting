using FPSPrototype.Combat;
using FPSPrototype.Core;
using UnityEngine;
using UnityEngine.AI;

namespace FPSPrototype.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HealthShieldSystem))]
    public class EnemyAI : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Transform eyes;
        [SerializeField] private float detectionRange = 28f;
        [SerializeField] private float fieldOfView = 90f;
        [SerializeField] private float attackRange = 20f;
        [SerializeField] private float fireRate = 3f;
        [SerializeField] private float weaponDamage = 8f;
        [SerializeField] private LayerMask visibilityMask;

        private NavMeshAgent agent;
        private HealthShieldSystem health;
        private Transform player;
        private int patrolIndex;
        private float fireTimer;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<HealthShieldSystem>();
            health.OnDied += () => Destroy(gameObject, 2f);
        }

        private void Start()
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }

            MoveToNextPatrol();
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector3.Distance(transform.position, player.position);
            if (CanSeePlayer(distance))
            {
                agent.SetDestination(player.position);
                if (distance <= attackRange)
                {
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
            if (patrolPoints.Length == 0) return;
            if (!agent.pathPending && agent.remainingDistance <= 0.5f)
            {
                MoveToNextPatrol();
            }
        }

        private void MoveToNextPatrol()
        {
            if (patrolPoints.Length == 0) return;
            agent.SetDestination(patrolPoints[patrolIndex].position);
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        private bool CanSeePlayer(float distance)
        {
            if (distance > detectionRange) return false;

            Vector3 toPlayer = (player.position - eyes.position).normalized;
            float angle = Vector3.Angle(eyes.forward, toPlayer);
            if (angle > fieldOfView * 0.5f) return false;

            if (Physics.Raycast(eyes.position, toPlayer, out RaycastHit hit, detectionRange, visibilityMask))
            {
                return hit.transform == player;
            }

            return false;
        }

        private void TryShootPlayer()
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer > 0f) return;

            fireTimer = 1f / fireRate;
            if (player.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(new DamageInfo(weaponDamage, player.position, -player.forward, gameObject));
            }
        }

        public void TakeDamage(DamageInfo info)
        {
            health.TakeDamage(info);
        }
    }
}

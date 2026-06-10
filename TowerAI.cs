using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public enum TowerState {Patrol, Attack, Die}
    public TowerState currentState = TowerState.Patrol;
    [Header("Patrol Settings")]

    public Transform turret;
    public float rotationSpeed = 30f;
    public float maxRotationAngle = 90f;
    public float detectionRange = 10f;
    Transform target;

    [Header("Attack Settings")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 2f;
    float fireCooldown = 0f;
    [Header("Die Settings")]
    public int health = 100;
    public GameObject destroyPrefab;
    bool isTowerDead = false;
    [Header("General Settings")]
    public GameObject buildEffectPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (buildEffectPrefab)
        {
            Instantiate(buildEffectPrefab, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TowerState.Patrol:
                Patrol();
                break;
            case TowerState.Attack:
                Attack();
                break;
            case TowerState.Die:
                Die();
                break;
        }
    }
        
        void Patrol()
        {
            Debug.Log("Patrolling...");
            //turret.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            float angle = Mathf.PingPong(Time.time * rotationSpeed, maxRotationAngle * 2) - maxRotationAngle;
            turret.localRotation = Quaternion.Euler(0, angle, 0);

            LookForEnemies();
        }

        void LookForEnemies()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
            Transform closestEnemy = null;
            float shortestDistance = Mathf.Infinity;

            foreach(Collider collider in colliders)
            {
                if(collider.CompareTag("Enemy"))
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);

                    if(distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        closestEnemy = collider.transform;
                    }
                }
                if (closestEnemy)
                {
                    currentState = TowerState.Attack;
                    target = closestEnemy;
                    Debug.Log("Enemy Detected: " + target.name);
                }
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }

        void Attack()
        {
            Debug.Log("Attacking...");

            if (target == null || Vector3.Distance(transform.position, target.position) > detectionRange)
            {
                target = null;
                currentState = TowerState.Patrol;
                return;
            }
            //face the enemy
            //turret.LookAt(target);
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            //cooldown, can we shoot?

            if(fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
                fireCooldown -= Time.deltaTime;
        }

        void Shoot()
        {
            var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
            if (bulletBehavior != null)
            {
                bulletBehavior.SetTarget(target);
            }
        }


        void Die()
        {
            if(destroyPrefab)
            {
                if(isTowerDead)
                return;
                Instantiate(destroyPrefab, transform.position, transform.rotation);
            }
            Destroy(gameObject, 1);
            isTowerDead = true;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                currentState = TowerState.Die;
                Instantiate(destroyPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }


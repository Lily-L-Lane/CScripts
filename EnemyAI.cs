using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    public enum EnemyState {Navigate, Attack, Die};

    [Header("General Settings")]
    public Transform targetBase;
    public EnemyState currentState = EnemyState.Navigate;
    public int baseDamageValue = 10;
    public int reward = 1;

    [Header("Navigate Settings")]
    public Transform turret;
    public float roatationSpeed = 30f;
    public float maxRotationAngle = 90f;
    public float detectionRange = 10f;

    [Header("Attack Settings")]
    public bool canAttack = true;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2;

    [Header("Die Settings")]
    public int health = 100;
    public GameObject destroyPrefab;
    public Slider healthSlider;
    bool isEnemyDead;
    float fireCooldown = 0;
    Transform attackTarget;
    Quaternion initialTurretRotation;
    int maxHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!targetBase)
        {
            targetBase = GameObject.FindGameObjectWithTag("Target").transform;

            if(!targetBase)
            {
                Debug.log("No target base for enemies.");
                return;
            }
        }
        agent.SetDestination(targetBase.position);

        if(turret)
        {
            initialTurretRotation = turret.localRotation;
        }
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Navigate:
                Navigate();
                break;
            case EnemyState.Attack:
                if(canAttack)
                    Attack();
                else
                    currentState = EnemyState.Navigate;
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }
    void Navigate()
    {
        //agent.SetDestination(targetBase.position);
        if(canAttack)
            FindNearestTower();
        
        if(turret)
        {
            turret.localRotation = Quaternion.Slerp(turret.localRotation, initialTurretRotation, Time.deltaTime * roatationSpeed);
        }
    }

    void Attack()
    {
        if(attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > detectionRange)
        {
            attackTarget = null;
            currentState = EnemyState.Navigate;
            return;
        }

        //attack
        Vector3 direction = attackTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * roatationSpeed);

        //cooldown, can we shoot?

        if(fireCooldown <= 0)
        {
            if(HasLineOfSight(attackTarget))
            Shoot();
            fireCooldown = 1f / fireRate;
        }
        else
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if(!canAttack)
            return;
        var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        BulletBehavior bulletScript = bullet.GetComponent<BulletBehavior>();

        if(bulletBehavior)
        {
            var targetTowerTurret = attackTarget.tranform.Find("Turret").transform;
            if(targetTowerTurret)
                bulletBehavior.SetTarget(targetTowerTarget);
            else
                bulletBehavior.SetTarget(attackTarget);
        }
    }
    
    void Die()
    {
        if(isEnemyDead)
            return;
        Debug.Log("Enemy Died!");
        agent.isStopped = true;
        isEnemyDead = true;

        if(destroyPrefab)
        {
            Instantiate(destroyPrefab, transform.position, transform.rotation);
        }
        MoneyManager.Instance.GetMoney(reward);
        Destroy(gameObject, 1);
    }

    void FindNearestTower()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        float closestDistance = Mathf.Infinity;
        Transform closestTower = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTower = collider.transform;
                }
            }
        }

        if (closestTower)
        {
            attackTarget = closestTower;
            Debug.Log("Tower Detected: " + closestTower.name);
            currentState = EnemyState.Attack;
            return;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthSlider)
        {
            healthSlider.value = health;
        }
        if (health <= 0)
        {
            currentState = EnemyState.Die;
            health = 0;
        }
    }

    public int GetEnemyDamageValue()
    {
        return baseDamageValue;
    }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag("Bullet"))
            {
            BulletBehavior bulletBehavior = collision.gameObject.GetComponent<BulletBehavior>();
            if (bulletBehavior)
            {
                int bulletDamage = bulletBehavior.damage;
                TakeDamage(bulletDamage); 
                Debug.Log("Enemy took damage: " + bulletDamage);
            } 
            else
            {
                Debug.LogWarning("Bullet does not have a BulletBehavior script attached.");
            }
            }
        }

        bool HasLineOfSight(Transform target)
        {   RaycastHit hit;
        Vector3 direction = (target.position - firePoint.position).normalized;
        if(Physics.Raycast(firePoint.position, direction, out hit, detectionRange))
        {
            if (hit.collder.CompareTag("Tower"))
            {
                Debug.Log("Tower is in sight:" + hit.collider.name);
                return true;
            }
        }
        return false;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
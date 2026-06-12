using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseBehavior : MonoBehavior
{
    public int health = 100;
    public Slider HealthSlider;
    public ParticleSystem baseAttackVfx;
    int maxHealth;
    void Start()
    {
        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
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
            Debug.log("Game Over");
            health = 0;
            GameLost();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI)
            {
                int baseDamageValue = enemyAI.GetEnemyDamageValue();
                TakeDamage(baseDamageValue); 
                if(baseAttackVfx)
                baseAttackVfx.Play();
                Debug.Log("Base took damage: " + baseDamageValue);
            } 
            Destory(other.gameObject);
        }
    }
    void GameLost()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
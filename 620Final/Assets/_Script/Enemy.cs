using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum DamageTypes
{
    ghost,
    wall
}*/

public interface IDamageable
{
    public void TakeDamage(float damage);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    //public DamageTypes damageType ; 
    [SerializeField] protected float _life = 10;
    //static float _allLife = 10;//change life for all enemies (can be used for achievement of how many enemies have hit)
    public virtual void TakeDamage(float damage)
    {
        _life = Mathf.Max(_life - damage, 0);
    }
}

/*public class Ghost: Enemy
{
    private void Start()
    {
        _life = 100f;
        damageType = DamageTypes.ghost; 
    }
}

public class Player: MonoBehaviour
{
    Enemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null && enemy.damageType == DamageTypes.ghost)
        {
            enemy.TakeDamage(10f);
        }
    }
}*/

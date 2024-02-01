using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] int Health = 2;
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] GameObject noodle;

    private Rigidbody rb;
    public int currentHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = Health;
    }

    public void TakeHit()
    {
        currentHealth--;
        anim.SetTrigger("Hit");
        Debug.Log("Hit");

        if (currentHealth <= 0)
            Die();
    }    

    private void Die()
    {
        Instantiate(noodle, transform.position, Quaternion.identity);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

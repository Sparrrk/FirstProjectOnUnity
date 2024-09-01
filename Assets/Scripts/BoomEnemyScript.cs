using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemyScript : EnemyScript
{
    [SerializeField] float damage = 30f;
    [SerializeField] float timeBtwAttacks = 10f;
    [SerializeField] float ExplosionRadius = 8f;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject boomEffect;
    [SerializeField] AudioClip boomSound;
    

    private float timer = 9f;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (player != null)
        {
            base.Update();
            if (CanAttackCheck())
            {
                timer += Time.deltaTime;
                if (timer > timeBtwAttacks)
                {
                    Boom();
                    timer = 0f;
                }
            }
        }
    }

    private void Boom()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, ExplosionRadius, mask);
        
        foreach(Collider2D collider in colliders)
        {
            collider?.GetComponent<PlayerScript>()?.TakeDamage(damage);
        }

        TakeDamage(damage * 10);
        SoundManager.instance.PlaySound(boomSound);
        Instantiate(boomEffect, transform.position, Quaternion.identity);
    }



}

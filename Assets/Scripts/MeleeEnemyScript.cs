using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MeleeEnemyScript : EnemyScript
{
    [SerializeField] float damage = 10f;
    [SerializeField] float timeBtwAttacks = 2f;
    [SerializeField] float attackSpeed = 6f;
    [SerializeField] AudioClip attackSound;

    private float timer = 1.5f;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (player != null)
        {
            base.Update();
            timer += Time.deltaTime;
            if (CanAttackCheck())
            {
                if (timer > timeBtwAttacks)
                {
                    timer = 0f;

                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        SoundManager.instance.PlaySound(attackSound);

        Vector2 initialPos = transform.position;
        Vector2 target = player.transform.position;

        player.TakeDamage(damage);

        float percent = 0f;

        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            
            transform.position = Vector2.Lerp(initialPos, target, interpolation);

            yield return null;
        }
    }
}

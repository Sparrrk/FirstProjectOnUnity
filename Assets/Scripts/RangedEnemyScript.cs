using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyScript : EnemyScript
{
    [SerializeField] float timeBtwAttacks = 2f;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;
    [SerializeField] AudioClip shootSound;

    private float timer = 1.0f;

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
            timer += Time.deltaTime;
            if (CanAttackCheck())
            {
                if (timer > timeBtwAttacks)
                {
                    timer = 0f;

                    Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        SoundManager.instance.PlaySound(shootSound);

        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shootPos.transform.rotation = rotation;

        Instantiate(bullet, shootPos.transform.position, shootPos.transform.rotation) ;
    }
}

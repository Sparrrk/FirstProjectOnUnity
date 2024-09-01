using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    
    [SerializeField] float health = 30f;
    [SerializeField] float speed = 10f;
    [SerializeField] float stopDistance = 10f;
    [SerializeField] GameObject hitEffect;
    [SerializeField] ParticleSystem footSteps;
    [SerializeField] int minMoneyAmount;
    [SerializeField] int maxMoneyAmount;
    [SerializeField] AudioClip takeDamageSound, deathSound; 

    protected PlayerScript player;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private bool canAttack = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        player = PlayerScript.instance;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SortingEnemyScript.instance.AddEnemy(spriteRenderer);
    }

    private void OnDestroy()
    {
        SortingEnemyScript.instance.RemoveEnemy(spriteRenderer);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (player != null)
            TurnLeftRight(player.transform.position);
    }

    protected void FixedUpdate()
    {
        if (player != null)
            Move();
    }

    private void Move() 
    {
        if (isDead) return;
        if (player != null)
        {
            var emission = footSteps.emission;
            if (Vector2.Distance(player.transform.position, transform.position) > stopDistance)
            {
                animator.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position + AddSomeDispersion(), speed * Time.deltaTime);
                canAttack = false;
                emission.rateOverTime = 10;
            }
            else
            {
                animator.SetBool("Run", false);
                canAttack = true;
                emission.rateOverTime = 0;
            }
        }
    }

    private Vector3 AddSomeDispersion()
    {
        float additionalX = Random.Range(-stopDistance + 0.1f, stopDistance - 0.1f);
        float additionalY = Random.Range(-stopDistance + 0.1f, stopDistance - 0.1f);

        Vector3 addition = new Vector3(additionalX, additionalY);
        return addition;
    }

    private void TurnLeftRight(Vector2 playerPos)
    {
        if (isDead) return;
        if (playerPos.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        SoundManager.instance.PlaySound(takeDamageSound);
        health -= damage ;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            AddMoney(Random.Range(minMoneyAmount, maxMoneyAmount));
            SoundManager.instance.PlaySound(deathSound);
            VampiricBuff();
            isDead = true;
            animator.SetTrigger("Death");
        }
    }

    private void VampiricBuff()
    {
        if (PlayerPrefs.GetInt("Button3") == 1 && player.GetHealth() < 100) 
        {
            player.AddHealth(1);
            player.HealthBarUpdate();
        }
    }

    public IEnumerator EnemyFade()
    {
        while (spriteRenderer.color.a > 0)
        {
            float visibility = spriteRenderer.color.a;
            spriteRenderer.color = new Color(255f, 255f, 255f, visibility - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    protected void AddMoney(int amount)
    {
        player.coinAmount += amount;
    }

    protected bool CanAttackCheck()
    {
        return canAttack;
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] int speed = 1;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float timeBetweenShoots = 0.25f;
    [SerializeField] GameObject shootPos;
    [SerializeField] float health = 100f;
    [SerializeField] SpriteRenderer muzzleFlashSpr;
    [SerializeField] Sprite muzzleFlashSprite;
    [SerializeField] Slider healthBarSlider;
    [SerializeField] ParticleSystem footSteps;
    [SerializeField] GameObject deathPanel;

    [SerializeField] AudioClip takeDamageSound, shootSound, deathSound;
    [SerializeField] AudioClip[] footstepsSound;
    private AudioSource audioSource;

     
    public static PlayerScript instance;
    public int coinAmount = 0;
    public bool canBeDamaged = true;
    private float TimeCounter = 2;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 velocity;
    public Vector2 VectorSpeed;
    private float maxHealth = 100f;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = SoundManager.instance.audioSource;
    }


    // Update is called once per frame
    void Update()
    {
        TimeCounter += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && TimeCounter >= timeBetweenShoots)
        {
            Shoot();
            TimeCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        if (instance != null)
            Move();
    }

    private void Move()
    {
        VectorSpeed = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") );
        var emission = footSteps.emission;
        if (VectorSpeed !=  Vector2.zero)
        {
            animator.SetBool("Walk", true);
            emission.rateOverTime = 10;

            if (!audioSource.isPlaying)
            {
                audioSource.clip = footstepsSound[Random.Range(0, footstepsSound.Length)];
                audioSource.Play();
            }
        }
        else
        {
            animator.SetBool("Walk", false);
            emission.rateOverTime = 0;
        }

        PlayerTurn(VectorSpeed.x);

        velocity = VectorSpeed.normalized * speed;

        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    private void PlayerTurn(float x)
    {
        if (x == 1)
        {
            spriteRenderer.flipX = false;
        }
        else if (x == -1)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Shoot()
    {
        SoundManager.instance.PlaySound(shootSound);
        Instantiate(bullet, shootPos.transform.position, shootPos.transform.rotation);
        if (PlayerPrefs.GetInt("Button1") == 1)
        {
            Instantiate(bullet, shootPos.transform.position, shootPos.transform.rotation * Quaternion.Euler(0, 0, 15));
            Instantiate(bullet, shootPos.transform.position, shootPos.transform.rotation * Quaternion.Euler(0, 0, -15));
        }

        StartCoroutine(MuzzleFlashFunc());
    }

    IEnumerator MuzzleFlashFunc()
    {
        muzzleFlashSpr.enabled = true;
        muzzleFlashSpr.sprite = muzzleFlashSprite;

        yield return new WaitForSeconds(0.1f);

        muzzleFlashSpr.enabled = false;

    }


    public void TakeDamage(float damage)
    {
        if (!canBeDamaged) return;

        health -= damage;

        SoundManager.instance.PlaySound(takeDamageSound);

        HealthBarUpdate();

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        CameraScript.instance.GetComponent<CameraScript>().CameraShake();

        if (health <= 0)
        {
            SoundManager.instance.PlaySound(deathSound);
            deathPanel.SetActive(true);
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void AddHealth(float add)
    {
        health += add;
    }

    public void IncreaseAtkSpeed()
    {
        timeBetweenShoots *= 0.6f;
    }

    public void HealthBarUpdate()
    {
        healthBarSlider.value = (float)health / maxHealth;
    }
}

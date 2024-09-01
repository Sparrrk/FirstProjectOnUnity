using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    [SerializeField] int speed = 15;
    [SerializeField] float deathTime = 4f;
    [SerializeField] float damage = 10f;

    private float currentLifeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        currentLifeTime += Time.deltaTime;
        if (currentLifeTime >= deathTime)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDashScript : MonoBehaviour
{
    [SerializeField] float timeBtwDash = 5f;
    [SerializeField] float dashForce = 100f;
    [SerializeField] float dashDuration = 1.5f;
    [SerializeField] Slider dashSlider;
    [SerializeField] AudioClip dashSound;

    private PlayerScript player;
    private float dashTimer;
    private bool isDashing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerScript.instance;
        dashTimer = timeBtwDash;
    }

    // Update is called once per frame
    void Update()
    {
        dashTimer += Time.deltaTime;
        
        dashSlider.value = dashTimer / timeBtwDash;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer >= timeBtwDash)
        {
            dashTimer = 0f;
            ActivateDash();
        }
    }

    private void ActivateDash()
    {
        SoundManager.instance.PlaySound(dashSound);
        isDashing = true;
        player.canBeDamaged = false;

        Invoke(nameof(UnActivateDash), dashDuration);
    }

    private void UnActivateDash()
    {
        player.canBeDamaged = true;
        isDashing = false;
    }

    private void Dash()
    {
        player.GetComponent<Rigidbody2D>().AddForce(player.VectorSpeed * Time.deltaTime * dashForce * 100);
    }

    private void FixedUpdate()
    {
        if (isDashing) { Dash(); }
    }
}

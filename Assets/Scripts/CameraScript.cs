using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] float maxX, maxY, minX, minY;
    [SerializeField] Transform target;
    [SerializeField] float cameraSpeed;

    public static CameraScript instance;

    private Animator animator;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!target) return;

        transform.position = Vector3.Lerp(transform.position, 
                                          new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minY, maxY), -10), 
                                          cameraSpeed * Time.deltaTime);
            
    }


    public void CameraShake()
    {
        animator.Play("CameraShakeAnimation");
    }

}

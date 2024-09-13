using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float minSpeed = -5f; 
    public float maxSpeed = -15f; 
    public float lateralSpeed = 2f; 
    private float speed;
    private Rigidbody rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = Random.Range(minSpeed, maxSpeed);
        float randomLateralMovement = Random.Range(-lateralSpeed, lateralSpeed);

        rb.velocity = transform.forward * speed + transform.right * randomLateralMovement;
    }
}


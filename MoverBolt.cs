using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBolt : MonoBehaviour
{
    public float minSpeedBolt = -5f;
    public float maxSpeedBolt = -15f;
    public float lateralSpeedBolt = 2f;  // Will be adjusted based on fireRate
    private float speed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = Random.Range(minSpeedBolt, maxSpeedBolt);

        float randomLateralMovement = Random.Range(-lateralSpeedBolt, lateralSpeedBolt);

        rb.velocity = transform.forward * speed + transform.right * randomLateralMovement;
    }
}

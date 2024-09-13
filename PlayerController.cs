using System.Collections;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed;
    public float tilt;
    public Boundary boundary;

    [Header("Bolt Settings")]
    public GameObject Shot;
    public Transform ShotSpawn;
    public float fireRate;

    private float nextFire;

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public float missileFireRate;

    public float nextMissileFire;
    public GameObject currentTarget;  // Ensure this is the target instance

    void Update()
    {
      
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bolt = Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
            MoverBolt boltMover = bolt.GetComponent<MoverBolt>();

            if (boltMover != null)
            {
                
                float spreadFactor = Mathf.Clamp(fireRate * 2f, 1f, 10f); 
                boltMover.lateralSpeedBolt *= spreadFactor;
            }
        }

        // Fire missile
        if (Input.GetButton("Fire2") && Time.time > nextMissileFire)
        {
            nextMissileFire = Time.time + missileFireRate;
            LaunchMissile();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    private void LaunchMissile()
    {
        if (currentTarget == null)
        {
            Debug.LogWarning("No target for missile!");
            return;
        }

        // Instantiate the missile and assign the target
        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        Missile missileScript = missile.GetComponent<Missile>();

        if (missileScript != null)
        {
            missileScript._target = currentTarget;  // Assign the asteroid instance as the missile target
        }
    }

    // This method should be called by the GameController when a new asteroid spawns.
    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }
}

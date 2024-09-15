using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{

    public GameObject explosion;
    public GameObject Playerexplosion;


   void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary")
        {
            return;
        }

        if (other.tag == "ForceField")
        {
                return;
        }

        Instantiate(explosion, other.transform.position, transform.rotation);
            if (other.tag == "Player")
        {
            Instantiate(Playerexplosion, other.transform.position, other.transform.rotation);
        }
                
        
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}

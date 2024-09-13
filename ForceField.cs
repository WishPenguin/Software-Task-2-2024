using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    [Header("Force Field Settings")]
    public float startWait1 = 2f;

    private void Start()
    {
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Shield"))
        {
            gameObject.SetActive(false);
            StartCoroutine(ForceFieldWait());
        }
    }

    private IEnumerator ForceFieldWait()
    {
        yield return new WaitForSeconds(startWait1);
        gameObject.SetActive(true);
    }
}

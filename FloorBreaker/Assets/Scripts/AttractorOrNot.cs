using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorOrNot : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject player, voidSucker;
    const float G = 667.4f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
        player = GameObject.FindGameObjectWithTag("Player");
        voidSucker.SetActive(true);
    }

    private void FixedUpdate()
    {
        Attract();
    }

    void Attract() // constant force
    {
        Rigidbody rbToAttract = player.GetComponent<Rigidbody>(); 

        Vector3 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;

        if (distance > 7) return; // hopefully this makes it not go wild when captured by a voidsucker
        if (distance < 0.7f) gameObject.SetActive(false);

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}

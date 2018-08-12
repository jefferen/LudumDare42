using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarCapture : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pillar"))
        {
            collision.gameObject.GetComponent<PillarFunctionality>().CustumOnDisable();
        }
    }
}

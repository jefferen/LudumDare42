using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
    public HexagonLvlBuilder gameManager;

    private void OnCollisionEnter(Collision collision) // remember to change player name
    {
        if (collision.gameObject.CompareTag("Player")) gameManager.PressStart();
    }
}

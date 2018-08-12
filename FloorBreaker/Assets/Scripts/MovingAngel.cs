using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAngel : MonoBehaviour
{
    public PlayerController player; // why not just gameobject! What are you doing?
    float leTime;
    bool tellTheTime;
    public Vector3 startPos;
    public GameObject halo, becomeGodLike;

    public void TimerReset()
    {
        leTime = 0;
        transform.position = startPos;
        tellTheTime = true;
    }
	
	void LateUpdate ()
    {
        if (!tellTheTime) return;

        leTime += Time.deltaTime;

        if (leTime > 180)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.4f);
        }
        else
        {
            Debug.Log(leTime);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.015f);
        } 
    }

    private void OnCollisionEnter(Collision collision) // ascend
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            halo.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
            halo.transform.parent = player.transform;
            becomeGodLike.SetActive(true);
            TimerReset();
        }
    }
}

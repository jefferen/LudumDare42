using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraSensitivity = 0.25f;
    Vector3 offset;
    public float distance = 5, offsetY;

    void Start ()
    {
        transform.position = (transform.position - player.transform.position).normalized * distance + player.transform.position;
        offset = transform.position - player.transform.position;
    }
	
	void LateUpdate ()
    {
        Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("HoriCam") * cameraSensitivity, Vector3.up);
        offset = turnAngle * offset; // not to clean of a method but can't come up with another solution atm

        Vector3 pos = player.transform.position + offset;
        transform.position = Vector3.Slerp(transform.position, pos, cameraSensitivity);
        transform.position = new Vector3(transform.position.x, player.transform.position.y + offsetY, transform.position.z);

        transform.LookAt(player.transform.position);
    }
}

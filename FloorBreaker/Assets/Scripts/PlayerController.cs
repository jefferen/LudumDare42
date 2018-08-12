using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject stabaliser;
    float h, v;
    public float moveSpeed, jumpForce;
    Rigidbody rb;
    bool jump, onGround;
    public float startG, endG;
    public AudioSource jumpSource, rollSource;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        ResetPlayerPos();
	}

    public void ResetPlayerPos()
    {
        transform.position = new Vector3(0, 3, 0);
    }
	
	void Update ()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        jump = Input.GetButtonDown("Jump");

        if (jump && onGround)
        {
            jumpSource.Play();
            rb.AddForce(Vector3.up * jumpForce);
            onGround = false;
        }

        if (rb.velocity.y < 0.07f) Physics.gravity = new Vector3(0, -endG, 0);
        else Physics.gravity = new Vector3(0, -startG, 0);
	}

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0,rb.velocity.y,0);
        Vector3 stab = stabaliser.transform.forward;

        stab.y = 0;

        rb.AddForce(stab * v * moveSpeed);
        rb.AddForce(stabaliser.transform.right * h * moveSpeed);

        if (h != 0 | v != 0 && !rollSource.isPlaying) rollSource.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.CompareTag("Ground")) onGround = true;
        onGround = true; // danm not enough time to make better jump
    }

    private void OnCollisionStay(Collision collision)
    {
        if (onGround) return;
        onGround = true;
    }
}

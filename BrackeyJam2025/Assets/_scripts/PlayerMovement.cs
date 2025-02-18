using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float jumpForce = 100;
    private Rigidbody2D RB;


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        //Player 'A', 'D', right arrow, left arrow, and remote controller stick movement
        float x = Input.GetAxis("Horizontal");
        RB.velocity = new Vector2(x*speed, RB.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            RB.AddForce(new Vector2(0,jumpForce));
            print("jump");
        }
    }


}

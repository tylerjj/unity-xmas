using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 2021 Update:
//  Author: Tyler Johnston
//  Note On Formatting: 
//        Not sure why LineSeperator characters are showing up.
//        This file's indentation and newlines were saved in an
//        unfamiliar format. Possibly from scripting in an IDE.
//        My attempts to rectify this might result in multiple
//        newline characters. 
public class PlayerManager : MonoBehaviour
{    
	public float speedX;    
	public float jumpSpeedY;    
	bool facingRight;    
	bool jumping;    
	float speed;

    SpriteRenderer spr;    
	Animator anim;    
	Rigidbody2D rb;    
	int prevState;    
	int currState;	
	
	// Use this for initialization
	void Start ()    
	{        
		spr = GetComponent<SpriteRenderer>();
		// Stores a reference to the Animator component inside anim.        
		anim = GetComponent<Animator>();
		// Stores a reference to the Rigidbody2D component inside rb.        
		rb = GetComponent<Rigidbody2D>();
		
		// Initially player is facing right.     
		facingRight = true;  
		jumping = false; 
		prevState = 0; 
		currState = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// player movement    
		MovePlayer(speed);
		
		// left player movement
		if (Input.GetKeyDown(KeyCode.LeftArrow))     
		{
			if (facingRight)
			{
				spr.flipX = true;
				facingRight = false;
			}
			prevState = currState;
			currState = 2;
			anim.SetInteger("State", currState);         
			speed = -speedX;
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			if (!facingRight)
            {
				// we don't want character to move anymore
                speed = speedX + speed;
                prevState = currState;
                currState = 0;
                anim.SetInteger("State", currState);
			}
		}

        // right player movement
        if (Input.GetKeyDown(KeyCode.RightArrow))    
		{   
			if (!facingRight)
            {
                spr.flipX = false;
                facingRight = true;
            }
            prevState = currState;
            currState = 2;
            anim.SetInteger("State", currState);         
			speed = speedX;    
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))  
		{
            if (facingRight)
            {
                // we don't want character to move anymore
                speed = speedX - speed;
                prevState = currState;
                currState = 0;
                anim.SetInteger("State", currState);
            }
		}

        // jump movement
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!jumping)
            {
                jumping = true;
                anim.SetInteger("State", 3);
                // this adds a force in the vertical direction.
                rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
            }
        }
		
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            prevState = currState;
            currState = 1;
            anim.SetInteger("State", currState);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            prevState = currState;
            currState = 0;
            anim.SetInteger("State", currState);
        }
        
    }
	
	void MovePlayer(float playerSpeed)
	{
        if (!jumping)
        {
            // code for player movement
            rb.velocity = new Vector3(speed, rb.velocity.y, 0);
        }
		else
        {
            rb.velocity = new Vector3(speed / 2, rb.velocity.y, 0);
        }
	}
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumping)
        {
            if (collision.gameObject.tag == "GROUND")
            {
                jumping = false;
                prevState = currState;
                anim.SetInteger("State", currState);
               
            }
        }
    }
}
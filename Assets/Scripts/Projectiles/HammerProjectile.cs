﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HammerProjectile : MonoBehaviour
{
    public float projectileSpeed;
    Vector3 startingPosition;
    //TileMap tilemap;
    public int direction = 1;
    public float movementSpeed = 0.05f;
    public float timeInterval = 4f;

    private bool m_FacingRight = true;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        StartCoroutine(destroyAfterInterval());
        //tilemap = GameObject.Find("Collisions");
    }

    protected IEnumerator destroyAfterInterval()
    {
        yield return new WaitForSeconds(timeInterval);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //TileBase tb = tilemap.GetTile(startingPosition);
        transform.position+=new Vector3(direction * movementSpeed * Time.fixedDeltaTime,0,0);
        //Debug.Log(tb);
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Edge");
        if(other is TilemapCollider2D)
        {
            Debug.Log(other);
            Destroy(gameObject);
        }
    }

    public void changeDirection(int hammerDirection)
    {
        if(hammerDirection!=direction)
        {
            Flip();
            direction = hammerDirection;
        }
    }

    public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    private new Rigidbody2D rigidbody;

    public float speed = 15;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();


        if (TEST.sharedInstance.transform.localScale.x == 1)
        {
            rigidbody.velocity = Vector2.right * speed * Time.fixedDeltaTime;
            GetComponent<SpriteRenderer>().flipY = false;

        }
        else
        {
            rigidbody.velocity = Vector2.left * speed * Time.fixedDeltaTime;
            GetComponent<SpriteRenderer>().flipY = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {


    }

}
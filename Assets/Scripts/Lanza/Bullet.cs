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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealthController>().DealWithDamage();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<BossHealthController>().DealWithDamage();
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }


    void FixedUpdate()
    {


    }

}
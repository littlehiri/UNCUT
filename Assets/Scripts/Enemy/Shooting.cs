using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    private float timer;
    public GameObject player;
    private void Start()
    {
        
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        //Debug.Log(distance);

        if(distance < 8)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                shoot();
            }
        }
        
        
    }
    void shoot()
    {
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
        AudioManager.sharedInstance.PlaySFX(2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool attacking = false;

    private float timeToAttack = 0.75f;
    private float timer = 0f;
    public UnityEvent<float> onReloading;


    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        onReloading?.Invoke(timer);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();

            AudioManager.sharedInstance.PlaySFX(4);
        }

        if (attacking)
        {
            timer += Time.deltaTime;
            onReloading?.Invoke(timer/ timeToAttack);

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }

        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }
}

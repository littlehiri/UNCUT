using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Comprobamos si es el jugador el que ha entrado en esa zona de trigger
        if (collision.gameObject.CompareTag("Enemy"))
        {

            //Buscará un objeto que tenga metido el script PlayerHealthController y realizará el método DealWithDamage
            //FindObjectOfType<PlayerHealthController>().DealWithDamage();

            //Llamamos al Singleton y usamos el método que necesitamos
            //PlayerHealthController.sharedInstance.DealWithDamage();

            collision.GetComponent<EnemyHealthController>().DealWithDamage();

        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.GetComponent<BossHealthController>().DealWithDamage();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public void EnemyDeathController()
    {
        AudioManager.sharedInstance.PlaySFX(1);
        //Desactivamos al enemigo padre
        transform.gameObject.SetActive(false);
        //Instanciamos el efecto de muerte del enemigo en la posición del primer hijo
        //Instantiate(deathEffect, transform.GetChild(0).position, transform.GetChild(0).rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBoss : MonoBehaviour
{
    public GameObject closeplatform;

    //Método para detectar cuando un objeto entra en la zona del activador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si es el jugador el que entra en esta zona
        if (collision.CompareTag("Player"))
        {
            closeplatform.SetActive(false);
            //Activamos al jefe final
        }
    }
}

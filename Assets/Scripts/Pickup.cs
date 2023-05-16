using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //Variable de objeto
    public bool isMoney, isHeal, isMaxHealth;

    //Variable para saber si ha sido recogido
    private bool isCollected;

    public GameObject pickupEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metodo para interactuar
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            //Si el objeto en este caso es una gema
            //if (isMoney)
            //{

            //}
        }
        if (isHeal)
        {
            //Si el jugador no tiene la vida al máximo
            if (PlayerHealthController.sharedInstance.currentHealth != PlayerHealthController.sharedInstance.maxHealth)
            {
                //Hacemos el método que cura vida al jugador
                PlayerHealthController.sharedInstance.HealPlayer();
                //El objeto ha sido recogido
                isCollected = true;
                //Llamamos al sonido de recoger una cura
                AudioManager.sharedInstance.PlaySFX(6);
                //Instanciamos el efecto de recoger el item
                Instantiate(pickupEffect, transform.position, transform.rotation);//Le pasamos el objeto a instanciar, su posición, su rotación
                //Destruimos el objeto
                Destroy(gameObject);
            }
        }
        if (isMaxHealth)
        {
            PlayerHealthController.sharedInstance.UpgradeHealthPlayer();
            //El objeto ha sido recogido
            isCollected = true;
            //Llamamos al sonido de recoger una cura
            AudioManager.sharedInstance.PlaySFX(6);
            //Instanciamos el efecto de recoger el item
            Instantiate(pickupEffect, transform.position, transform.rotation);//Le pasamos el objeto a instanciar, su posición, su rotación
            //Destruimos el objeto
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    //Variables para controlar la vida actual del jugador y el m�ximo de vida que puede tener
    public int currentHealth, maxHealth;

    //Referencia del SpriteRenderer del jugador
    private SpriteRenderer theSR;
    public float invincibleLength; //Valor que tendr� el contador de tiempo
    private float invincibleCounter; //Contador de tiempo

    //La referencia del efecto de muerte del jugador
    //public GameObject deathEffect;

    //Hacemos el Singleton de este script
    public static EnemyHealthController sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos la vida del jugador
        currentHealth = maxHealth;
        //Obtenemos el SpriteRenderer del jugador
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Comprobamos si el contador de invencibilidad a�n no est� vac�o
        if (invincibleCounter > 0)
        {
            //Le restamos 1 cada segundo a ese contador independientemente del dispositivo que ejecute el juego
            invincibleCounter -= Time.deltaTime;

            //Cuando el contador haya decrecido hasta 0
            if (invincibleCounter <= 0)
            {
                //Cambiamos el color del sprite, mantenemos el RGB y ponemos la opacidad a tope
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    //M�todo para manejar el da�o
    public void DealWithDamage()
    {
        AudioManager.sharedInstance.PlaySFX(5);
        //Si el contador de tiempo de invencibilidad se ha agotado, es decir, ya no somos invencibles
        if (invincibleCounter <= 0)
        {

            //Restamos 1 de la vida que tengamos
            currentHealth--; //currentHealth -= 1; currentHealth = currentHealth - 1;

            //Si la vida est� en 0 o por debajo (para asegurarnos de tener en cuenta solo valores positivos)
            if (currentHealth <= 0)
            {
                //Hacemos cero la vida si fuera negativa
                currentHealth = 0;
                //Desactivamos al enemigo padre
                transform.gameObject.SetActive(false);
            }
            //Si el jugador ha recibido da�o pero no ha muerto
            else
            {
                //Inicializamos el contador de invencibilidad
                invincibleCounter = invincibleLength;
                //Cambiamos el color del sprite, mantenemos el RGB y ponemos la opacidad a la mitad
                //theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, .5f);

                //Llamamos al m�todo que hace que el jugador realice el KnockBack
                EnemyController.sharedInstance.KnockBack();
            }
                
        }
    }
}

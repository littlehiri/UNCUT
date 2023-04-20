using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    //Atributo de las variables que genera un encabezado en el editor de Unity
    [Header("Shooting")]

    public GameObject Ranger;
    //Referencia a los proyectiles del enemigo
    public GameObject bullet;
    //Punto desde el que salen los proyectiles
    public Transform firePoint;
    //Tiempo entre disparos
    public float timeBetweenShots;
    //Contador de tiempo entre disparos
    private float shotCounter;

    private void Update()
    {
        var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        //Como cada bala estará referenciada (será única) puedo aplicarle los cambios que queramos
        //En este caso le diré a cada bala hacia donde debe apuntar según hacia donde mira el jefe final
        newBullet.transform.localScale = Ranger.localScale;
    }
}

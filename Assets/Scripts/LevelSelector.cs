using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string tutorial, nivel, creditos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Tutorial()
    {
        //Para saltar a la escena que le pasamos en la variable
        SceneManager.LoadScene(tutorial);
    }

    public void Nivel()
    {
        //Para saltar a la escena que le pasamos en la variable
        SceneManager.LoadScene(nivel);
    }
    public void Creditos()
    {
        //Para saltar a la escena que le pasamos en la variable
        SceneManager.LoadScene(creditos);
    }
}

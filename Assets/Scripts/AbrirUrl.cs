using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirUrl : MonoBehaviour
{
    public string URL = "https://littlehiri.itch.io/uncut";
    // Start is called before the first frame update
    public void AbrirPagina ()
    {
        Application.OpenURL(URL);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

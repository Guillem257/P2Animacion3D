using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    float contadorMuerte = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(AzriController.instance.dead || ScoutController.instance.dead)
        {
            contadorMuerte += Time.deltaTime;
            if(contadorMuerte >= 7)
            {
                SceneManager.LoadScene("Combate");
            }
        }
    }
}

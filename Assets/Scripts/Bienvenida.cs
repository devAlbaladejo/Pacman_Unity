using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bienvenida : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            FindObjectOfType<GameStatus>().puntos = 0;
            FindObjectOfType<GameStatus>().vidas = 0;
            FindObjectOfType<GameStatus>().nivelActual = 1;
            FindObjectOfType<GameStatus>().velocidadEnemigo = 0.6f;
        }
        catch(NullReferenceException e)
        {
            Console.WriteLine(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LanzarJuego()
    {
        SceneManager.LoadScene("Nivel1");
    }
}

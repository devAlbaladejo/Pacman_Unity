using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public int puntos;
    public int vidas;
    public int nivelActual;
    public float velocidadEnemigo;
    public int highScore = 0;
    private void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameStatus>().Length;

        if (gameStatusCount > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

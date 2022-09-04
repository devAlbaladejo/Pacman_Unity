using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int puntosRestantes;
    private int puntos;
    private int highScore;
    private int vidas;
    private int nivel;
    private int contadorFrutas = 0;
    private int contadorFantasmas = 0;
    [SerializeField] Text textoPuntos;
    [SerializeField] Text textoPuntosHighScore;
    [SerializeField] AudioClip sonidoStartGame;
    [SerializeField] AudioClip sonidoFantasmaVolviendo;
    [SerializeField] AudioClip sonidoFantasmaComido;
    [SerializeField] AudioClip sonidoRecogerPuntoGrande;
    [SerializeField] List<Transform> imagenesVidas;
    [SerializeField] List<Transform> imagenesFrutas;
    [SerializeField] List<Transform> puntosFrutas;
    [SerializeField] List<Transform> puntosFantasmas;
    private Vector3 respawnFrutas = new Vector3(0, -0.15f, 0);
    private bool invocarFruta = true;
    // Start is called before the first frame update
    void Start()
    {
        puntosRestantes = FindObjectsOfType<Puntos>().Length + 
            FindObjectsOfType<PuntosGrandes>().Length;
        vidas = FindObjectOfType<GameStatus>().vidas;
        puntos = FindObjectOfType<GameStatus>().puntos;
        highScore = FindObjectOfType<GameStatus>().highScore;
        nivel = FindObjectOfType<GameStatus>().nivelActual;
        textoPuntos.text = puntos + "";
        textoPuntosHighScore.text = highScore + ""; 
        StartCoroutine(InicioPartida());
        OcultarFrutas();
        OcultarPuntos();
        OcultarVidas();
        StartCoroutine(ComprobarFrutaInvocada());
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void IncrementarPuntos()
    {
        puntos += 10;
        textoPuntos.text = puntos + "";

        ComprobarHighScore(); 

        FindObjectOfType<GameStatus>().puntos = puntos;
        ComprobarPuntosRestantes();
    }
    public void IncrementarPuntosGrandes()
    {
        puntos += 50;
        textoPuntos.text = puntos + "";

        ComprobarHighScore();

        FindObjectOfType<GameStatus>().puntos = puntos;
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].SendMessage("ComerFantasmas");
        }
        ComprobarPuntosRestantes();
        StartCoroutine(TemporizadorComerFantasmas());
    }
    public void ComprobarPuntosRestantes()
    {
        puntosRestantes--;
        if (puntosRestantes <= 0)
        {
            if(nivel == 2)
            {
                SceneManager.LoadScene("Fin");
                GuardarHighScore();
            }
            else
                SiguienteNivel();
        }
    }
    public void SiguienteNivel()
    {
        nivel++;
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].SendMessage("IncrementarVelocidad");
        }
            
        FindObjectOfType<GameStatus>().nivelActual = nivel;
        SceneManager.LoadScene("Nivel" + nivel);
    }
    public IEnumerator InicioPartida()
    {
        AudioSource.PlayClipAtPoint(sonidoStartGame, Camera.main.transform.position);
        yield return new WaitForSeconds(4.5f);
        FindObjectOfType<Pacman>().SendMessage("EstablecerVelocidad");
        FindObjectOfType<Enemigo>().SendMessage("EstablecerVelocidad");
        ReinicioNivel();
    }
    public void PerderVida()
    {
        FindObjectOfType<Pacman>().SendMessage("Muerte");
        GetComponent<AudioSource>().Pause();
        ReiniciarPuntosFantasmas();
        imagenesVidas[vidas].gameObject.SetActive(false);
        vidas++;
        FindObjectOfType<GameStatus>().vidas = vidas;

        if (vidas >= 5)
        {
            SceneManager.LoadScene("GameOver");
            GuardarHighScore();
        }
    }
    public void ReinicioNivel()
    {
        GetComponent<AudioSource>().Play();
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].SendMessage("EstablecerVelocidad");
        }
    }
    public void InvocarFruta()
    {
        imagenesFrutas[contadorFrutas].gameObject.SetActive(true);
        Instantiate(imagenesFrutas[contadorFrutas],
            respawnFrutas,
            Quaternion.identity);
    }
    public void OcultarVidas()
    {
        for (int i = 0; i < vidas; i++)
        {
            imagenesVidas[i].gameObject.SetActive(false);
        }
    }
    public void OcultarFrutas()
    {
        for (int i = 0; i < imagenesFrutas.Count; i++)
        {
            imagenesFrutas[i].gameObject.SetActive(false);
        }
    }
    public void OcultarPuntos()
    {
        for (int i = 0; i < puntosFrutas.Count; i++)
        {
            puntosFrutas[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < puntosFantasmas.Count; i++)
        {
            puntosFantasmas[i].gameObject.SetActive(false);
        }
    }
    public void FrutaObtenida()
    {
        puntosFrutas[contadorFrutas].gameObject.SetActive(false);
        contadorFrutas++;
    }
    public void MostrarPuntosFruta()
    {
        puntosFrutas[contadorFrutas].gameObject.SetActive(true);
        int obtenerPuntosFrutas = 0;
        switch (contadorFrutas)
        {
            case 0: obtenerPuntosFrutas = 100; break;
            case 1: obtenerPuntosFrutas = 300; break;
            case 2: obtenerPuntosFrutas = 500; break;
            case 3: obtenerPuntosFrutas = 700; break;
            case 4: obtenerPuntosFrutas = 1000; break;
        }
        puntos += obtenerPuntosFrutas;
        textoPuntos.text = puntos + "";
        ComprobarHighScore();
        FindObjectOfType<GameStatus>().puntos = puntos;
        Invoke("FrutaObtenida", 2);
    }
    public IEnumerator ComprobarFrutaInvocada()
    {
        float tiempoInvocarFruta = Random.Range(20, 40);
        yield return new WaitForSeconds(tiempoInvocarFruta);

        if (invocarFruta)
        {
            InvocarFruta();
            invocarFruta = false;
        }

        if (contadorFrutas < imagenesFrutas.Count)
            StartCoroutine(ComprobarFrutaInvocada());
    }
    public void FrutaComida()
    {
        invocarFruta = true;
    }

    public IEnumerator Pausa(int seg)
    {
        GetComponent<AudioSource>().Pause();
        AudioSource.PlayClipAtPoint(sonidoFantasmaComido, Camera.main.transform.position);
        AudioSource.PlayClipAtPoint(sonidoFantasmaVolviendo, Camera.main.transform.position);
        yield return new WaitForSeconds(seg);
        GetComponent<AudioSource>().Play();
    }
    public void RegresoFantasma(Vector3 posicion)
    {
        MostrarPuntosFantasma(posicion);
        StartCoroutine(Pausa(2));
    }
    public void MostrarPuntosFantasma(Vector3 posicion)
    {
        if (contadorFantasmas < 4)
        {
            puntosFantasmas[contadorFantasmas].transform.SetPositionAndRotation(posicion,Quaternion.identity);
            puntosFantasmas[contadorFantasmas].gameObject.SetActive(true);
            int obtenerPuntosFantasmas = 0;
            switch(contadorFantasmas)
            {
                case 0: obtenerPuntosFantasmas = 200; break;
                case 1: obtenerPuntosFantasmas = 400; break;
                case 2: obtenerPuntosFantasmas = 800; break;
                case 3: obtenerPuntosFantasmas = 1600; break;
            }
            puntos += obtenerPuntosFantasmas;
            textoPuntos.text = puntos + "";
            ComprobarHighScore();
            FindObjectOfType<GameStatus>().puntos = puntos;
            Invoke("FantasmaComido", 0.5f);
        }
    }
    public void FantasmaComido()
    {
        puntosFantasmas[contadorFantasmas].gameObject.SetActive(false);
        contadorFantasmas++;
    }
    public IEnumerator TemporizadorComerFantasmas()
    {
        AudioSource.PlayClipAtPoint(sonidoRecogerPuntoGrande, Camera.main.transform.position);
        yield return new WaitForSeconds(2.1f);
    }
    public void ReiniciarPuntosFantasmas()
    {
        contadorFantasmas = 0;
    }
    public void GuardarHighScore()
    {
        FindObjectOfType<GameStatus>().highScore = highScore;
    }
    public void ComprobarHighScore()
    {
        if (puntos >= highScore)
        {
            highScore = puntos;
            textoPuntosHighScore.text = highScore + "";
        }
    }
}

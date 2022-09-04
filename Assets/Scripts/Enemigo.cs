using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private float xInicial, yInicial;
    private float velocidad;
    Animator animacion;
    [SerializeField] List<Transform> wayPoints = null;
    private float distanciaCambio = 0.01f;
    private byte siguientePosicion = 0;
    private bool sePuedeComerRojo, sePuedeComerRosa, sePuedeComerAzul, sePuedeComerNaranja;
    private bool rojoComido, rosaComido, azulComido, naranjaComido;
    private bool rojoRegreso, rosaRegreso, azulRegreso, naranjaRegreso;

    // Start is called before the first frame update
    void Start()
    {
        xInicial = transform.position.x;
        yInicial = transform.position.y;
        animacion = gameObject.GetComponent<Animator>();
        sePuedeComerRojo = sePuedeComerRosa = sePuedeComerAzul = sePuedeComerNaranja = false;
        rojoComido = rosaComido = azulComido = naranjaComido = false;
        rojoRegreso = rosaRegreso = azulRegreso = naranjaRegreso = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            wayPoints[siguientePosicion].transform.position,
            velocidad * Time.deltaTime);
        
        if (tag == "Rojo")
        {
            if (!rojoComido)
                MovimientosFantasmas("Rojo", sePuedeComerRojo);
            else
                FantasmaComido();
        }
        if (tag == "Rosa")
        {
            if (!rosaComido)
                MovimientosFantasmas("Rosa", sePuedeComerRosa);
            else
                FantasmaComido();
        }
        if (tag == "Azul")
        {
            if (!azulComido)
                MovimientosFantasmas("Azul", sePuedeComerAzul);
            else
                FantasmaComido();
        }
        if (tag == "Naranja")
        {
            if (!naranjaComido)
                MovimientosFantasmas("Naranja", sePuedeComerNaranja);
            else
                FantasmaComido();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(tag == "Rojo" && sePuedeComerRojo)
            {
                if(!rojoRegreso)
                {
                    rojoComido = true;
                    ReproducirSonidoComido(transform.position);
                    rojoRegreso = true;
                }
            }
            else if(tag == "Rosa" && sePuedeComerRosa)
            {
                if (!rosaRegreso)
                {
                    rosaComido = true;
                    ReproducirSonidoComido(transform.position);
                    rosaRegreso = true;
                }
            }
            else if(tag == "Azul" && sePuedeComerAzul)
            {
                if (!azulRegreso)
                {
                    azulComido = true;
                    ReproducirSonidoComido(transform.position);
                    azulRegreso = true;
                }
            }
            else if(tag == "Naranja" && sePuedeComerNaranja)
            {
                if (!naranjaRegreso)
                {
                    naranjaComido = true;
                    ReproducirSonidoComido(transform.position);
                    naranjaRegreso = true;
                }
            }
            else{
                FindObjectOfType<GameController>().SendMessage("PerderVida");
                Respawn();
            }
        }
    }
    public void EstablecerVelocidad()
    {
        velocidad = FindObjectOfType<GameStatus>().velocidadEnemigo;
    }
    public void Respawn()
    {
        velocidad = 0;
        transform.position = new Vector3(xInicial, yInicial, 0);
        siguientePosicion = 0;
        if (tag == "Rojo")
            animacion.Play("EnemigoRojoEstatico");
        if (tag == "Rosa")
            animacion.Play("EnemigoRosaEstatico");
        if (tag == "Azul")
            animacion.Play("EnemigoAzulEstatico");
        if (tag == "Naranja")
            animacion.Play("EnemigoNaranjaEstatico");
        sePuedeComerRojo = sePuedeComerRosa = sePuedeComerAzul = sePuedeComerNaranja = false;
        rojoComido = rosaComido = azulComido = naranjaComido = false;
        rojoRegreso = rosaRegreso = azulRegreso = naranjaRegreso = false;
    }
    public void IncrementarVelocidad()
    {
        velocidad = 0.75f;
        FindObjectOfType<GameStatus>().velocidadEnemigo = velocidad;
    }
    public void ComerFantasmas()
    {
        animacion.Play("EnemigoComestible");
        sePuedeComerRojo = sePuedeComerRosa = sePuedeComerAzul = sePuedeComerNaranja = true;
        Invoke("FantasmaNoComestible", 6);
    }
    public void MovimientosFantasmas(string color, bool comer)
    {
        int posicionAnterior = siguientePosicion - 1;
        if (siguientePosicion == 0)
            posicionAnterior = wayPoints.Count - 1;
        if (!comer)
        {
            if (wayPoints[posicionAnterior].transform.position.x <
                wayPoints[siguientePosicion].transform.position.x &&
                wayPoints[posicionAnterior].transform.position.y ==
                wayPoints[siguientePosicion].transform.position.y)
                animacion.Play("Enemigo" + color + "Derecha");
            else if (wayPoints[posicionAnterior].transform.position.x >
                wayPoints[siguientePosicion].transform.position.x &&
                wayPoints[posicionAnterior].transform.position.y ==
                wayPoints[siguientePosicion].transform.position.y)
                animacion.Play("Enemigo" + color + "Izquierda");
            else if (wayPoints[posicionAnterior].transform.position.y <
                 wayPoints[siguientePosicion].transform.position.y &&
                 wayPoints[posicionAnterior].transform.position.x ==
                 wayPoints[siguientePosicion].transform.position.x)
                animacion.Play("Enemigo" + color + "Arriba");
            else if (wayPoints[posicionAnterior].transform.position.y >
                 wayPoints[siguientePosicion].transform.position.y &&
                 wayPoints[posicionAnterior].transform.position.x ==
                 wayPoints[siguientePosicion].transform.position.x)
                animacion.Play("Enemigo" + color + "Abajo");
        }
        if (Vector3.Distance(transform.position,
                wayPoints[siguientePosicion].transform.position) < distanciaCambio)
        {
            siguientePosicion++;
            if (siguientePosicion >= wayPoints.Count)
                siguientePosicion = 0;
        }
    }
    public void FantasmaComido()
    {
        int posicionPosterior = siguientePosicion + 1;
        if (wayPoints[siguientePosicion].transform.position.x >
            wayPoints[posicionPosterior].transform.position.x &&
            wayPoints[siguientePosicion].transform.position.y ==
            wayPoints[posicionPosterior].transform.position.y)
            animacion.Play("EnemigoInvisibleDerecha");
        else if (wayPoints[siguientePosicion].transform.position.x <
            wayPoints[posicionPosterior].transform.position.x &&
            wayPoints[siguientePosicion].transform.position.y ==
            wayPoints[posicionPosterior].transform.position.y)
            animacion.Play("EnemigoInvisibleIzquierda");
        else if (wayPoints[siguientePosicion].transform.position.y >
            wayPoints[posicionPosterior].transform.position.y &&
            wayPoints[siguientePosicion].transform.position.x ==
            wayPoints[posicionPosterior].transform.position.x)
            animacion.Play("EnemigoInvisibleArriba");
        else if (wayPoints[siguientePosicion].transform.position.y <
            wayPoints[posicionPosterior].transform.position.y &&
            wayPoints[siguientePosicion].transform.position.x ==
            wayPoints[posicionPosterior].transform.position.x)
            animacion.Play("EnemigoInvisibleAbajo");
        if (Vector3.Distance(transform.position,
                wayPoints[siguientePosicion].transform.position) < distanciaCambio)
        {
            siguientePosicion--;
            if (siguientePosicion == 0)
            {
                velocidad = FindObjectOfType<GameStatus>().velocidadEnemigo;
                if (tag == "Rojo")
                {
                    sePuedeComerRojo = false;
                    rojoComido = false;
                    rojoRegreso = false;
                }
                if (tag == "Rosa")
                {
                    sePuedeComerRosa = false;
                    rosaComido = false;
                    rosaRegreso = false;
                }
                if (tag == "Azul")
                {
                    sePuedeComerAzul = false;
                    azulComido = false;
                    azulRegreso = false;
                }
                if (tag == "Naranja")
                {
                    sePuedeComerNaranja = false;
                    naranjaComido = false;
                    naranjaRegreso = false;
                }
            }
        }
    }
    public void ReproducirSonidoComido(Vector3 posicion)
    {
        velocidad = 1.5f;
        siguientePosicion--;
        FindObjectOfType<GameController>().SendMessage("RegresoFantasma", posicion);
    }
    public void FantasmaNoComestible()
    {
        sePuedeComerRojo = sePuedeComerRosa = sePuedeComerAzul = sePuedeComerNaranja = false;
        FindObjectOfType<GameController>().SendMessage("ReiniciarPuntosFantasmas");
    }
}

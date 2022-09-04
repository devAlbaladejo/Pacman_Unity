using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    private float xInicial, yInicial;
    private float velocidad;
    Animator animacion;
    private Vector3 limiteDerecha = new Vector3(1.0f, 0.08f, 0);
    private Vector3 limiteIzquierda = new Vector3(-1.0f, 0.08f, 0);
    [SerializeField] AudioClip sonidoMuerte;
    // Start is called before the first frame update
    void Start()
    {
        xInicial = transform.position.x;
        yInicial = transform.position.y;
        animacion = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (velocidad > 0)
        {
            float horizontal = Input.GetAxis("Horizontal");
            transform.Translate(horizontal * velocidad * Time.deltaTime, 0, 0);

            if (horizontal > 0.1f)
                animacion.Play("PacmanMovimientoDerecha");
            if (horizontal < -0.1f)
                animacion.Play("PacmanMovimientoIzquierda");

            float vertical = Input.GetAxis("Vertical");
            transform.Translate(0, vertical * velocidad * Time.deltaTime, 0);

            if (vertical > 0.1f)
                animacion.Play("PacmanMovimientoArriba");
            if (vertical < -0.1f)
                animacion.Play("PacmanMovimientoAbajo");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SalidaIzquierda")
            transform.SetPositionAndRotation(limiteDerecha, Quaternion.identity);
        if (other.tag == "SalidaDerecha")
            transform.SetPositionAndRotation(limiteIzquierda, Quaternion.identity);
    }
    public void EstablecerVelocidad()
    {
        velocidad = 0.6f;
    }
    public void Respawn()
    {
        transform.position = new Vector3(xInicial, yInicial, 0);
        EstablecerVelocidad();
        FindObjectOfType<GameController>().SendMessage("ReinicioNivel");
    }
    public void Muerte()
    {
        velocidad = 0;
        animacion.Play("PacmanMuerte");
        AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position);
        Enemigo[] enemigos = FindObjectsOfType<Enemigo>();
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].SendMessage("Respawn");
        }
        Invoke("Respawn", 2);
    }
}

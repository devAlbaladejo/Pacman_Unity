using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscenaFinal : MonoBehaviour
{
    private float velocidad = 3;
    [SerializeField] Text textoFinal;
    Animator animacion;
    // Start is called before the first frame update
    void Start()
    {
        textoFinal.enabled = false;
        animacion = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocidad * Time.deltaTime, 0,0);
        
        if (tag == "Rojo")
            animacion.Play("EnemigoRojoDerecha");

        if (tag == "Player")
            animacion.Play("PacmanMovimientoDerecha");

        if (transform.position.x > 11)
        {
            velocidad = 0;
            textoFinal.enabled = true;
            StartCoroutine(Inicio());
        }
    }

    public IEnumerator Inicio()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Bienvenida");
    }
}

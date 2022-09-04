using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frutas : MonoBehaviour
{
    [SerializeField] AudioClip sonidoRecogerFruta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<GameController>().SendMessage("MostrarPuntosFruta");
            FindObjectOfType<GameController>().SendMessage("FrutaComida");
            AudioSource.PlayClipAtPoint(sonidoRecogerFruta, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}

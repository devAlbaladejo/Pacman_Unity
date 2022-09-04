using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntos : MonoBehaviour
{
    [SerializeField] AudioClip sonidoRecogerPunto;
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
            FindObjectOfType<GameController>().SendMessage("IncrementarPuntos");
            AudioSource.PlayClipAtPoint(sonidoRecogerPunto, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Jugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("[ENTER] Colisioné con: " + col.gameObject.name);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        Debug.Log("[STAY] Sigo en contacto con: " + col.gameObject.name);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("[EXIT] Dejé de colisionar con: " + col.gameObject.name);
    }

}

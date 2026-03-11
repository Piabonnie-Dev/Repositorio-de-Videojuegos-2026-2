using UnityEngine;

public class Jugador : MonoBehaviour
{
    public bool enSuelo = false;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
            Debug.Log("[ENTER] Colisioné con: " + col.gameObject.name);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
            Debug.Log("[EXIT] Dejé de colisionar con: " + col.gameObject.name);
        }
    }
}
using UnityEngine;

public class ControldeJugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if(h != 0)
        {
            Debug.Log("Movimiento horizontal: " + h);

        }

if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Salto");
        }



    }
}


using UnityEngine;

public class ControldeJugador : MonoBehaviour
{
    public float velocidadActual = 0f;
    public float velocidadMaxima = 5f;
    public float aceleracion = 10F;

 public float velocidadVertical = 0f;
 public float gravedad = -20f;
    public float tiempoMaxSalto = 0.2f;
    private float tiempoSaltoActual = 0f;
    private float tiempoAnterior;

    private Jugador jugador;
private Rigidbody2D rb;
    private void Awake()
    {
        jugador = GetComponent<Jugador>();
rb = GetComponent<Rigidbody2D>();


        if (jugador == null)
        {
            Debug.LogError("No se encontró el componente Jugador en el mismo GameObject.", this);
            enabled = false; // Desactiva este script si no se encuentra el componente Jugador
            return;
        }
        if (rb == null)
        {
            Debug.LogError("No se encontró el componente Rigidbody2D en el mismo GameObject.", this);
            enabled = false; // Desactiva este script si no se encuentra el componente Rigidbody2D
            return;
        }
    }
     
    





    private void Start()
    {
        tiempoAnterior = Time.fixedTime;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       float delta = Time.fixedTime - tiempoAnterior;
        tiempoAnterior = Time.fixedTime;
        
        float h = Input.GetAxis("Horizontal");
        velocidadActual += h * aceleracion * delta;
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMaxima, velocidadMaxima);
        
        

float jumpAxis = Input.GetAxis("Jump");
        if (jumpAxis > 0f && jugador.enSuelo)
        {
            velocidadVertical = 10f;
            jugador.enSuelo = false;
            tiempoSaltoActual = 0f;
            
        }
//salto sostenido
if (!jugador.enSuelo && jumpAxis > 0f && tiempoSaltoActual < tiempoMaxSalto)
        {

                
                velocidadVertical += 20f * delta;
                tiempoSaltoActual += delta;
            


        }

if (jumpAxis == 0f)
        {
            tiempoSaltoActual = tiempoMaxSalto;
        }
//gravedad
    if(jugador.enSuelo && velocidadVertical < 0f)
        {
            velocidadVertical = 0f;
        }

        else {
            velocidadVertical += gravedad * delta;
        }

        //Mover con fisica
        Vector2 nuevapos = rb.position + new Vector2(velocidadActual * delta, velocidadVertical * delta);
        rb.MovePosition(nuevapos);

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

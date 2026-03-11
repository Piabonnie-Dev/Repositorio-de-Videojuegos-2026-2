using UnityEngine;

public class ControldeJugador : MonoBehaviour
{
    [Header("Movimiento horizontal")]
    public float velocidadActual = 0f;
    public float velocidadMaxima = 5f;
    public float aceleracion = 10f;
    public float desaceleracion = 8f;

    [Header("Movimiento vertical")]
    public float velocidadVertical = 0f;
    public float fuerzaSalto = 10f;
    public float fuerzaExtraSalto = 20f;
    public float gravedad = -20f;
    public float gravedadCaida = -30f;
    public float velocidadMaximaVertical = 12f;
    public float velocidadMaxCaida = -20f;
    public float tiempoMaxSalto = 0.2f;

    [Header("Ayudas de salto")]
    public float tiempoCoyote = 0.1f;
    public float tiempoBufferSalto = 0.1f;

    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;
    private float tiempoSaltoActual = 0f;
    private float tiempoAnterior = 0f;

    [Header("Estados para animación")]
    public bool estaCaminando;
    public bool estaSaltando;
    public bool estaCayendo;

    private Jugador jugador;
    private Rigidbody2D rb;

    // Inputs guardados
    private float h;
    private bool saltoMantenido;
    private bool saltoSoltado;




private SpriteRenderer sr;



    private void Awake()
    {
        jugador = GetComponent<Jugador>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (jugador == null)
        {
            Debug.LogError("No se encontró el componente Jugador en el mismo GameObject.", this);
            enabled = false;
            return;
        }

        if (rb == null)
        {
            Debug.LogError("No se encontró el componente Rigidbody2D en el mismo GameObject.", this);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        tiempoAnterior = Time.fixedTime;
    }

    private void Update()
    {
        // Guardamos inputs en Update para que no se pierdan
        h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            bufferTimer = tiempoBufferSalto;
        }

        saltoMantenido = Input.GetButton("Jump");

        if (Input.GetButtonUp("Jump"))
        {
            saltoSoltado = true;
        }
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedTime - tiempoAnterior;
        tiempoAnterior = Time.fixedTime;

        if (delta <= 0f) return;

        // Reducir buffer con el tiempo
        bufferTimer -= delta;

        // --------------------------
        // 1. MOVIMIENTO HORIZONTAL
        // --------------------------
        if (h != 0f)
        {
            velocidadActual += h * aceleracion * delta;
        }
        else
        {
            // Desaceleración automática
            if (velocidadActual > 0f)
                velocidadActual -= desaceleracion * delta;
            else if (velocidadActual < 0f)
                velocidadActual += desaceleracion * delta;

            if (Mathf.Abs(velocidadActual) < 0.1f)
                velocidadActual = 0f;
        }

        // Límite horizontal
        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMaxima, velocidadMaxima);

        // Dirección visual del personaje
        if (h > 0f)
    sr.flipX = false;
else if (h < 0f)
    sr.flipX = true;

        // --------------------------
        // 2. COYOTE TIME
        // --------------------------
        if (jugador.enSuelo)
        {
            coyoteTimer = tiempoCoyote;

            if (velocidadVertical < 0f)
                velocidadVertical = 0f;
        }
        else
        {
            coyoteTimer -= delta;
        }

        // --------------------------
        // 3. SALTO CON BUFFER + COYOTE
        // --------------------------
        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            velocidadVertical = fuerzaSalto;
            jugador.enSuelo = false;
            bufferTimer = 0f;
            coyoteTimer = 0f;
            tiempoSaltoActual = 0f;
        }

        // --------------------------
        // 4. SALTO SOSTENIDO
        // --------------------------
        if (!jugador.enSuelo && saltoMantenido && velocidadVertical > 0f && tiempoSaltoActual < tiempoMaxSalto)
        {
            velocidadVertical += fuerzaExtraSalto * delta;
            tiempoSaltoActual += delta;
        }

        if (saltoSoltado)
        {
            tiempoSaltoActual = tiempoMaxSalto;
            saltoSoltado = false;
        }

        // --------------------------
        // 5. GRAVEDAD MEJORADA
        // --------------------------
        if (!jugador.enSuelo)
        {
            if (velocidadVertical < 0f)
                velocidadVertical += gravedadCaida * delta;
            else
                velocidadVertical += gravedad * delta;
        }

        // Límite vertical
        velocidadVertical = Mathf.Clamp(velocidadVertical, velocidadMaxCaida, velocidadMaximaVertical);

        // --------------------------
        // 6. MOVIMIENTO FINAL
        // --------------------------
        Vector2 nuevaPos = rb.position + new Vector2(
            velocidadActual * delta,
            velocidadVertical * delta
        );

        rb.MovePosition(nuevaPos);

        // --------------------------
        // 7. ESTADOS PARA ANIMACIÓN
        // --------------------------
        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f;
        estaSaltando = velocidadVertical > 0.1f && !jugador.enSuelo;
        estaCayendo = velocidadVertical < -0.1f && !jugador.enSuelo;
    }
}


    
    
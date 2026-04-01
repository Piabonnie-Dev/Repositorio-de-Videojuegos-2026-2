using UnityEngine;

public class ControldeJugador : MonoBehaviour
{
    [Header("Movimiento horizontal")]
    public float velocidadActual = 0f;
    public float velocidadMaxima = 5f;
    public float aceleracion = 18f;
    public float desaceleracion = 20f;

    [Header("Movimiento vertical")]
    public float velocidadVertical = 0f;
    public float fuerzaSalto = 10f;
    public float fuerzaExtraSalto = 18f;
    public float gravedad = -20f;
    public float gravedadCaida = -30f;
    public float velocidadMaximaVertical = 12f;
    public float velocidadMaxCaida = -20f;
    public float tiempoMaxSalto = 0.20f;

    [Header("Ayudas de salto")]
    public float tiempoCoyote = 0.10f;
    public float tiempoBufferSalto = 0.10f;
    [Range(0f, 1f)]
    public float corteSalto = 0.5f;

    [Header("Estados de animación")]
    public bool Caminando = false;
    public bool Saltando = false;
    public bool Cayendo = false;

    [Header("Sonido")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoPaso;
    public AudioClip sonidoImpacto;
    public float intervaloPasos = 0.35f;

    private Jugador jugador;
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource audioSrc;
    private SpriteRenderer sr;

    private float h = 0f;
    private float tiempoSaltoActual = 0f;
    private float coyoteTimer = 0f;
    private float bufferTimer = 0f;
    private float pasoTimer = 0f;

    private bool saltoMantenido = false;
    private bool saltoSoltado = false;
    private bool estabaEnSuelo = false;

    private void Awake()
    {
        jugador = GetComponent<Jugador>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
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
        estabaEnSuelo = jugador.enSuelo;
    }

    private void Update()
    {
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
        float delta = Time.fixedDeltaTime;

        bufferTimer -= delta;

        if (h != 0f)
        {
            velocidadActual += h * aceleracion * delta;
        }
        else
        {
            if (velocidadActual > 0f)
            {
                velocidadActual -= desaceleracion * delta;
                if (velocidadActual < 0f) velocidadActual = 0f;
            }
            else if (velocidadActual < 0f)
            {
                velocidadActual += desaceleracion * delta;
                if (velocidadActual > 0f) velocidadActual = 0f;
            }
        }

        velocidadActual = Mathf.Clamp(velocidadActual, -velocidadMaxima, velocidadMaxima);

        if (sr != null)
        {
            if (h > 0f) sr.flipX = false;
            else if (h < 0f) sr.flipX = true;
        }

        if (jugador.enSuelo)
        {
            coyoteTimer = tiempoCoyote;

            if (velocidadVertical < 0f)
                velocidadVertical = 0f;

            if (!estabaEnSuelo)
            {
                ReproducirSonido(sonidoImpacto);
            }
        }
        else
        {
            coyoteTimer -= delta;
        }

        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            velocidadVertical = fuerzaSalto;
            jugador.enSuelo = false;
            bufferTimer = 0f;
            coyoteTimer = 0f;
            tiempoSaltoActual = 0f;

            ReproducirSonido(sonidoSalto);
        }

        if (!jugador.enSuelo && saltoMantenido && velocidadVertical > 0f && tiempoSaltoActual < tiempoMaxSalto)
        {
            velocidadVertical += fuerzaExtraSalto * delta;
            tiempoSaltoActual += delta;
        }

        if (saltoSoltado)
        {
            if (velocidadVertical > 0f)
                velocidadVertical *= corteSalto;

            tiempoSaltoActual = tiempoMaxSalto;
            saltoSoltado = false;
        }

        if (!jugador.enSuelo)
        {
            if (velocidadVertical < 0f)
                velocidadVertical += gravedadCaida * delta;
            else
                velocidadVertical += gravedad * delta;
        }

        velocidadVertical = Mathf.Clamp(velocidadVertical, velocidadMaxCaida, velocidadMaximaVertical);

        Vector2 movimientoPropio = new Vector2(velocidadActual * delta, velocidadVertical * delta);
        Vector2 movimientoPlataforma = jugador.ObtenerDeltaPlataforma();

        Vector2 nuevaPos = rb.position + movimientoPropio + movimientoPlataforma;
        rb.MovePosition(nuevaPos);

        Caminando = Mathf.Abs(velocidadActual) > 0.1f && jugador.enSuelo;
        Saltando = velocidadVertical > 0.1f && !jugador.enSuelo;
        Cayendo = velocidadVertical < -0.1f && !jugador.enSuelo;

        if (anim != null)
        {
            anim.SetBool("Caminando", Caminando);
            anim.SetBool("Saltando", Saltando);
            anim.SetBool("Cayendo", Cayendo);
        }

        if (Caminando && jugador.enSuelo)
        {
            pasoTimer -= delta;

            if (pasoTimer <= 0f)
            {
                ReproducirSonido(sonidoPaso);
                pasoTimer = intervaloPasos;
            }
        }
        else
        {
            pasoTimer = 0f;
        }

        estabaEnSuelo = jugador.enSuelo;
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSrc != null && clip != null)
        {
            audioSrc.PlayOneShot(clip);
        }
    }
}
    
    
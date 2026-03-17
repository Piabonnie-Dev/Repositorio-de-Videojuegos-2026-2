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
    public bool estaCaminando = false;
    public bool estaSaltando = false;
    public bool estaCayendo = false;

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

        if (sr == null)
        {
            Debug.LogWarning("No se encontró SpriteRenderer. El personaje no podrá voltearse visualmente.", this);
        }

        if (anim == null)
        {
            Debug.LogWarning("No se encontró Animator. Las animaciones no se actualizarán.", this);
        }

        if (audioSrc == null)
        {
            Debug.LogWarning("No se encontró AudioSource. No se reproducirá sonido.", this);
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

        // MOVIMIENTO HORIZONTAL
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

        // VOLTEAR SPRITE SIN CAMBIAR ESCALA
        if (sr != null)
        {
            if (h > 0f) sr.flipX = false;
            else if (h < 0f) sr.flipX = true;
        }

        // COYOTE TIME Y ATERRIZAJE
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

        // JUMP BUFFER + COYOTE
        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            velocidadVertical = fuerzaSalto;
            jugador.enSuelo = false;
            bufferTimer = 0f;
            coyoteTimer = 0f;
            tiempoSaltoActual = 0f;

            ReproducirSonido(sonidoSalto);
        }

        // SALTO SOSTENIDO
        if (!jugador.enSuelo && saltoMantenido && velocidadVertical > 0f && tiempoSaltoActual < tiempoMaxSalto)
        {
            velocidadVertical += fuerzaExtraSalto * delta;
            tiempoSaltoActual += delta;
        }

        // CORTAR SALTO AL SOLTAR BOTÓN
        if (saltoSoltado)
        {
            if (velocidadVertical > 0f)
                velocidadVertical *= corteSalto;

            tiempoSaltoActual = tiempoMaxSalto;
            saltoSoltado = false;
        }

        // GRAVEDAD MEJORADA
        if (!jugador.enSuelo)
        {
            if (velocidadVertical < 0f)
                velocidadVertical += gravedadCaida * delta;
            else
                velocidadVertical += gravedad * delta;
        }

        velocidadVertical = Mathf.Clamp(velocidadVertical, velocidadMaxCaida, velocidadMaximaVertical);

        // MOVER PERSONAJE
        Vector2 nuevaPos = rb.position + new Vector2(velocidadActual * delta, velocidadVertical * delta);
        rb.MovePosition(nuevaPos);

        // ESTADOS PARA ANIMACIÓN
        estaCaminando = Mathf.Abs(velocidadActual) > 0.1f && jugador.enSuelo;
        estaSaltando = velocidadVertical > 0.1f && !jugador.enSuelo;
        estaCayendo = velocidadVertical < -0.1f && !jugador.enSuelo;

        // ACTUALIZAR ANIMATOR
        if (anim != null)
        {
            anim.SetBool("Caminando", estaCaminando);
            anim.SetBool("Saltando", estaSaltando);
            anim.SetBool("Cayendo", estaCayendo);
        }

        // SONIDO DE PASOS
        if (estaCaminando && jugador.enSuelo)
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

    
    
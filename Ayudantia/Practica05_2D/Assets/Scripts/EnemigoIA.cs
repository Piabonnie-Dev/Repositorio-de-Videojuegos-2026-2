using UnityEngine;

public class EnemigoIA : Personaje
{
    [Header("Movimiento")]
    public float direccion = -1f;

    [Header("Pisotón tipo Goomba")]
    public float margenPisoton = 0.08f;
    public float reboteJugador = 8f;

    private Animator anim;
    private Collider2D colEnemigo;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        colEnemigo = GetComponent<Collider2D>();

        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);

        if (anim != null)
        {
            anim.SetBool("Caminando", Mathf.Abs(direccion) > 0.01f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Dar vuelta al chocar con pared
        if (collision.gameObject.CompareTag("Pared"))
        {
            Girar();
            return;
        }

        // Solo nos interesa la colisión con el jugador
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Jugador jugador = collision.gameObject.GetComponent<Jugador>();
        ControldeJugador control = collision.gameObject.GetComponent<ControldeJugador>();
        Collider2D colJugador = collision.gameObject.GetComponent<Collider2D>();

        if (jugador == null || control == null || colJugador == null || colEnemigo == null)
            return;

        // Detectar si los pies del jugador están por encima de la cabeza del enemigo
        bool jugadorEncima =
            colJugador.bounds.min.y >= colEnemigo.bounds.max.y - margenPisoton;

        // Detectar si el jugador va cayendo o al menos no va subiendo
        bool jugadorCayendo = control.velocidadVertical <= 0f;

        if (jugadorEncima && jugadorCayendo)
        {
            // Muere el enemigo
            RecibirDaño(1);

            // Rebote del jugador
            control.velocidadVertical = reboteJugador;
            jugador.enSuelo = false;
        }
        else
        {
            // Si toca por lado o por abajo, muere el jugador
            jugador.RecibirDaño(1);
        }
    }

    private void Girar()
    {
        direccion *= -1f;

        if (sr != null)
            sr.flipX = !sr.flipX;
    }
}
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidadBase = 10f;
    public float tiempoVida = 2f;
    public int daño = 1;

    private Vector2 direccion;
    private float velocidadExtra = 0f;
    private bool impacto = false;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Inicializar(Vector2 dir, float velocidadJugador)
    {
        direccion = dir.normalized;
        velocidadExtra = Mathf.Abs(velocidadJugador);

        // VOLTEAR SOLO EL SPRITE DEL DISPARO
        if (sr != null)
        {
            sr.flipX = direccion.x < 0f;
        }

        float velocidadFinal = velocidadBase + velocidadExtra;

        if (rb != null)
        {
            rb.linearVelocity = direccion * velocidadFinal;
        }

        Destroy(gameObject, tiempoVida);
    }

    private void FixedUpdate()
    {
        if (impacto) return;

        if (rb == null)
        {
            float velocidadFinal = velocidadBase + velocidadExtra;
            transform.position += (Vector3)(direccion * velocidadFinal * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impacto) return;

        if (other.CompareTag("Player"))
            return;

        EnemigoIA enemigo = other.GetComponent<EnemigoIA>();
        if (enemigo == null)
            enemigo = other.GetComponentInParent<EnemigoIA>();

        if (enemigo != null)
        {
            enemigo.RecibirDaño(daño);
            ApagarYDestruir();
            return;
        }

        if (other.CompareTag("Pared") ||
            other.CompareTag("Obstaculo") ||
            other.CompareTag("Suelo") ||
            other.CompareTag("Plataforma"))
        {
            ApagarYDestruir();
        }
    }

    private void ApagarYDestruir()
    {
        impacto = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (col != null)
            col.enabled = false;

        Destroy(gameObject);
    }
}
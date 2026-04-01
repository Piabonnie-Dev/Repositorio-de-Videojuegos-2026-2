using UnityEngine;

public class Jugador : Personaje
{
    [Header("Disparo")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;

    [HideInInspector]
    public bool enSuelo = false;

    private ControldeJugador control;
    private float puntoDisparoXInicial;
    private bool teniaPuntoDisparo = false;
    private Collider2D colJugador;

    private PlataformaMovil plataformaActual;

    protected override void Awake()
    {
        base.Awake();

        control = GetComponent<ControldeJugador>();
        colJugador = GetComponent<Collider2D>();

        if (puntoDisparo != null)
        {
            puntoDisparoXInicial = Mathf.Abs(puntoDisparo.localPosition.x);
            teniaPuntoDisparo = true;
        }
    }

    private void Update()
    {
        ActualizarLadoDisparo();

        if (Input.GetButtonDown("Fire1"))
        {
            Disparar();
        }
    }

    private void ActualizarLadoDisparo()
    {
        if (!teniaPuntoDisparo || puntoDisparo == null || sr == null)
            return;

        Vector3 local = puntoDisparo.localPosition;
        local.x = sr.flipX ? -puntoDisparoXInicial : puntoDisparoXInicial;
        puntoDisparo.localPosition = local;
    }

    private bool EsSueloOPlataforma(GameObject obj)
    {
        return obj.CompareTag("Suelo") || obj.CompareTag("Plataforma");
    }

    private bool EstaEncimaDe(Collision2D col)
    {
        if (colJugador == null || col.collider == null)
            return false;

        return colJugador.bounds.min.y >= col.collider.bounds.max.y - 0.15f;
    }

    public Vector2 ObtenerDeltaPlataforma()
    {
        if (plataformaActual != null && enSuelo)
            return plataformaActual.DeltaMovimiento;

        return Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!EsSueloOPlataforma(col.gameObject))
            return;

        if (EstaEncimaDe(col))
        {
            enSuelo = true;

            if (col.gameObject.CompareTag("Plataforma"))
            {
                plataformaActual = col.gameObject.GetComponent<PlataformaMovil>();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (!EsSueloOPlataforma(col.gameObject))
            return;

        if (EstaEncimaDe(col))
        {
            enSuelo = true;

            if (col.gameObject.CompareTag("Plataforma"))
            {
                plataformaActual = col.gameObject.GetComponent<PlataformaMovil>();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (EsSueloOPlataforma(col.gameObject))
        {
            enSuelo = false;

            if (col.gameObject.CompareTag("Plataforma"))
            {
                PlataformaMovil p = col.gameObject.GetComponent<PlataformaMovil>();
                if (plataformaActual == p)
                    plataformaActual = null;
            }
        }
    }

    private void Disparar()
    {
        if (prefabProyectil == null)
        {
            Debug.LogWarning("No asignaste el prefab del proyectil.", this);
            return;
        }

        if (puntoDisparo == null)
        {
            Debug.LogWarning("No asignaste el punto de disparo.", this);
            return;
        }

        Vector2 dir = (sr != null && sr.flipX) ? Vector2.left : Vector2.right;
        float velocidadJugador = (control != null) ? control.velocidadActual : 0f;

        GameObject p = Instantiate(prefabProyectil, puntoDisparo.position, Quaternion.identity);

        Proyectil proyectil = p.GetComponent<Proyectil>();
        if (proyectil != null)
        {
            proyectil.Inicializar(dir, velocidadJugador);
        }
    }
}
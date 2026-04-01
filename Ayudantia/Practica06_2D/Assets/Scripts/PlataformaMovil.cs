using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;

    private Transform objetivo;
    private Rigidbody2D rb;

    public Vector2 DeltaMovimiento { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        objetivo = puntoB;
    }

    private void FixedUpdate()
    {
        if (puntoA == null || puntoB == null || rb == null)
        {
            DeltaMovimiento = Vector2.zero;
            return;
        }

        Vector2 posicionActual = rb.position;

        Vector2 nuevaPos = Vector2.MoveTowards(
            posicionActual,
            objetivo.position,
            velocidad * Time.fixedDeltaTime
        );

        DeltaMovimiento = nuevaPos - posicionActual;

        rb.MovePosition(nuevaPos);

        if (Vector2.Distance(nuevaPos, objetivo.position) < 0.05f)
        {
            objetivo = (objetivo == puntoA) ? puntoB : puntoA;
        }
    }
}
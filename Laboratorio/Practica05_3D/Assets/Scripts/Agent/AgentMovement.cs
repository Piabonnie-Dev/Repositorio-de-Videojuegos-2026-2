using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    NavMeshAgent agent;
    [Tooltip("Radio en el que se elige un destino aleatorio desde la posición del NPC")]
    public float wanderRadius = 20f;
    [Tooltip("Distancia mínima para considerar que llegó al destino")]
    public float arriveThreshold = 0.5f;
    [Tooltip("Velocidad de rotación al girar hacia la dirección de movimiento")]
    public float turnSpeed = 5f;
    [Tooltip("Mostrar gizmos en la escena para ver el destino")]
    public bool showGizmos = true;

    // Guarda el destino actual — así el NPC "sabe" adónde va
    Vector3 currentDestination = Vector3.zero;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent no encontrado en " + gameObject.name);
            enabled = false;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning(gameObject.name + " no está sobre un NavMesh. ");
        }

        SetRandomDestination();
    }

    void Update()
    {
        if (agent == null) return;

        // Si no hay path pendiente y estamos cerca del destino, elegir otro
        if (!agent.pathPending && agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, arriveThreshold))
        {
            SetRandomDestination();
        }

        // Hacer que el NPC mire en la dirección de su velocidad (suavizado)
        Vector3 vel = agent.velocity;
        vel.y = 0f;
        if (vel.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(vel.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        }
    }

    // Selecciona un destino aleatorio válido sobre el NavMesh y lo almacena
    void SetRandomDestination()
    {
        Vector3 randomPos = transform.position + new Vector3(Random.Range(-wanderRadius, wanderRadius), 0f, Random.Range(-wanderRadius, wanderRadius));
        NavMeshHit hit;
        // Intentar muestrear una posición válida cerca del punto generado
        if (NavMesh.SamplePosition(randomPos, out hit, 2.0f, NavMesh.AllAreas))
        {
            currentDestination = hit.position;
            agent.SetDestination(currentDestination);
        }
        else
        {
            // Si no se encuentra un punto en el NavMesh, usar la posición calculada (fallback)
            currentDestination = randomPos;
            agent.SetDestination(currentDestination);
        }
    }

    // Método público para que otros sistemas pregunten el destino actual
    public Vector3 GetCurrentDestination()
    {
        return currentDestination;
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(currentDestination, 0.25f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.2f, currentDestination + Vector3.up * 0.2f);
    }
}

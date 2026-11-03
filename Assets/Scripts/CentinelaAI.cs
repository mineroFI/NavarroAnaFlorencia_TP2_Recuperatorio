using UnityEngine;
using UnityEngine.AI;

public class CentinelaAI : MonoBehaviour
{
    [Header("Configuración de Patrulla")]
    public Transform[] puntosPatrulla;
    private int indiceActual = 0;

    [Header("Detección y Persecución")]
    public Transform objetivoJugador;
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 1.8f;

    private NavMeshAgent agente;
    private bool persiguiendo = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();


        if (objetivoJugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                objetivoJugador = playerObj.transform;
            }
        }

        IrAlSiguientePunto();
    }

    void Update()
    {
        if (objetivoJugador == null) return;

        // Calculamos a cuantos metros esta Glorp del Centinela
        float distanciaAlJugador = Vector3.Distance(transform.position, objetivoJugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
        {
            //  glopr entra al area y el centinela lo persigue
            if (!persiguiendo)
            {
                persiguiendo = true;
                Debug.Log("[IA Centinela] ¡Jugador detectado! Iniciando persecución.");
            }

            agente.SetDestination(objetivoJugador.position);
        }
        else
        {
            // Si glorp se aleja fuera del rango, el centinela vuelve a su ruta de patrulla
            if (persiguiendo)
            {
                persiguiendo = false;
                Debug.Log("[IA Centinela] Jugador fuera de rango. Retomando patrulla.");
                IrAlSiguientePunto();
            }

            // Cuando llega a una esquina del camino, pasa a la siguiente esquina
            if (!agente.pathPending && agente.remainingDistance < 0.5f)
            {
                IrAlSiguientePunto();
            }
        }
    }

    void IrAlSiguientePunto()
    {
        if (puntosPatrulla.Length == 0) return;

        agente.SetDestination(puntosPatrulla[indiceActual].position);
        indiceActual = (indiceActual + 1) % puntosPatrulla.Length;
    }

    // circulo amarillo en la escena para ver el radio de vision del enemigo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
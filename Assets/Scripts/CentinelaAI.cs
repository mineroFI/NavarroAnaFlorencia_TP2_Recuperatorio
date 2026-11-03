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

    [Header("Sistema de Ataque y Cooldown")]
    public int danioAtaque = 20;
    public float tiempoEntreAtaques = 1.5f; // Espera 1.5s entre cada golpe
    private float proximoAtaquePermitido = 0f;

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

        // Si el juego terminó o está en pausa, el fantasma no ataca
        if (GameManager.Instancia != null && GameManager.Instancia.estadoActual != EstadoJuego.Playing)
        {
            return;
        }

        float distanciaAlJugador = Vector3.Distance(transform.position, objetivoJugador.position);

        if (distanciaAlJugador <= rangoDeteccion)
        {
            if (!persiguiendo)
            {
                persiguiendo = true;
                Debug.Log("[IA Centinela] ¡Jugador detectado! Iniciando persecución.");
            }

            agente.SetDestination(objetivoJugador.position);

            // Si está lo suficientemente cerca y ya pasó el tiempo de espera, ataca
            if (distanciaAlJugador <= rangoAtaque && Time.time >= proximoAtaquePermitido)
            {
                AtacarJugador();
            }
        }
        else
        {
            if (persiguiendo)
            {
                persiguiendo = false;
                Debug.Log("[IA Centinela] Jugador fuera de rango. Retomando patrulla.");
                IrAlSiguientePunto();
            }

            if (!agente.pathPending && agente.remainingDistance < 0.5f)
            {
                IrAlSiguientePunto();
            }
        }
    }

    void AtacarJugador()
    {
        proximoAtaquePermitido = Time.time + tiempoEntreAtaques;

        if (GameManager.Instancia != null)
        {
            GameManager.Instancia.RecibirDanio(danioAtaque);
        }
        else
        {
            Debug.Log("[IA Centinela] ¡Ataque al jugador! -" + danioAtaque + " HP.");
        }
    }

    void IrAlSiguientePunto()
    {
        if (puntosPatrulla.Length == 0) return;

        agente.SetDestination(puntosPatrulla[indiceActual].position);
        indiceActual = (indiceActual + 1) % puntosPatrulla.Length;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
using UnityEngine;
using UnityEngine.AI;

public class CentinelaAI : MonoBehaviour
{
    [Header("Configuración de Patrulla")]
    public Transform[] puntosPatrulla;
    private int indiceActual = 0;

    [Header("Detección y Persecución")]
    public Transform objetivoJugador;
    private PlayerController scriptJugador;
    public float rangoDeteccion = 10f;
    public float rangoAtaque = 1.8f;

    [Header("Sistema de Ataque y Cooldown")]
    public int danioAtaque = 20;
    public float tiempoEntreAtaques = 1.5f;
    private float proximoAtaquePermitido = 0f;

    [Header("Opción A: Estado de Aturdimiento")]
    private bool estaAturdido = false;
    private float tiempoFinAturdimiento = 0f;

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

        if (objetivoJugador != null)
        {
            scriptJugador = objetivoJugador.GetComponent<PlayerController>();
        }

        IrAlSiguientePunto();
    }

    void Update()
    {
        if (objetivoJugador == null) return;

        if (GameManager.Instancia != null && GameManager.Instancia.estadoActual != EstadoJuego.Playing)
        {
            return;
        }

        // Si el fantasma fue alcanzado por el Raycast aturdidor, se congela
        if (estaAturdido)
        {
            if (Time.time >= tiempoFinAturdimiento)
            {
                estaAturdido = false;
                agente.isStopped = false;
                Debug.Log("[IA Centinela] El enemigo se recuperó del aturdimiento y vuelve a moverse.");
            }
            else
            {
                return; // Mientras está aturdido no camina ni ataca
            }
        }

        // Verifica si Glorp está invisible
        bool jugadorInvisible = (scriptJugador != null && scriptJugador.esInvisible);

        float distanciaAlJugador = Vector3.Distance(transform.position, objetivoJugador.position);

        if (distanciaAlJugador <= rangoDeteccion && !jugadorInvisible)
        {
            if (!persiguiendo)
            {
                persiguiendo = true;
                Debug.Log("[IA Centinela] ¡Jugador detectado! Iniciando persecución.");
            }

            agente.SetDestination(objetivoJugador.position);

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
                Debug.Log("[IA Centinela] Jugador fuera de rango o invisible. Retomando patrulla.");
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
    }

    // Método llamado por el arma Raycast de Glorp
    public void Aturdir(float segundos)
    {
        estaAturdido = true;
        tiempoFinAturdimiento = Time.time + segundos;
        agente.isStopped = true;
        agente.velocity = Vector3.zero; // Frena en seco cualquier inercia
        Debug.Log($"[Arma Raycast] Enemigo aturdido y paralizado por {segundos} segundos!");
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
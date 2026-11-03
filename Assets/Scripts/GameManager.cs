using UnityEngine;

public enum EstadoJuego
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Estado Actual del Juego")]
    public EstadoJuego estadoActual = EstadoJuego.Start;

    [Header("Salud del Jugador")]
    public int saludMaxima = 100;
    public int saludActual = 100;

    [Header("Puntuación y Meta de Victoria")]
    public int puntajeActual = 0;
    public int puntosParaGanar = 50; // Si cada item da 10, al juntar 5 items ganas

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        saludActual = saludMaxima;
        CambiarEstado(EstadoJuego.Start);
        // Pasamos automáticamente al estado jugando al iniciar la partida
        CambiarEstado(EstadoJuego.Playing);
    }

    private void Update()
    {
        // Pausar o reanudar con la tecla P o Escape
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (estadoActual == EstadoJuego.Playing)
            {
                CambiarEstado(EstadoJuego.Paused);
            }
            else if (estadoActual == EstadoJuego.Paused)
            {
                CambiarEstado(EstadoJuego.Playing);
            }
        }
    }

    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        estadoActual = nuevoEstado;

        switch (estadoActual)
        {
            case EstadoJuego.Start:
                Time.timeScale = 1f;
                Debug.Log($"[GameManager] Estado: START | Vida: {saludActual}/{saludMaxima} | Puntaje: {puntajeActual}/{puntosParaGanar}");
                break;

            case EstadoJuego.Playing:
                Time.timeScale = 1f;
                Debug.Log($"[GameManager] Estado: PLAYING (Jugando) | Vida: {saludActual}/{saludMaxima} | Puntaje: {puntajeActual}/{puntosParaGanar}");
                break;

            case EstadoJuego.Paused:
                Time.timeScale = 0f; // Congela el tiempo del juego
                Debug.Log($"[GameManager] Estado: PAUSED (Juego en Pausa) | Vida: {saludActual}/{saludMaxima} | Puntaje: {puntajeActual}/{puntosParaGanar}");
                break;

            case EstadoJuego.Victory:
                Time.timeScale = 0f;
                Debug.Log($"[GameManager] ¡VICTORIA! Recolectaste todos los datos. | Vida final: {saludActual} | Puntaje final: {puntajeActual}");
                break;

            case EstadoJuego.GameOver:
                Time.timeScale = 0f;
                Debug.Log($"[GameManager] GAME OVER. Glorp se quedó sin salud. | Puntaje alcanzado: {puntajeActual}");
                break;
        }
    }

    public void SumarPuntos(int cantidad)
    {
        if (estadoActual != EstadoJuego.Playing) return;

        puntajeActual += cantidad;
        Debug.Log($"[GameManager] ¡Ítem recolectado! (+{cantidad} pts) | Puntaje actual: {puntajeActual}/{puntosParaGanar} | Vida actual: {saludActual}");

        if (puntajeActual >= puntosParaGanar)
        {
            CambiarEstado(EstadoJuego.Victory);
        }
    }

    public void RecibirDanio(int danio)
    {
        if (estadoActual != EstadoJuego.Playing) return;

        saludActual -= danio;
        if (saludActual < 0) saludActual = 0;

        Debug.Log($"[GameManager] ¡Glorp recibió {danio} de daño! | Vida actual: {saludActual}/{saludMaxima} | Puntaje: {puntajeActual}");

        if (saludActual <= 0)
        {
            CambiarEstado(EstadoJuego.GameOver);
        }
    }

    public bool CurarJugador(int cantidadCuracion)
    {
        if (estadoActual != EstadoJuego.Playing) return false;
        if (saludActual >= saludMaxima) return false;

        saludActual += cantidadCuracion;
        if (saludActual > saludMaxima) saludActual = saludMaxima;

        Debug.Log($"[GameManager] +Curación recibida (+{cantidadCuracion} HP) | Vida actual: {saludActual}/{saludMaxima} | Puntaje: {puntajeActual}");
        return true;
    }
}
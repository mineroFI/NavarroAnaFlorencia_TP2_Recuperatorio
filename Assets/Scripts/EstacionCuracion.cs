using UnityEngine;
using System.Collections;

public class EstacionCuracion : MonoBehaviour
{
    [Header("Configuración de Curación")]
    public int puntosPorTick = 10;
    public float intervaloCuracion = 3f;
    public int maximoCuracionTotal = 30;
    private int curacionOtorgadaActual = 0;

    [Header("Cooldown y Estado")]
    public float tiempoEnfriamiento = 8f;
    private bool estaEnRecarga = false;
    private bool jugadorEnZona = false;

    [Header("Retroalimentación Visual (Opcional)")]
    public Light luzEstacion;
    public Renderer renderEstacion;
    public Material materialActivo;
    public Material materialRecarga;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = true;
            Debug.Log("[Estación de Curación] Glorp entró a la zona de curación.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnZona = false;
            Debug.Log("[Estación de Curación] Glorp salió de la zona de curación.");
        }
    }

    private void Start()
    {
        StartCoroutine(BucleEstacion());
    }

    IEnumerator BucleEstacion()
    {
        while (true)
        {
            // Si el jugador está dentro y la estación no está recargando
            if (jugadorEnZona && !estaEnRecarga)
            {
                if (GameManager.Instancia != null)
                {
                    bool seCuro = GameManager.Instancia.CurarJugador(puntosPorTick);
                    if (seCuro)
                    {
                        curacionOtorgadaActual += puntosPorTick;
                        Debug.Log($"[Estación de Curación] Curando +{puntosPorTick} HP. Total otorgado: {curacionOtorgadaActual}/{maximoCuracionTotal}");

                        // Si alcanzó el límite de carga, pausa el ciclo y empieza el cooldown
                        if (curacionOtorgadaActual >= maximoCuracionTotal)
                        {
                            yield return StartCoroutine(RutinaCooldown());
                        }
                    }
                    else
                    {
                        Debug.Log("[Estación de Curación] Glorp ya tiene la salud al máximo.");
                    }
                }

                yield return new WaitForSeconds(intervaloCuracion);
            }
            else
            {
                yield return null; // Espera al siguiente frame si no hay jugador o esta en recarga
            }
        }
    }

    IEnumerator RutinaCooldown()
    {
        estaEnRecarga = true;
        curacionOtorgadaActual = 0;
        ActualizarVisuales(false);
        Debug.Log("[Estación de Curación] ¡Estación agotada! Entrando en período de recarga...");

        yield return new WaitForSeconds(tiempoEnfriamiento);

        estaEnRecarga = false;
        ActualizarVisuales(true);
        Debug.Log("[Estación de Curación] ¡Estación recargada y lista para usarse nuevamente!");
    }

    void ActualizarVisuales(bool activa)
    {
        if (luzEstacion != null)
        {
            luzEstacion.enabled = activa;
        }

        if (renderEstacion != null && materialActivo != null && materialRecarga != null)
        {
            renderEstacion.material = activa ? materialActivo : materialRecarga;
        }
    }
}
using UnityEngine;

public class ZonaBarro : MonoBehaviour
{
    [Header("Configuración de Desaceleración")]
    public float velocidadEnBarro = 2.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController jugador = other.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.CambiarVelocidad(velocidadEnBarro);
                Debug.Log("[Zona de Barro] Glorp entró al barro. Velocidad reducida a " + velocidadEnBarro);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController jugador = other.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.RestaurarVelocidad();
                Debug.Log("[Zona de Barro] Glorp salió del barro. Velocidad normal restaurada.");
            }
        }
    }
}
using UnityEngine;

public class PowerUpInvisibilidad : MonoBehaviour
{
    [Header("Configuración del Power-Up")]
    public float duracionInvisibilidad = 5f;
    public float velocidadGiro = 90f;

    void Update()
    {
        
        transform.Rotate(0f, velocidadGiro * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController jugador = other.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.ActivarInvisibilidad(duracionInvisibilidad);
                Destroy(gameObject);
            }
        }
    }
}
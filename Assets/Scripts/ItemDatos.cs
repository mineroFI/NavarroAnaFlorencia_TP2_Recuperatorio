using UnityEngine;

public class ItemDatos : MonoBehaviour
{
    [Header("Configuración del Ítem")]
    public int puntosQueOtorga = 10;
    public float velocidadGiro = 90f;

    void Update()
    {
        
        transform.Rotate(0f, velocidadGiro * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instancia != null)
            {
                GameManager.Instancia.SumarPuntos(puntosQueOtorga);
            }
            else
            {
                Debug.Log("[Ítem Recolectado] +" + puntosQueOtorga + " puntos de datos.");
            }

            Destroy(gameObject);
        }
    }
}
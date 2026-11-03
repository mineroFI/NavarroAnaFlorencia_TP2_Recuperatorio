using UnityEngine;

public class ItemCuracion : MonoBehaviour
{
    [Header("Configuración de Curación")]
    public int puntosDeSalud = 25;
    public float velocidadGiro = 90f;

    void Update()
    {
        
        transform.Rotate(0f, velocidadGiro * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instancia != null)
        {
            // Solo se consume si a Glorp le falta vida
            bool seCuro = GameManager.Instancia.CurarJugador(puntosDeSalud);
            if (seCuro)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("[Curación] Glorp ya tiene la salud al máximo (100/100). No se consume el pescadito.");
            }
        }
    }
}
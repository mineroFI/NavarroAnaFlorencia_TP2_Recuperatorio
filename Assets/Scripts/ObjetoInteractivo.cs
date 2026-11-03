using UnityEngine;

public class ObjetoInteractivo : MonoBehaviour
{
    [Header("Objeto que se va a abrir o desactivar")]
    public GameObject puertaAsociada;
    private bool activado = false;

    public void Interactuar()
    {
        if (!activado)
        {
            activado = true;
            Debug.Log("[Interacción] Consola activada por Raycast");

            if (puertaAsociada != null)
            {
                puertaAsociada.SetActive(false);
            }
        }
        else
        {
            Debug.Log("[Interacción] Esta consola ya fue activada.");
        }
    }
}
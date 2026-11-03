using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento y Salto")]
    public float velocidadMovimiento = 6f;
    public float velocidadGiro = 120f;
    public float fuerzaSalto = 6f;

    [Header("1. Raycast Chequeo de Suelo")]
    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;
    private bool enSuelo;

    [Header("2. Raycast Interacción Frontal")]
    public float distanciaInteraccion = 4f;
    public LayerMask capaInteractivo;
    public float alturaRayoFrontal = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        enSuelo = Physics.Raycast(transform.position, Vector3.down, distanciaSuelo, capaSuelo);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }

        
        Vector3 origenFrontal = transform.position + Vector3.up * alturaRayoFrontal;
        Vector3 frenteReal = -transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origenFrontal, frenteReal, out hit, distanciaInteraccion, capaInteractivo))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ObjetoInteractivo interactivo = hit.collider.GetComponent<ObjetoInteractivo>();
                if (interactivo != null)
                {
                    interactivo.Interactuar();
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Si el juego no está en Playing, frenamos el movimiento físico
        if (GameManager.Instancia != null && GameManager.Instancia.estadoActual != EstadoJuego.Playing)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float avanzar = Input.GetAxis("Vertical");
        float girar = Input.GetAxis("Horizontal");

        transform.Rotate(0f, girar * velocidadGiro * Time.fixedDeltaTime, 0f);

        Vector3 direccion = (-transform.forward) * avanzar * velocidadMovimiento;
        rb.linearVelocity = new Vector3(direccion.x, rb.linearVelocity.y, direccion.z);
    }

    // lineas en la vista Scene para poder verlos y calibrar
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanciaSuelo);

        Gizmos.color = Color.red;
        Vector3 origenFrontal = transform.position + Vector3.up * alturaRayoFrontal;
        Vector3 frenteReal = -transform.forward;
        Gizmos.DrawLine(origenFrontal, origenFrontal + frenteReal * distanciaInteraccion);
    }
}
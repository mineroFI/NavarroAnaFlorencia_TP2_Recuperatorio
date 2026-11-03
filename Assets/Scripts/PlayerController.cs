using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento y Salto")]
    public float velocidadMovimiento = 6f;
    private float velocidadOriginal;
    public float velocidadGiro = 120f;
    public float fuerzaSalto = 6f;

    [Header("1. Raycast Chequeo de Suelo")]
    public float distanciaSuelo = 1.2f;
    public LayerMask capaSuelo;
    private bool enSuelo;

    [Header("2. Raycast Interacción Frontal (Tecla E)")]
    public float distanciaInteraccion = 4f;
    public LayerMask capaInteractivo;
    public float alturaRayoFrontal = 0.5f;

    [Header("Opción A: Arma Aturdidora (Tecla F o Clic Izq)")]
    public float distanciaArma = 8f;
    public float segundosAturdimiento = 4f;
    public int usosRestantesArma = 3; 
    public float cooldownArma = 6f;  
    private float proximoDisparoPermitido = 0f;

    [Header("Opción B: Estado de Invisibilidad")]
    public bool esInvisible = false;
    private float tiempoFinInvisibilidad = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocidadOriginal = velocidadMovimiento;
    }

    public void CambiarVelocidad(float nuevaVelocidad)
    {
        velocidadMovimiento = nuevaVelocidad;
    }

    public void RestaurarVelocidad()
    {
        velocidadMovimiento = velocidadOriginal;
    }

    public void ActivarInvisibilidad(float duracion)
    {
        esInvisible = true;
        tiempoFinInvisibilidad = Time.time + duracion;
        Debug.Log($"[Power-Up] ¡Glorp es INVISIBLE ante los enemigos por {duracion} segundos!");
    }

    void Update()
    {
        // Si el juego está en Pausa, Victoria o GameOver, Glorp no ejecuta acciones
        if (GameManager.Instancia != null && GameManager.Instancia.estadoActual != EstadoJuego.Playing)
        {
            return;
        }

        // Control del tiempo de Invisibilidad 
        if (esInvisible && Time.time >= tiempoFinInvisibilidad)
        {
            esInvisible = false;
            Debug.Log("[Power-Up] El efecto de invisibilidad terminó. Glorp vuelve a ser visible.");
        }

        // Rayo hacia abajo para el Salto
        enSuelo = Physics.Raycast(transform.position, Vector3.down, distanciaSuelo, capaSuelo);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }

        // Dirección frontal de Glorp
        Vector3 origenFrontal = transform.position + Vector3.up * alturaRayoFrontal;
        Vector3 frenteReal = -transform.forward;
        RaycastHit hit;

        // Rayo frontal para interactuar con la Consola (Tecla E)
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

        // Disparo de Raycast aturdidor con loimite de usos y cooldown (Tecla F o Clic Izq)
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            IntentarDispararArma(origenFrontal, frenteReal);
        }
    }

    void IntentarDispararArma(Vector3 origen, Vector3 direccion)
    {
        // 1. Verificamos si todavía le quedan cargas
        if (usosRestantesArma <= 0)
        {
            Debug.Log("[Arma Raycast] ¡Sin batería! Ya no te quedan cargas de aturdimiento (0 restantes).");
            return;
        }

        // Cooldown
        if (Time.time < proximoDisparoPermitido)
        {
            float segundosFaltantes = proximoDisparoPermitido - Time.time;
            Debug.Log($"[Arma Raycast] Recargando energía... Espera {segundosFaltantes:F1} segundos.");
            return;
        }

        // Lanzamos el Raycast frontal
        RaycastHit hitArma;
        if (Physics.Raycast(origen, direccion, out hitArma, distanciaArma))
        {
            CentinelaAI enemigo = hitArma.collider.GetComponentInParent<CentinelaAI>();
            if (enemigo != null)
            {
                usosRestantesArma--;
                proximoDisparoPermitido = Time.time + cooldownArma;

                enemigo.Aturdir(segundosAturdimiento);
                Debug.Log($"[Arma Raycast] Impacto exitoso. Cargas restantes: {usosRestantesArma}");
            }
            else
            {
                Debug.Log("[Arma Raycast] El disparo chocó contra un obstáculo, no contra el enemigo.");
            }
        }
        else
        {
            Debug.Log("[Arma Raycast] No hay ningún enemigo en el alcance del rayo.");
        }
    }

    void FixedUpdate()
    {
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

    void OnDrawGizmos()
    {
        // chequeo de suelo
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * distanciaSuelo);

        Vector3 origenFrontal = transform.position + Vector3.up * alturaRayoFrontal;
        Vector3 frenteReal = -transform.forward;

        // alcance del Arma Aturdidora 
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origenFrontal, origenFrontal + frenteReal * distanciaArma);

        // alcance de interacción con Consola 
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origenFrontal, origenFrontal + frenteReal * distanciaInteraccion);
    }
}
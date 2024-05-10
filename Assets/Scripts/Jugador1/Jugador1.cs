
using Assets.Scripts.Stats;
using UnityEngine;
using UnityEngine.UI;

public class Scripts : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animator;
    private Vector2 input;
    private PlayerStats stats;


    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavizadoMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    public float fuerzaDeSalto;
    public LayerMask queEsSuelo;
    public Transform controladorSuelo;
    public Vector3 dimencionesCaja;
    public bool enSuelo;
    private bool salto = false;
    private bool dobleSaltoDisponible = false;

    [Header("Escalar")]
    public float velocidadEscalar;
    private BoxCollider2D boxCollider2D;
    private float gravedadInicial;
    private bool escalando;

    [Header("SaltoPared")]
    public Transform controladorPared;
    public Vector3 dimensionesCajaPared;
    public float velocidadDeslisamiento;
    private bool enPared;
    private bool deslizando;
    [Header("BarraDeVida")]
    public Image barraDeVida;
    private float vidaActual;





    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        stats = GetComponent<PlayerStats>();
        gravedadInicial = rb2D.gravityScale;

        vidaActual = stats.Health;
    }

    void Update()
    {
        barraDeVida.fillAmount = vidaActual/stats.maxHealth;
        
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        

        movimientoHorizontal = input.x * velocidadDeMovimiento;
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animator.SetBool("Escalar", escalando);
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo || dobleSaltoDisponible)
            {
                if (!enSuelo)
                {
                    dobleSaltoDisponible = false;
                }
                salto = true;
            }
        }
    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimencionesCaja, 0f, queEsSuelo);
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, queEsSuelo);
        Escalar();

        //Move
        Move(movimientoHorizontal * Time.deltaTime, salto);
        salto = false;

        if (deslizando)
        {

        }
    }

    private void Move(float mover, bool saltar)
    {
        Vector3 velocidadObjetivo = new Vector2(mover, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, velocidadObjetivo, ref velocidad, suavizadoMovimiento);

        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        if (enSuelo && !dobleSaltoDisponible)
        {
            dobleSaltoDisponible = true;
        }

        if (saltar)
        {
            rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
            if (!enSuelo)
            {
                dobleSaltoDisponible = false;
            }
        }

        animator.SetBool("a_jump", !enSuelo); 
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimencionesCaja);
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }
    private void Escalar()
    {
        if (enPared)
        {
            escalando = true;
            Vector2 veloscidadSubida = new Vector2(rb2D.velocity.x, input.y * velocidadEscalar);
            rb2D.velocity = veloscidadSubida;
            rb2D.gravityScale = 0;
        }
        else
        {
            rb2D.gravityScale = gravedadInicial;
            escalando = false;
        }

        if (enSuelo)
        {

            escalando = false;
        }
    }
}

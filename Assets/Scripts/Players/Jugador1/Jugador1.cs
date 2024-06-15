using System;
using Assets.Scripts.Stats;
using UnityEngine;
using UnityEngine.UI;

public class FirstPlayerController : MonoBehaviour
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
    public float vidaActual;

    private Vector2 direccionMirando = Vector2.right;

    [Header("Aumento de Vida")]
    public float aumentoMaximo = 0.2f; // Porcentaje de aumento de la vida máxima

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        stats = GetComponent<PlayerStats>();
        gravedadInicial = rb2D.gravityScale;
        vidaActual = stats.maxHealth;
        barraDeVida.fillAmount = stats.maxHealth;
    }

    void Update()
    {
        vidaActual = stats.Health;
        StartCoroutine(Routines.WaitAndExecute(0.2f, () => barraDeVida = UpdateHealth(barraDeVida, vidaActual, stats.maxHealth)));

        input.x = Input.GetAxisRaw("Horizontal1");
        input.y = Input.GetAxisRaw("Vertical1");

        if (Input.GetKeyDown(KeyCode.W) && (enSuelo || dobleSaltoDisponible))
        {
            if (!enSuelo && dobleSaltoDisponible)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
                rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
                dobleSaltoDisponible = false;
            }
            else
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
                rb2D.AddForce(new Vector2(0f, fuerzaDeSalto));
            }
        }
        else
        {
            movimientoHorizontal = input.x * velocidadDeMovimiento;
        }

        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animator.SetBool("Escalar", escalando);
    }

    private void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimencionesCaja, 0f, queEsSuelo);
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, queEsSuelo);
        Escalar();
        Move(movimientoHorizontal);
    }

    private void Move(float mover)
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

        animator.SetBool("a_jump", !enSuelo);
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("pill"))
        {
            AumentarVida();
            Destroy(other.gameObject); // Destruir la pastilla
        }
        if (other.CompareTag("att"))
        {
            AumentarAtaque();
            Destroy(other.gameObject); // Destruir la pastilla
        }
        if (other.CompareTag("def"))
        {
            AumentarEscudo();
            Destroy(other.gameObject); // Destruir la pastilla
        }
    }

    public void AumentarVida()
    {
        if (stats.Health >= stats.maxHealth)
        {
            float aumentoVida = stats.maxHealth * aumentoMaximo;
            stats.maxHealth += aumentoVida;
            stats.Health += aumentoVida;
        }
        else
        {
            stats.maxHealth += stats.maxHealth * aumentoMaximo;
            stats.Health += stats.maxHealth * aumentoMaximo;
        }

        if (stats.Health > stats.maxHealth)
        {
            stats.Health = stats.maxHealth;
        }
    }

    public void AumentarEscudo(){
        stats.Defense += Mathf.RoundToInt(stats.Defense * aumentoMaximo);
    }

    public void AumentarAtaque(){
        stats.Attack += stats.Attack * aumentoMaximo;
    }           

    Image UpdateHealth(Image healthBar, float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        return healthBar;
    }
}

using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Stats;
using UnityEngine.UI;

public class SecondPlayerController : MonoBehaviour {

    [SerializeField] float          m_speed = 4.0f;
    [SerializeField] float          m_jumpForce = 7.5f;
    [SerializeField] float          m_rollForce = 6.0f;
    [SerializeField] bool           m_noBlood = false;
    [SerializeField] GameObject     m_slideDust;
    [SerializeField] Transform      attackSensor1;
    [SerializeField] Transform      attackSensor2;
    [SerializeField] Transform      attackSensor3;
    [SerializeField] Vector3        boxAtackColliderDimensions;
    [SerializeField] private Image      barraDeVida;


    //Not initialized variables
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    //private BoxCollider2D       m_boxCollider;
    //private DamageTargetStats   m_damageTargetStats;
    private Sensor_HeroKnight   m_groundSensorR1;
    private Sensor_HeroKnight   m_groundSensorL1;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private float               attack1SensorDistance;
    private float               attack2SensorDistance;
    private float               attack3SensorDistance;

    //Initialized variables
    private bool                doubleJump = true;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    //private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    //private int                 m_currentAttack = 0;
    //private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    //private float               m_rollDuration = 8.0f / 14.0f;
    //private float               m_rollCurrentTime;
    private readonly string     horizontalMovement = "Horizontal2";
    private readonly string     verticalMovement= "Vertical2";
    private readonly string     jump = "Jump2";
    private PlayerStats         stats;
    private float               vidaActual;



    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>(); 
    }

    // Use this for initialization
    void Start ()
    {
        //m_damageTargetStats = GetComponent<DamageTargetStats>();

        m_groundSensorR1 = transform.Find("GroundSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_groundSensorL1 = transform.Find("GroundSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        stats = transform.GetComponent<PlayerStats>();
        
        vidaActual = stats.maxHealth;
        barraDeVida.fillAmount = stats.maxHealth;



        attack1SensorDistance = Math.Abs(Math.Abs(transform.position.x) - Math.Abs(attackSensor1.position.x));
        attack2SensorDistance = Math.Abs(Math.Abs(transform.position.x) - Math.Abs(attackSensor3.position.x));
        attack3SensorDistance = Math.Abs(Math.Abs(transform.position.x) - Math.Abs(attackSensor3.position.x));


    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls horizontalAttack combo
        //m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        //if (m_rolling)
        //    m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        //if (m_rollCurrentTime > m_rollDuration)
        //    m_rolling = false;
        vidaActual = stats.Health;
        barraDeVida.fillAmount = vidaActual / stats.maxHealth;


        validateGrounded(m_grounded, m_groundSensorR1.State(), m_groundSensorL1.State());

        float inputX = Input.GetAxis(horizontalMovement);

        swapSpriteDirection(inputX);

        // Move
        //if (!m_rolling)
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat(SecondPlayerAnimations.airSpeedY, m_body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool(SecondPlayerAnimations.wallSlide, m_isWallSliding);

        //Death
        Death();

        //Player actions like jump, horizontalAttack, block, etc.
        PlayerActions(inputX);
    }

    private void PlayerActions(float inputX)
    {
        //Attack
        //if (Input.GetButtonDown(horizontalAttack) && m_timeSinceAttack > 0.25f /*&& !m_rolling*/)
        //{
        //    m_currentAttack++;
        //
        //    // Loop back to one after third horizontalAttack
        //    if (m_currentAttack > 3)
        //        m_currentAttack = 1;
        //
        //    // Reset Attack combo if time since last horizontalAttack is too large
        //    if (m_timeSinceAttack > 1.0f)
        //        m_currentAttack = 1;
        //
        //    // Call one of three horizontalAttack animations "Attack1", "Attack2", "Attack3"
        //    m_animator.SetTrigger(SecondPlayerAnimations.attack + m_currentAttack);
        //
        //    m_damageTargetStats.Attack();
        //
        //    // Reset timer
        //    m_timeSinceAttack = 0.0f;
        //}

        // Block
        if (Input.GetKeyDown(KeyCode.J) && !Input.GetKeyDown(KeyCode.N) /*&& !m_rolling*/)
        {
            m_animator.SetTrigger(SecondPlayerAnimations.block);
            m_animator.SetBool(SecondPlayerAnimations.idleBlock, true);
        }

        else if (Input.GetKeyDown(KeyCode.J) && !Input.GetKeyDown(KeyCode.N))
            m_animator.SetBool(SecondPlayerAnimations.idleBlock, false);

        // Roll
        //else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        //{
        //    m_rolling = true;
        //    m_animator.SetTrigger(SecondPlayerAnimations.roll);
        //    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        //}


        //Jump
        else if (Input.GetButtonDown(jump) && m_grounded /*&& !m_rolling*/)
        {
            m_animator.SetTrigger(SecondPlayerAnimations.jump);
            m_grounded = false;
            m_animator.SetBool(SecondPlayerAnimations.grounded, m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensorR1.Disable(0.2f);
            m_groundSensorL1.Disable(0.2f);
        }
        else if (Input.GetButtonUp(jump) && !m_grounded && doubleJump/* && !m_rolling*/)
        {
            m_animator.SetTrigger(SecondPlayerAnimations.jump);
            doubleJump = false;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce * 0.5f);
            m_groundSensorR1.Disable(0.2f);
            m_groundSensorL1.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger(SecondPlayerAnimations.animState, 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger(SecondPlayerAnimations.animState, 0);
        }
    }

    private void Death()
    {
        //m_animator.SetBool(HeroKnightAnimations.noBlood, m_noBlood);
        //m_animator.SetTrigger(HeroKnightAnimations.death);
    }

    public void TakeDamage()
    {
        m_animator.SetTrigger(SecondPlayerAnimations.hurt);
    }

    private void swapSpriteDirection(float inputX)
    {
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
            attackSensor1.position = new Vector3(transform.position.x + attack1SensorDistance, attackSensor1.position.y, attackSensor1.position.z);
            attackSensor2.position = new Vector3(transform.position.x + attack2SensorDistance, attackSensor2.position.y, attackSensor2.position.z);
            attackSensor3.position = new Vector3(transform.position.x + attack3SensorDistance, attackSensor3.position.y, attackSensor3.position.z);

        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
            attackSensor1.position = new Vector3(transform.position.x - attack1SensorDistance, attackSensor1.position.y, attackSensor1.position.z);
            attackSensor2.position = new Vector3(transform.position.x - attack2SensorDistance, attackSensor2.position.y, attackSensor2.position.z);
            attackSensor3.position = new Vector3(transform.position.x - attack3SensorDistance, attackSensor3.position.y, attackSensor3.position.z);
        }
    }


    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    void validateGrounded(bool isGrounded, bool groundSensorR1State, bool groundSensorL1State)
    {

        //Check if character just landed on the ground
        if (!isGrounded && groundSensorR1State || groundSensorL1State)
        {
            m_grounded = true;
            doubleJump = true;
            m_animator.SetBool(SecondPlayerAnimations.grounded, isGrounded);
        }

        //Check if character just started falling
        if (isGrounded && !groundSensorR1State || !groundSensorL1State)
        {
            m_grounded = false;
            m_animator.SetBool(SecondPlayerAnimations.grounded, isGrounded);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackSensor1.position, boxAtackColliderDimensions);
    }
}

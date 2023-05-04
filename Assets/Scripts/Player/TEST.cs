using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public GameObject lanza;

    public Transform spawner;
    public GameObject bulletPrefab;
    private float horizontal;
    private float speed = 4f;
    private float jumpForce = 5f;
    private bool isFacingRight = true;

    public bool isGrounded;
    private bool canDoubleJump;

    //Variable para detener al jugador
    public bool stopInput;

    //Referencia al PauseMenu
    public PauseMenu reference;

    public float knockBackLength, knockBackForce; //Valor que tendrá el contador de KnockBack, y la fuerza de KnockBack
    private float knockBackCounter;

    //Planeo
    public float slideForce;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 13f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = .5f;

    public float cooldownTime = 2;
    private float nextFireTime = 0;

    //Gancho
    public float swingForce = 4f;
    //public bool isSwinging;
    public Vector2 ropeHook;

    [SerializeField] private Rigidbody2D theRB;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer theSR;
    [SerializeField] private Animator animator;

    public static TEST sharedInstance;
    private float jumpInput;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        nextFireTime = 2;
        //Debug.Log(nextFireTime);
    }
    void Update()
    {

        //jumpInput = Input.GetAxis("Jump");

        if (nextFireTime <= 0)
        {
           
            if (Input.GetKeyDown(KeyCode.Q))
            {

                GameObject bullet = Instantiate(bulletPrefab, spawner.position, bulletPrefab.transform.rotation);

                nextFireTime = 2;
                Destroy(bullet, 2f);

            }

        }
        else
            nextFireTime -= Time.deltaTime;

        //Si el juego está pausado, no funciona el movimiento. Tampoco si el jugador está parado
        if (!reference.isPaused && !stopInput)
        {
            //Si el contador de KnockBack se ha vaciado, el jugador recupera el control del movimiento
            if (knockBackCounter <= 0)
            {
                

                isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

                if (isDashing)
                {
                    return;
                }

                horizontal = Input.GetAxisRaw("Horizontal");

                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded)
                    {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        canDoubleJump = true;
                        AudioManager.sharedInstance.PlaySFX(0);
                    }
                    else
                    {
                        if (canDoubleJump)
                        {
                            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                            canDoubleJump = false;
                            AudioManager.sharedInstance.PlaySFX(0);
                        }
                    }
                }

                if (Input.GetButtonUp("Jump") && theRB.velocity.y > 0f)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * 0.5f);
                }

                if (Input.GetButton("Dash") && canDash)
                {
                    StartCoroutine(Dash());
                }

                Flip();

                //Si se pulsa el boton de Slide
                if (Input.GetButton("Slide"))
                {
                    //Slide
                    theRB.velocity = new Vector2(theRB.velocity.x, slideForce);
                }
            }
            //Si el contador de KnockBack todavía no está vacío
            else
            {
                //Hacemos decrecer el contador en 1 cada segundo
                knockBackCounter -= Time.deltaTime;
                //Si el jugador mira a la izquierda
                if (!theSR.flipX)
                {
                    //Aplicamos un pequeño empuje a la derecha
                    theRB.velocity = new Vector2(knockBackForce, theRB.velocity.y);
                }
                //Si el jugador mira a la derecha
                else
                {
                    //Aplicamos un pequeño empuje a la izquierda
                    theRB.velocity = new Vector2(-knockBackForce, theRB.velocity.y);
                }
            }
        }
    }

    

    private void FixedUpdate()
    {
        if (horizontal < 0f || horizontal > 0f)
        {
            ////animator.SetFloat("Speed", Mathf.Abs(horizontal));
            ////theSR.flipX = horizontal < 0f;
            //if (isSwinging)
            //{
            //    //animator.SetBool("IsSwinging", true);

            //    // Get normalized direction vector from player to the hook point
            //    //var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

            //    // Inverse the direction to get a perpendicular direction
            //    //Vector2 perpendicularDirection;
            //    //if (horizontal < 0)
            //    //{
            //    //    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
            //    //    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
            //    //    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
            //    //}
            //    //else
            //    //{
            //    //    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
            //    //    var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
            //    //    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
            //    //}

            //    //var force = perpendicularDirection * swingForce;
            //    //theRB.AddForce(force, ForceMode2D.Force);
            //}
            //else
            //{
            //    //animator.SetBool("IsSwinging", false);
            //    //if (isGrounded)
            //    //{
            //    //    var groundForce = speed * 2f;
            //    //    ////theRB.AddForce(new Vector2((horizontal * groundForce - theRB.velocity.x) * groundForce, 0));
            //    //    //theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y);
            //    //}
            //}
        }
        else
        {
            //animator.SetBool("IsSwinging", false);
            animator.SetFloat("Speed", 0f);
        }

        //if (!isSwinging)
        //{
        if (!isGrounded) return;

            //isJumping = jumpInput > 0f;
            //if (isJumping)
            //{
            //    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            //}
        //}

        if (isDashing)
        {
            return;
        }

        theRB.velocity = new Vector2(horizontal * speed, theRB.velocity.y);
    }
    public void StopPlayer()
    {
        theRB.velocity = Vector2.zero;
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = theRB.gravityScale;
        theRB.gravityScale = 0f;
        theRB.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        theRB.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void KnockBack()
    {
        //Inicializar el contador de KnockBack
        knockBackCounter = knockBackLength;
        //Paralizamos en X al jugador y hacemos que salte en Y
        theRB.velocity = new Vector2(0f, knockBackForce);
    }


}

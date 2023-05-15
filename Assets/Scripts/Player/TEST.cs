using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEST : MonoBehaviour
{
    public GameObject lanza;
    public UnityEvent<float> onReloading;
    

    public Transform spawner;
    public GameObject bulletPrefab;
    private float horizontal;
    private float speed = 4f;
    private float jumpForce = 5f;
    private bool isFacingRight = true;

    public bool isGrounded;
    private bool canDoubleJump;

    //Variable para saber cuando el jugador puede interactuar con los objetos
    public bool canInteract = false;

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
        //nextFireTime = 2;
        //Debug.Log(nextFireTime);
        onReloading?.Invoke(nextFireTime);
    }

    

    void Update()
    {

        animator.SetFloat("Velocity", Mathf.Abs(theRB.velocity.x));

        animator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("atacking");
        }


        if (nextFireTime <= 0)
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetTrigger("throwing");
                GameObject bullet = Instantiate(bulletPrefab, spawner.position, bulletPrefab.transform.rotation);

                nextFireTime = 2;
                Destroy(bullet, 2f);

            }

        }
        else
            onReloading?.Invoke(nextFireTime/cooldownTime);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }

    private void FixedUpdate()
    {
       

        if (isDashing)
        {
            return;
        }

        theRB.velocity = new Vector2(horizontal * speed, theRB.velocity.y);
        //if (!isSwinging)
        //{
        if (!isGrounded) return;

            //isJumping = jumpInput > 0f;
            //if (isJumping)
            //{
            //    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            //}
        //}

        

        
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

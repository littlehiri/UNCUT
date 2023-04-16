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

    public float knockBackLength, knockBackForce; //Valor que tendr� el contador de KnockBack, y la fuerza de KnockBack
    private float knockBackCounter;

    //Planeo
    public float slideForce;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 13f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = .5f;

    [SerializeField] private Rigidbody2D theRB;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private SpriteRenderer theSR;

    public static TEST sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    void Update()
    {
        //Si el juego est� pausado, no funciona el movimiento. Tampoco si el jugador est� parado
        if (!reference.isPaused && !stopInput)
        {
            //Si el contador de KnockBack se ha vaciado, el jugador recupera el control del movimiento
            if (knockBackCounter <= 0)
            {

                if (Input.GetKeyDown(KeyCode.Q))
                {
                  GameObject bullet = Instantiate(bulletPrefab, spawner.position, bulletPrefab.transform.rotation);
                  Destroy(bullet, 2f);
                }

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
                    }
                    else
                    {
                        if (canDoubleJump)
                        {
                            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                            canDoubleJump = false;
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
            //Si el contador de KnockBack todav�a no est� vac�o
            else
            {
                //Hacemos decrecer el contador en 1 cada segundo
                knockBackCounter -= Time.deltaTime;
                //Si el jugador mira a la izquierda
                if (!theSR.flipX)
                {
                    //Aplicamos un peque�o empuje a la derecha
                    theRB.velocity = new Vector2(knockBackForce, theRB.velocity.y);
                }
                //Si el jugador mira a la derecha
                else
                {
                    //Aplicamos un peque�o empuje a la izquierda
                    theRB.velocity = new Vector2(-knockBackForce, theRB.velocity.y);
                }
            }
        }
    }

    private void FixedUpdate()
    {
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

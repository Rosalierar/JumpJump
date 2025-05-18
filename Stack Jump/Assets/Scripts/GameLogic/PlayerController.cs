using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioClip[] audios;
    AudioSource audioSource;
    Animator anim;
    GameManager gameManager;
    Rigidbody2D rb;

    [Header("RayCastY")]
    [SerializeField] bool getBlock = false;
    [SerializeField] GameObject lastBlock;
    RaycastHit2D hit;
    public LayerMask layerMask;
    [SerializeField] private float distance;
    [SerializeField] Transform SpawnPoint;
    
    [Header("RayCastx")]
    public bool touchBlockSide = false;
    RaycastHit2D hitX1;
    RaycastHit2D hitX2;
    [SerializeField] private float distanceSides;
    public Transform SpawnPointXL;
    public Transform SpawnPointXR;

    [Header("Jump Controller")]
    byte totalJump = 1;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGrounded = false;

    float yAlturaMaxima = 2f;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    } 
    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOver)
        {
            return;
        }

        //BlockCheck();

        ControllerJump();
    }

    void CheckBlockSide()
    {
        hitX1 = Physics2D.Raycast(SpawnPointXR.position, Vector2.right, distanceSides, layerMask);
    
        if(hitX1.collider != null)
        {
            if (hitX1.collider.CompareTag("Block"))
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                print("colidio direito");
                touchBlockSide = true;
            }
        }
        hitX2 = Physics2D.Raycast(SpawnPointXL.position, Vector2.left, distanceSides, layerMask);

        if (hitX2.collider != null)
        {
            if (hitX2.collider.CompareTag("Block"))
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                print("colidio esquerdo");
                touchBlockSide = true;
            }
        }
    }

   void Jump()
    {
        anim.SetBool("isJump", true);
        print("Posso pular?");

        float a = Physics.gravity.magnitude;
        float m = rb.mass;

        float v = math.sqrt(2 * a * yAlturaMaxima);

        Vector2 f = m * v * Vector2.up;

        rb.AddForce(f, ForceMode2D.Impulse);

        Debug.Log("v: " + v + " f: " + f + " a: " + a);

        //gameManager.GetScussedPayer(false, false);

        isJumping = true;
        isGrounded = false;
        totalJump = 0;
    }

    void ControllerJump()
    {
        if (isGrounded && gameManager.isInstanciate)
        {
            if (Input.GetKeyDown(KeyCode.P) || (Input.GetMouseButtonDown(0)))
            {
                Jump();
            }
            else if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        Jump();

                        break;
                }
            }
        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = 1;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = 3;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                if (Mathf.Abs(normal.x) > 0.5f)
                {
                    // Colidiu pela lateral
                    Debug.Log("Colidiu pela lateral!");

                    gameManager.GetScussedPayer(false, true);
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    touchBlockSide = true;
                    break;
                }
            }

            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;

                if (Mathf.Abs(normal.y) > 0.6f)
                {
                    Debug.Log("Colidiu pela Horizontal!");
                    
                    rb.gravityScale = 1;
                    isGrounded = true;
                    isJumping = false;
                    anim.SetBool("isJump", false);
                    audioSource.clip = audios[1];
                    audioSource.Play();
                        
                    // Colidiu pela lateral

                    if (!getBlock && lastBlock != collision.gameObject)
                    {
                        Debug.Log("Não é igual");
                        lastBlock = collision.gameObject;
                        getBlock = true;
                    }
                        
                        if (getBlock && lastBlock == collision.gameObject)
                        {
                            //GetNewPosY();
                            Debug.Log("Mesmo");
                            gameManager.GetScussedPayer(true, false);
                            getBlock = false;
                        }

                        else if (!getBlock && lastBlock == collision.gameObject)
                        {
                            print("É o mesmo");
                            //lastBlock = hit.collider.gameObject;
                        }

                        Debug.Log("Hit Block down. Get Block: " + getBlock + "GameObject: " + collision);
                    break;
                }
            }
        }
    }
}

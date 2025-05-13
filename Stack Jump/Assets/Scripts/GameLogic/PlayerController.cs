using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rb;


    [Header("RayCastY")]
    bool getBlock = false;
    GameObject lastBlock;
    RaycastHit2D hit;
    public LayerMask layerMask;
    [SerializeField] private float distance;
    Vector2 SpawnPoint;
    
    [Header("RayCastx")]
    public bool touchBlockSide = false;
    RaycastHit2D hitX1;
    RaycastHit2D hitX2;
    [SerializeField] private float distanceSides;
    public Transform SpawnPointXL;
    public Transform SpawnPointXR ;

    [Header("Jump Controller")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isGrounded = false;

    bool pressedKeyCode = false;

    float yAlturaMaxima = 2f;
    //float v;
    //[SerializeField] private float t = 2f;
    //float m = 60;
   // float a = 10;
    //float f;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>();   

    } 
    // Update is called once per frame
    void Update()
    {
        CheckPosX();

        if (gameManager.isGameOver)
        {
            return;
        }

        BlockCheck();
        Jump();
    }
    void FixedUpdate()
    {
        /*if (transform.position.y >= yF && !touchBlockSide)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y - a);
        }
        else if (transform.position.y < yF && touchBlockSide)
        {
             rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - a);
        }*/
        
        
    }

    void Jump()
    {
        if (isGrounded && gameManager.isInstanciate && Input.GetKeyDown(KeyCode.P))
        {
            print("Posso pular?");

            float a = Physics.gravity.magnitude;
            float m = rb.mass;

            float v = math.sqrt(2 * a * yAlturaMaxima);

            Vector2 f = m * v * Vector2.up ;

            rb.AddForce(f, ForceMode2D.Impulse);
            
            Debug.Log("v: " + v + " f: " + f + " a: " + a);  

            isJumping = true;
            isGrounded = false;

            gameManager.GetScussedPayer(false, false);
        }
    }

    void BlockCheck()
    {
        hit = Physics2D.Raycast(SpawnPoint, Vector2.down, distance, layerMask);
        hitX1 = Physics2D.Raycast(SpawnPointXL.position, Vector2.left, distanceSides, layerMask);
        hitX2= Physics2D.Raycast(SpawnPointXR.position, Vector2.right, distanceSides, layerMask);

        Debug.DrawRay(SpawnPoint, Vector2.down * distance, Color.red);
        Debug.DrawRay(SpawnPointXL.position, Vector2.left * distanceSides, Color.cyan);
        Debug.DrawRay(SpawnPointXR.position, Vector2.right * distanceSides, Color.cyan);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Block"))
            {
                isGrounded = true;
                isJumping = false;

                if (!getBlock && lastBlock != hit.collider.gameObject)
                {
                    Debug.Log("N�o � igual");
                    lastBlock = hit.collider.gameObject;
                    getBlock = true;
                }
                else if (getBlock && lastBlock == hit.collider.gameObject)
                {
                    //GetNewPosY();
                    Debug.Log("Mesmo");
                    gameManager.GetScussedPayer(true, false);
                    getBlock = false;
                }

                Debug.Log("Hit Block down");
            }
        }

        /*if (hitX1.collider != null)
        {
            Debug.Log("Hit Block Left");
            if (hitX1.collider.CompareTag("Block") && hitX1.distance < 0.05f)
            {
                Debug.Log("Close Block Left");
                gameManager.GetScussedPayer(false, true);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                touchBlockSide = true;
            }
        }
        if (hitX2.collider != null)
        {
            Debug.Log("Hit Block Right");
            if (hitX2.collider.CompareTag("Block") && hitX2.distance < 0.05f)
            {
                Debug.Log("Close Block Right");
                gameManager.GetScussedPayer(false, true);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                touchBlockSide = true;
            }
        }*/
    }
    /* 
    void GetNewPosY()
    {
        yI = transform.position.y;
        yF = transform.position.y + 2.2f;
    }
    */
    void CheckPosX()
    {
        if (transform.position.x != 0)
        {
             gameManager.GetScussedPayer(false, true);
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
        }
    }
}

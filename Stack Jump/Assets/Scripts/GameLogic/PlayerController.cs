using System.Collections;
using System.Collections.Generic;
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

    float yI;
    float yF;
    float v;
    [SerializeField] private float t = 2f;
    float m = 60;
    float a = 10;
    float f;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        rb = GetComponent<Rigidbody2D>();   

    } 
    // Start is called before the first frame update
    void Start()
    {
        GetNewPosY();
    }


    // Update is called once per frame
    void Update()
    {
        SpawnPoint = transform.position;

        CheckPosX();

        pressedKeyCode = Input.GetKeyDown(KeyCode.P);
        
        Jump();
    }
    void FixedUpdate()
    {
        if (transform.position.y >= yF && !touchBlockSide)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y - a);
        }
        else if (transform.position.y < yF && touchBlockSide)
        {
             rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - a);
        }

        
        if (gameManager.isGameOver)
        {
            return;
        }

        BlockCheck();
    }

    void Jump()
    {
        if (isGrounded && gameManager.isInstanciate)
        {
            if (pressedKeyCode)
            {
                v = (yF) / t;
                f = m * a;

                rb.AddForce(new Vector2(0f, f), ForceMode2D.Impulse);

                Debug.Log("v" + v + " f: " + f);  

                isJumping = true;
                isGrounded = false;

                gameManager.GetScussedPayer(false, false);
            }
        }
    }

    void BlockCheck()
    {
        hit = Physics2D.Raycast(SpawnPoint, Vector2.down, distance, layerMask);
        hitX1 = Physics2D.Raycast(SpawnPointXL.position, Vector2.left, distance, layerMask);
        hitX2= Physics2D.Raycast(SpawnPointXR.position, Vector2.right, distance, layerMask);

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
                    lastBlock = hit.collider.gameObject;
                    getBlock = true;
                }
                else if (getBlock && lastBlock == hit.collider.gameObject)
                {
                    GetNewPosY();
                    gameManager.GetScussedPayer(true, false);
                    getBlock = false;
                }

                Debug.Log("Hit Block down");
            }
        }

        if (hitX1.collider != null)
        {
            if (hitX1.collider.CompareTag("Block") && hitX1.distance < 0.1f)
            {
                touchBlockSide = true;
                gameManager.GetScussedPayer(false, true);
            }
        }
        if (hitX2.collider != null)
        {
            if (hitX2.collider.CompareTag("Block") && hitX2.distance < 0.1f)
            {
                touchBlockSide = true;
                gameManager.GetScussedPayer(false, true);
            }
        }
    }
    void GetNewPosY()
    {
        yI = transform.position.y;
        yF = transform.position.y + 2.2f;
    }

    void CheckPosX()
    {
        if (transform.position.x != 0)
        {
             gameManager.GetScussedPayer(false, true);
        }
    }
}

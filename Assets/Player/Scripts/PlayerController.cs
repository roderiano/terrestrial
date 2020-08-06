using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Attributes")]
    [SerializeField] private float speed = 13f;
    [Range(0, 50)]
    [SerializeField] private float fallMultiplier = 6f;
    [Range(0, 50)]
    [SerializeField] private float jumpMultiplier = 5f;
    [Range(0, 50)]
    [SerializeField] private float jumpForce = 25f;
    [Range(0, 10)]
    [SerializeField] private int maxHealth;
    [Range(0.0f, 1f)]
    [SerializeField] private float jumpMaxTime;

    [SerializeField] private int memoriesAmount = 0;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontal;
    private Animator animator;
    private float checkRadius;
    private LayerMask layer;
    private PlayerStatus status;
    private Transform groundDetector;
    private SkillTreeManager skillTreeManager;
    private int health;
    

    private void Start()
    {
        checkRadius = 0.03f;
        rb = GetComponent<Rigidbody2D>();
        layer = LayerMask.NameToLayer("Player");
        groundDetector = transform.Find("groundDetector").transform;
        animator = transform.Find("playerWithIK").GetComponent<Animator>();
        skillTreeManager = FindObjectOfType(typeof(SkillTreeManager)) as SkillTreeManager;
        health = maxHealth;

        LoadPlayer();
    }

    private void Update() 
    {
        //REMOVER DEPOIS
        if(Input.GetKeyDown(KeyCode.Escape) && status != PlayerStatus.Dialoguing)
            SceneManager.LoadScene("Main Menu");
        // ---

        DetectGround();
        RotatePlayer();
        GravityController();
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if(status == PlayerStatus.Moving)
        {
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                StartCoroutine(Jump());  
            }

            if(Input.GetKeyDown(KeyCode.LeftShift) && skillTreeManager.SkillIsAdquired(SkillType.Dash)) 
            {
                StartCoroutine(Dash());
            }
            
            animator.SetBool("isRunning", horizontal != 0 ? true : false);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if(Input.GetKeyDown(KeyCode.X)){
            StartCoroutine(Attack());
        }
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void RotatePlayer() 
    {
        Transform body = transform.Find("playerWithIK");
            
        if (horizontal > 0)
            body.rotation = new Quaternion(0f, 0f, 0f, 0f);
        else if (horizontal < 0)
            body.rotation = new Quaternion(0f, 180f, 0f, 0f);
    }

    private void Move() 
    {
        if(status == PlayerStatus.Moving || status == PlayerStatus.Jumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        } 
        else if(status != PlayerStatus.UsingAbility) 
        {
            rb.velocity = new Vector2(0, rb.velocity.y); 
        }
    }

    private void DetectGround() 
    {
        isGrounded = Physics2D.OverlapCircle(groundDetector.position, checkRadius, ~(layer));
    }

    private IEnumerator Jump() 
    {
        status = PlayerStatus.Jumping; 

        float startTime = Time.time;
        Debug.Log(startTime);

        while(!Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = (rb.velocity.normalized + Vector2.up) * jumpForce;
            
            if(Time.time - startTime > jumpMaxTime)
            {
                break;
            }

            yield return null;
        }

        status = PlayerStatus.Moving;
    }

    private IEnumerator Dash() 
    {
        float dir;
        if(horizontal != 0)
        {
            dir = horizontal;
        }
        else
        {
            dir = transform.Find("riggedPlayer").rotation.y == 0 ? -1 : 1; 
        }
        
        status = PlayerStatus.UsingAbility;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;


        rb.velocity = new Vector2(dir * 50f, rb.velocity.y);

        yield return new WaitForSeconds(0.2f);    
        
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        status = PlayerStatus.Moving;
    }

    private IEnumerator Attack() {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isAttacking", false); 
    }

    private void GravityController() 
    {
        if(rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0) 
        {
             rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier) * Time.deltaTime;   
        }
    }

    public void SetStatus(PlayerStatus playerStatus) 
    {
        this.status = playerStatus;
    }

    public PlayerStatus GetStatus() 
    {
        return status;
    }

    public void SavePlayer() 
    {
        SaveSystem.SavePlayer(this.gameObject, PlayerPrefs.GetString("Slot"));
    }

    public void LoadPlayer() 
    {
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("Slot"));
        if(data != null)
        {
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            memoriesAmount = data.memories;
        }  
    }

    public int GetMemoriesAmount()
    {
        return memoriesAmount;
    }

    public void AddMemories(int amount)
    {
        memoriesAmount += amount;
    }

    public void TakeDamage()
    {
        health -= 1;
        //rb.velocity = Vector2.up * 10f;
    }

    private void Die() 
    {
        LoadLastSave();
    }

    public void LoadLastSave() 
    {
        Debug.Log(PlayerPrefs.GetString("Slot"));
        PlayerData data = SaveSystem.LoadPlayer(PlayerPrefs.GetString("Slot"));
        
        if(data != null)
        {
            SceneManager.LoadScene(data.scene);
        }
    }
}

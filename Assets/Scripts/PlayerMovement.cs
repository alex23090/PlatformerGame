using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump_speed;
    [SerializeField] private LayerMask groundLayer;
    public PlayerCombat timeisgone;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private bool onAir;
    float horizontalInput;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.freezeRotation = true;
        timeisgone = FindObjectOfType<PlayerCombat>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-3, 3, 3);

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        onAir = isGrounded();
        if (onAir == false)
        {
            anim.SetTrigger("Fall");
        }
        if (timeisgone.health <= 0)
        {
            this.enabled = false;
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jump_speed);
        anim.SetTrigger("jump");
        anim.SetTrigger("Fall");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer); ;
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded();
    }
    public void respawn()
    {
        //body.position = new Vector3(0f, 0f, 0f);
        GetComponent<Rigidbody2D>().position = Vector3.zero;
    }
}

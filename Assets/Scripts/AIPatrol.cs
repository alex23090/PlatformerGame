using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    [SerializeField] private float jump_speed;
    public float walkSpeed;

    [HideInInspector]
    public bool mustPatrol;
    private bool mustTurn;
    private bool mustStop;
    private bool mustTurnBack;
    private bool stop;
    private bool mustCalm;
    private bool mustJump;
    private Animator anim;
    private Rigidbody2D body;

    public Rigidbody2D rb;
    public Transform groundCheckPos;
    public Transform playerCheckPos;
    public Transform backCheckPos;
    public Transform groundedCheckPos;
    public Transform TopCheckPos;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public LayerMask backLayer;
    public LayerMask grounded;
    public LayerMask top;
    public int attackDamage;
    public Transform attackPointP;
    public float attackRange = 0.7f;

    public PlayerCombat ph;
    public Enemy stp;

    float nextAttackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mustPatrol = true;
        stop = true;
        anim = GetComponent<Animator>();
        anim.SetBool("canWalk", true);
        ph = FindObjectOfType<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        if(stp.currentHealth <= 0)
        {
            this.enabled = false;
        }
        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
            mustTurnBack = Physics2D.OverlapCircle(backCheckPos.position, 1f, backLayer);
            mustCalm = Physics2D.OverlapCircle(groundedCheckPos.position, 1f, grounded);
        }
            if (stop)
            {
                mustStop = Physics2D.OverlapCircle(playerCheckPos.position, 0f, playerLayer);
                mustJump = Physics2D.OverlapCircle(TopCheckPos.position, 0.1f, playerLayer);
                if (mustStop || mustJump)
                {
                    mustPatrol = false;
                    anim.SetBool("canWalk", false);
                    if (mustJump)
                    {
                        anim.SetTrigger("Jump");
                        body.velocity = new Vector2(body.velocity.x, jump_speed);
                    }
                    if (Time.time >= nextAttackTime)
                    {
                        if (ph.health > 0)
                        {
                            anim.SetTrigger("Attack");
                            nextAttackTime = Time.time + 1f / 2f;
                            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPointP.position, attackRange, playerLayer);
                            foreach (Collider2D player in hitPlayer)
                                player.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
                        }
                    }
                }
                else
                {
                    mustPatrol = true;
                    anim.SetBool("canWalk", true);
                }
            }
    }

    void Patrol()
    {
        if ((mustTurn || mustTurnBack) && mustCalm)
        {
            Flip();
        }

        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
    void OnDrawGizmosSelected()
    {
        if (attackPointP == null)
            return;

        Gizmos.DrawWireSphere(attackPointP.position, attackRange);
    }
}

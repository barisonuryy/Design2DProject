using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class BasicMech : MonoBehaviour
{
    // Start is called before the first frame update
    public float dirSlide;
    float directionX,directionY;
    Rigidbody2D rb;
    Vector2 movement;
    public bool move,jump,slide;
    public float speed;
    Animator animator;
    public float verticalS;
    public float horizontalS;
    private bool isFacingRight = true;
    public bool canDash = true;
    public bool isDashing;
    public  bool dashControl;
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    private float dashingCoolDown = 3f;
    public BoxCollider2D bc2d;
    [SerializeField] private LayerMask platformLayerMask; 
    [SerializeField] private TrailRenderer tr;
    private int jumpCount;
    [SerializeField] private float jumpCoolDown;
    public bool attack;
    [SerializeField] private float cooldownAttack,attackRangeX,attackRangeY;
    private float totalDurationA;
    [SerializeField] private Transform attackPos;
    [SerializeField] private BoxCollider2D[] _boxCollider2DEnemies;
    private float timerJump;
    private bool isRotate;
    private void Awake()
    {
        timerJump = 0;
        jumpCount = 0;
        rb=GetComponent<Rigidbody2D>(); 
        bc2d=GetComponent<BoxCollider2D>();
    }
    void Start()
    {
        totalDurationA = 0;
        animator =GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        dashControl = false;
        if(isDashing)
        {
            return;
        }
        Movement();
        Jump(isGrounded());
        SlideE();
        Flip();
        Attack();

        //Glide();
         }
    void Movement()
    {

        directionX= Input.GetAxis("Horizontal");
  
        if (directionX!=0)
        {
            move =true;
        }
        else move=false;
     }
    // ReSharper disable Unity.PerformanceAnalysis
   
    void Jump(bool isGrounded)
    {
        if (isGrounded && Input.GetButtonUp("Vertical")&&jumpCount<=2)
        {
            jumpCount++;
            jump = true;
            directionY = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalS);
           
        }
        else
        {
            if (timerJump < Time.time)
            {
                jumpCount = 0;
                timerJump = Time.time + jumpCoolDown;
            }
            
            jump = false;
        }
           
    }
  public  void Flip()
    {
        if((!isFacingRight&&directionX>0f||isFacingRight&&directionX<0f)) {
                isFacingRight = !isFacingRight;
                Vector3 localScale= transform.localScale;
                localScale.x *= -1f;
                dirSlide = localScale.x;
                transform.localScale = localScale;
            }
           

           
        
    
        
    }

    public void SetRespawnFlip()
    {
        if (!isFacingRight&&directionX<=0)
        {
            Vector3 localScale= transform.localScale;
            localScale.x *= -1f;
            dirSlide = localScale.x;
            transform.localScale = localScale;
        }
    }
     void Attack()
    {

            if (Input.GetButton("Attack")&& Time.time > totalDurationA)
            {
            totalDurationA = Time.time + cooldownAttack;
                attack = true;
              
                Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0);
                
                foreach (Collider2D enemy in enemiesToDamage)
                {
                if (enemy.gameObject != null&&attack)
                {
                    if (enemy.gameObject.CompareTag("CatEnemy"))
                    {
                        Destroy(enemy, 0.05f);
                 
                    }
                    if (enemy.gameObject.name == "Minotaur")
                    {
                        enemy.GetComponentInParent<MinotaurHealth>().TakeDamageMinotaur(10);
                    }
                    if (enemy.gameObject.CompareTag("Archer"))
                    {
                        enemy.GetComponentInParent<GoblinHealth>().TakeDamageGoblin(10);
                    }
                  

                }
            }
                
            
            }
            else attack = false;

    }
    private IEnumerator Slide()
    {
        dashControl = true;
        canDash= false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        move = false;
        jump = false;
       
            foreach (var boxCollider in _boxCollider2DEnemies)
            {
                if(boxCollider!=null)
                boxCollider.isTrigger = true;
            }

        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale=originalGravity;
        isDashing = false;
        jump = true; 
       
            foreach (var boxCollider in _boxCollider2DEnemies)
            {
                if(boxCollider!=null)
                boxCollider.isTrigger = false;
            }
    

        yield return new WaitForSeconds(dashingCoolDown);
        canDash = true;
    }
    private bool isGrounded()
    {
        float extraHeight = 2.5f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, extraHeight, platformLayerMask);
        Vector3.Angle(Vector2.up,rb.velocity);

        return raycastHit.collider!=null;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    
        rb.velocity = new Vector2(directionX * horizontalS, rb.velocity.y);
        
    }
    private void OnAnimatorMove()
    {
        animator.SetBool("isAttack",attack);
        animator.SetBool("isWalking", move);
        animator.SetBool("isJump", jump);
        animator.SetBool("canSlide", slide);
    }
    private void OnDrawGizmos()
    {

        
    }
    void SlideE()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            slide = true;
            StartCoroutine(Slide());

        }
        else slide = false;
        
    }

   


}
   
    


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class meleeAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject player;
    private float cooldownTimer = Mathf.Infinity;
    [Header("AttackAnimInfo")]
    [SerializeField] AnimationClip animationClip;
    [SerializeField] float scaleHitBox=1f;
    private float animLength;

    public bool continueAttack;
    //References
    private Animator anim;
    private PlayerHealth playerHealth;
    private meleeEnemyMovement enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<meleeEnemyMovement>();
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex==5)
        player = GameObject.FindWithTag("Player");
        animLength = animationClip.length;
        continueAttack = false;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight()&&player.tag=="Player")
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
                continueAttack = true;
                Invoke("DamagePlayer",animLength-0.05f);
                
            }
           
        }
        else
        {
            continueAttack = false;
        }
        

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight()||player.tag=="Invisible";
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range*scaleHitBox, boxCollider.bounds.size.y * scaleHitBox, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range*scaleHitBox, boxCollider.bounds.size.y*scaleHitBox, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
            playerHealth.TakeDamageP(true);
        }
           
    }
}

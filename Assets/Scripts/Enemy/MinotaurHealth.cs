using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MinotaurHealth : MonoBehaviour
{
    public float MinoHealth = 100f;
    public float MinoMaxHealth = 100f;
    [SerializeField] GameObject smoke,score;
    private bool isDead;
    [SerializeField] private LevelManage _levelManage;
    private bool canContAttack;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        isDead = false;
        
        GetComponentInChildren<MinoHealthUI>().setHealthUI(MinoHealth,MinoMaxHealth);
    }

    private void Update()
    {
        if(GetComponentInChildren<MinoHealthUI>()!=null)
        GetComponentInChildren<MinoHealthUI>().setHealthUI(MinoHealth,MinoMaxHealth);
        canContAttack = GetComponent<meleeAttack>().continueAttack;
    
    }

    public void TakeDamageMinotaur(float damage)
    {
        MinoHealth -= damage;
        
        if (!isDead&&!canContAttack)
        {
            anim.SetTrigger("isHurt");
        } 
        if (MinoHealth <= 0&&!isDead)
        {
            anim.SetTrigger("isDie");
            GetComponentInParent<meleeEnemyMovement>().enabled = false;
            GetComponentInParent<meleeAttack>().enabled = false;
            Destroy(gameObject,2f);
            isDead = true;
        }
    }

    private void OnDestroy()
    {
        smoke.SetActive(true);
        smoke.transform.position=transform.root.position;
        _levelManage.score += 50;
        score.GetComponent<Animator>().SetBool("scoreIncrease",true);
    }

    public void IncreaseHealth(float incHealth)
    {
        if (MinoHealth < MinoMaxHealth)
        {
            MinoHealth += (incHealth / MinoMaxHealth) * 100;
            //setParticleState();
        }
        else
            MinoHealth = MinoMaxHealth;
        

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Snowball"))
        {
            TakeDamageMinotaur(5);
            Destroy(other.gameObject);
        }
    }

    /*async void setParticleState()
    {
        prt.Play();
        await Task.Delay(1500);
        prt.Stop();
    }*/
}

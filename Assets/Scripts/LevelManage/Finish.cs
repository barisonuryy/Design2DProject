using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
   public bool passed;
    private void Start()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
          
           GameObject.Find("Kinder").GetComponent<BasicMech>().enabled= false;
            GameObject.Find("Kinder").GetComponent<Animator>().enabled = false;
            passed= true;


            //SceneManager.LoadScene("SecondScene");
        }
    }


}

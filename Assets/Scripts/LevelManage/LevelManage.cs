using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class LevelManage : MonoBehaviour
{
    // Start is called before the first frame update

    bool p;
    public AudioSource bgm;
    public AudioSource PauseClick;
    public int score;
    public bool dead;
    public GameObject smoke;
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private Slider[] volumeSlider;
     public AudioSource[] audioSource;
     

    void Start()
    {
        for(int i=0;i<audioSource.Length;i++)
        volumeSlider[i].onValueChanged.AddListener(delegate { ChangeVolume(); });
     
    }
    public void ChangeVolume()
    {
        for(int i=0;i<audioSource.Length;i++)
        audioSource[i].volume = volumeSlider[i].value;
    }

    // Update is called once per frame
    void Update()
    {
        if(p!=null)
        p = GetComponentInChildren<Finish>().passed;
        if(mainCharacter != null )
        {
            int health = mainCharacter.GetComponent<PlayerHealth>().initialHealth;
            if (health == 0)
            {
                //SceneManager.LoadScene("SampleScene");
                dead = true;
            }
        }
       
  
        if (p)
        {
            bgm.Stop();
        }
     
 
      
     
    }

    IEnumerator RestartGameOrder()
    {
        StartCoroutine(GetComponent<FadeInFadeOut>().FadeIn(mainCharacter.GetComponent<SpriteRenderer>()));
        yield return new WaitForSeconds(1f);
        StartCoroutine(GetComponent<FadeInFadeOut>().FadeOut(mainCharacter.GetComponent<SpriteRenderer>(),-0.05f));
        yield return new WaitForSeconds(1f);
       
    }


   

    private void showScore(int score)
    {

    }
    private void playScoreAnim()
    {

    }

    public void StopCharacter()
    {
        if (mainCharacter != null)
        {
            mainCharacter.GetComponent<BasicMech>().enabled = false;
        }
    }

    public void PlayCharacter()
    {
        mainCharacter.GetComponent<BasicMech>().enabled = true;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public void pauseClick()
    {
        PauseClick.Play();
    }
    public void restartGame()
    {
        //mainCharacter.GetComponent<PlayerHealth>().SaveHealthValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void exitGame()
    {
        Application.Quit();

    }
    public void backHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnApplicationQuit()
    {
        mainCharacter.GetComponent<PlayerHealth>().SaveHealthValues();
        //PlayerPref verilerinin kaydedildiği kısım burası olacak
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&other.gameObject.name!="Legs")
        {
            mainCharacter.GetComponent<PlayerHealth>().DecreaseGeneralHealth();
            StartCoroutine(mainCharacter.GetComponent<PlayerHealth>().Respawn(0.5f)); 
        }
    }
}

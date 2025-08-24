using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainmenuscript : MonoBehaviour
{
    [SerializeField] Animator blackScreenAnim;
    [SerializeField] AudioSource clickSFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newRun() 
    {
        blackScreenAnim.SetTrigger("fadeIn");
        clickSFX.Play();
        Invoke("delayedNewRun", 3);
    }
    void delayedNewRun() 
    {
        SceneManager.LoadScene("Battle");
    }
}

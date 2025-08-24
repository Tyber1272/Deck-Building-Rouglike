using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class gameOverScript : MonoBehaviour
{
    [SerializeField] Text stats;
    HealthScript playerScript;
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>();
        stats.text =
            $"Stats: \n" +
            $"Encounter count: {playerScript.encouterCount} \n" +
            $"Highest HP: {playerScript.highestHP} \n" +
            $"Enemies killed: {playerScript.killCount}"
            ;

        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void restart() 
    {
        SceneManager.LoadScene(0);
    }

}

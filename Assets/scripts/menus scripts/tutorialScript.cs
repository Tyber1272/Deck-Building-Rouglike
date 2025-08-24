using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialScript : MonoBehaviour
{
    public GameObject[] boxsTutorialList;
    int count = 0;
    void Start()
    {
        count = 0;
        nextBox();
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>().firstBattle == false)
        {
            skip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextBox();
        }
    }
    public void nextBox() 
    {
        foreach (var item in boxsTutorialList)
        {
            item.SetActive(false);
        }
        if (count >= boxsTutorialList.Length)
        {
            skip();
        }
        else
        {
            boxsTutorialList[count].SetActive(true);
        }
        count++;
    }
    public void skip() 
    {
        Destroy(gameObject);
    }
}

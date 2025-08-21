using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int encounterCount; Text encounterText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void newBattle() 
    {
        Invoke("delayedNewBattle", 0.02f);
    }
    void delayedNewBattle() 
    {
        encounterText = GameObject.FindWithTag("encounterText").GetComponent<Text>();
        encounterCount = encounterCount + 1;
        encounterText.text = "Encounter: " + encounterCount;
    }
}

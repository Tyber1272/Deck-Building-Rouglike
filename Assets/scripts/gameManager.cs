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
        encounterText = GameObject.FindWithTag("encounterText").GetComponent<Text>();
        encounterCount++;
        encounterText.text = "Encounter: " + encounterCount;
    }
}

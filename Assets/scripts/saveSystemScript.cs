using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveSystemScript : MonoBehaviour
{
    [SerializeField] bool testy;
    public bool newGame;

    actionInventory inventoryScript;
    HealthScript healthScript;
    private void Start()
    {
        inventoryScript = GameObject.FindGameObjectWithTag("Player").GetComponent<actionInventory>();
        if (testy == false)
        {
            if (newGame == true)
            {
                newGame = false;
                PlayerPrefs.SetString("name0", "strike"); PlayerPrefs.SetFloat("power0", 6);
                PlayerPrefs.SetString("name1", "defend"); PlayerPrefs.SetFloat("power1", 5);
                PlayerPrefs.SetString("name2", "heal");   PlayerPrefs.SetFloat("power2", 2);

                
            }

            setInventoryActionsAsPrefs();
        }
        else
        {
            
        }
    }

    void setInventoryActionsAsPrefs() 
    {
        inventoryScript.unitActions.Clear();

        //inventoryScript.unitActions.Add(new actionsClass.action(PlayerPrefs.GetString("name0"), PlayerPrefs.GetFloat("power0"), 0));

    }
}

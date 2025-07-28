using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class actionInventory : MonoBehaviour
{
    actionsClass actionsClass;
    public List<actionsClass.action> unitActions = new List<actionsClass.action>();
    public List<GameObject> holders = new List<GameObject>();
    [SerializeField] bool player = false;
    [SerializeField] GameObject actionPrefab;
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject holdersInventory;
    [SerializeField] GameObject holderPrefab;
    [SerializeField] GameObject Canvas;


    private void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (!player)
        {
            unitActions.Add(new actionsClass.action("strike", 5));

            holdersInventory = GameObject.FindGameObjectWithTag("enemiesActionsParent");
        }
        else 
        {
            unitActions.Add(new actionsClass.action("strike", 5));
            unitActions.Add(new actionsClass.action("strike", 5));
            unitActions.Add(new actionsClass.action("defend", 7));
            unitActions.Add(new actionsClass.action("heal", 4));

        }

    }

    public void newTurn() 
    {
        Invoke("startTurn", 0.1f);
    }
    void startTurn() 
    {
        gameObject.GetComponent<HealthScript>().block = 0; gameObject.GetComponent<HealthScript>().updateStats();
        if (gameObject.GetComponent<HealthScript>().alive == false)
        {
            return;
        }
        if (player) 
        {
            Inventory.GetComponent<HorizontalLayoutGroup>().enabled = true;
            holders.Clear();
            foreach (var action in unitActions)
            {
                holders.Add(Instantiate(holderPrefab, transform.position, transform.rotation, holdersInventory.transform));
            }
            int count = 0;
            foreach (var action in unitActions)
            {
                GameObject currentAction = Instantiate(actionPrefab, transform.position, transform.rotation, Inventory.transform);
                actionPrefabScript script = currentAction.GetComponent<actionPrefabScript>();
                script.setStats(action.name, action.power, holders[count], gameObject);
                count++;
            }
            Invoke("diseableInventoryLayout", 0.1f);
        }
        else
        {
            GameObject[] slots = GameObject.FindGameObjectsWithTag("slot");
            foreach (var slot in slots) 
            {
                SlotScript script = slot.GetComponent<SlotScript>();
                if (script.user == gameObject)
                {
                    actionsClass.action action = unitActions[Random.Range(0, unitActions.Count)];
                    GameObject currentAction = Instantiate(actionPrefab, slot.transform.position, transform.rotation, holdersInventory.transform);
                    actionPrefabScript script2 = currentAction.GetComponent<actionPrefabScript>();
                    script2.setStats(action.name, action.power, slot.GetComponent<SlotScript>().actionHolderScript.gameObject, gameObject);
                }
            }
        }

    }
    void diseableInventoryLayout() 
    {
        Inventory.GetComponent<HorizontalLayoutGroup>().enabled = false;
    }
}

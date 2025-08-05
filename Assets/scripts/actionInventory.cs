using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class actionInventory : MonoBehaviour
{
    actionsClass actionsClass;
    public List<actionsClass.action> unitActions = new List<actionsClass.action>();
    public List<actionsClass.action> readyUnitActions = new List<actionsClass.action>();
    public List<GameObject> holders = new List<GameObject>();
    List<int> cooldownsTimer = new List<int>();
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
            unitActions.Add(new actionsClass.action("strike", 1, 0));
            unitActions.Add(new actionsClass.action("defend", 2, 1));
            unitActions.Add(new actionsClass.action("heal", 3, 2));
            holdersInventory = GameObject.FindGameObjectWithTag("enemiesActionsParent");
        }
        else 
        {
            unitActions.Add(new actionsClass.action("strike", 5, 0));
            unitActions.Add(new actionsClass.action("strike", 5, 0));
            unitActions.Add(new actionsClass.action("defend", 7, 0));
            unitActions.Add(new actionsClass.action("heal", 4, 2));

        }
        foreach (var actions in unitActions)
        {
            if (actions.coolDown > 0)
            {
                cooldownsTimer.Add(actions.coolDown + 1);
            }
            else
            {
                cooldownsTimer.Add(actions.coolDown);
            }
        }
    }
    public void addAction(actionsClass.action action) 
    {
        unitActions.Add(action);
        if (action.coolDown > 0)
        {
            cooldownsTimer.Add(action.coolDown + 1);
        }
        else
        {
            cooldownsTimer.Add(action.coolDown);
        }
    }
    public void newBattle() 
    {
        Inventory = GameObject.FindGameObjectWithTag("inventory");
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
        for (int i = 0; i < unitActions.Count; i++)
        {
            unitActions[i].inventoryOrder = i;
        }
        if (player) 
        {
            Inventory = GameObject.FindGameObjectWithTag("inventory");
            holdersInventory = GameObject.FindGameObjectWithTag("slotInventory");
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
                if (cooldownsTimer[count] > 0)
                {
                    cooldownsTimer[count] = cooldownsTimer[count] - 1;
                }
                script.setStats(action.name, action.power, holders[count], gameObject, cooldownsTimer[count], count);
                count++;
            }
            Invoke("diseableInventoryLayout", 0.1f);
        }
        else // enemy
        {
            GameObject[] slots = GameObject.FindGameObjectsWithTag("slot");
            readyUnitActions.Clear();
            int count = 0;
            foreach (var action in unitActions)
            {
                if (cooldownsTimer[count] <= 0)
                {
                    readyUnitActions.Add(action);
                    
                }
                count++;
            }
            count = 0;
            for (int i = 0; i < unitActions.Count; i++) 
            {
                if (cooldownsTimer[i] > 0)
                {
                    cooldownsTimer[i] = cooldownsTimer[i] - 1;
                }
            }
            foreach (var slot in slots) 
            {
                SlotScript script = slot.GetComponent<SlotScript>();
                if (script.user == gameObject && readyUnitActions.Count > 0)
                {
                    actionsClass.action action = readyUnitActions[Random.Range(0, readyUnitActions.Count)]; 
                    GameObject currentAction = Instantiate(actionPrefab, slot.transform.position, transform.rotation, holdersInventory.transform);
                    actionPrefabScript script2 = currentAction.GetComponent<actionPrefabScript>();
                    script2.setStats(action.name, action.power, slot.GetComponent<SlotScript>().actionHolderScript.gameObject, gameObject, cooldownsTimer[count], action.inventoryOrder);
                    count++;
                }
            }
        }

    }
    public void actionUsed(int actionOrder) 
    {
        if (true)
        {
            if (unitActions[actionOrder].coolDown > 0)
            {
                cooldownsTimer[actionOrder] = unitActions[actionOrder].coolDown + 1;
            }
            else
            {
                cooldownsTimer[actionOrder] = unitActions[actionOrder].coolDown; // czyli 0
            }
        }
        
    }
    void diseableInventoryLayout() 
    {
        Inventory.GetComponent<HorizontalLayoutGroup>().enabled = false;
    }
}

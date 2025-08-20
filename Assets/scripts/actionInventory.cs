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
    GameManager gameManager;
    public enum enemyType
    {
        basic, aggresive, defensive, healer, poisoner
    }
    public enemyType enemyName;
    private void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        if (!player)
        {
            enemiesTypesInventory();
            holdersInventory = GameObject.FindGameObjectWithTag("enemiesActionsParent");
        }
        else 
        {
            unitActions.Add(new actionsClass.action("strike", 10, 0));
            unitActions.Add(new actionsClass.action("strike", 10, 0));
            unitActions.Add(new actionsClass.action("defend", 5, 0));
            unitActions.Add(new actionsClass.action("heal", 3, 0));
            unitActions.Add(new actionsClass.action("poison", 10, 2));

            holders.Clear();
            foreach (var action in unitActions)
            {
                holders.Add(Instantiate(holderPrefab, transform.position, transform.rotation, holdersInventory.transform));
            }
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
        gameObject.GetComponent<HealthScript>().newBattle();
        print("new battle");
        holders.Clear();
    }

    public void newTurn() 
    {
        Invoke("startTurn", 0.1f);
    }
    void startTurn() 
    {
        gameObject.GetComponent<HealthScript>().block = 0; gameObject.GetComponent<HealthScript>().updateStats();
        gameObject.GetComponent<HealthScript>().newTurn();
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
            if (holders.Count == 0)
            {
                print("niga bliat");
                holders.Clear();
                foreach (var action in unitActions)
                {
                    holders.Add(Instantiate(holderPrefab, transform.position, transform.rotation, holdersInventory.transform));
                }
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
                script.setStats(action.name, action.power, action.coolDown, holders[count], gameObject, cooldownsTimer[count], count);
                count++;
            }
            Invoke("diseableInventoryLayout", 0.1f);
        }
        else // enemy
        {
            GameObject[] slots = GameObject.FindGameObjectsWithTag("slot");
            readyUnitActions.Clear();
            int count = 0;
            count = 0;
            for (int i = 0; i < unitActions.Count; i++)
            {
                if (cooldownsTimer[i] > 0)
                {
                    cooldownsTimer[i] = cooldownsTimer[i] - 1;
                }
            }
            foreach (var action in unitActions)
            {
                if (cooldownsTimer[count] <= 0)
                {
                    readyUnitActions.Add(action);
                    
                }
                count++;
            }
            count = 0;
            foreach (var slot in slots) 
            {
                SlotScript script = slot.GetComponent<SlotScript>();
                if (script.user == gameObject && readyUnitActions.Count > 0 && readyUnitActions.Count != 0)
                {
                    actionsClass.action action = readyUnitActions[Random.Range(0, readyUnitActions.Count)];
                    if (cooldownsTimer[count] == 0)
                    {
                        readyUnitActions.Remove(action);
                        GameObject currentAction = Instantiate(actionPrefab, slot.transform.position, transform.rotation, holdersInventory.transform);
                        actionPrefabScript script2 = currentAction.GetComponent<actionPrefabScript>();
                        script2.setStats(action.name, action.power, action.coolDown, slot.GetComponent<SlotScript>().actionHolderScript.gameObject, gameObject, cooldownsTimer[count], action.inventoryOrder);
                    }
                    
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

    void enemiesTypesInventory() 
    {
        switch (enemyName)
        {
            case enemyType.basic:
                unitActions.Add(new actionsClass.action("strike", 5, 0));
                unitActions.Add(new actionsClass.action("strike", 6, 0));
                unitActions.Add(new actionsClass.action("defend", 7, 0));
                break;
            case enemyType.aggresive:
                unitActions.Add(new actionsClass.action("strike", 10, 1));
                unitActions.Add(new actionsClass.action("strike", 7, 0));
                unitActions.Add(new actionsClass.action("strike", 5, 0));
                break;
            case enemyType.defensive:
                unitActions.Add(new actionsClass.action("strike", 6, 0));
                unitActions.Add(new actionsClass.action("defend", 10, 0));
                unitActions.Add(new actionsClass.action("defend", 14, 1));
                break;
            case enemyType.healer:
                unitActions.Add(new actionsClass.action("strike", 5, 0));
                unitActions.Add(new actionsClass.action("heal", 6, 0));
                unitActions.Add(new actionsClass.action("heal", 15, 2));
                break;
            case enemyType.poisoner:
                unitActions.Add(new actionsClass.action("strike", 8, 0));
                unitActions.Add(new actionsClass.action("heal", 7, 0));
                unitActions.Add(new actionsClass.action("poison", 4, 0));
                unitActions.Add(new actionsClass.action("poison", 10, 2));
                break;
        }
        foreach (var action in unitActions)
        {
            action.power = action.power + randomNum();
            if (action.power <= 0)
            {
                action.power = 2;
            }
        }
    }
    int randomNum() 
    {
        return (gameManager.encounterCount + Random.Range(-3, 4));
    }
}

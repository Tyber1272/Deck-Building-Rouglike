using System.Collections;
using System.Collections.Generic;
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
        unitActions.Add(new actionsClass.action("strike", 5));
        unitActions.Add(new actionsClass.action("strike", 5));
        unitActions.Add(new actionsClass.action("defend", 7));
        unitActions.Add(new actionsClass.action("heal", 4));
        unitActions.Add(new actionsClass.action("8==D", 69));

        newTurn();
    }

    void newTurn() 
    {
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
                script.setStats(action.name, action.power, holders[count]);
                count++;
            }
            Invoke("diseableInventoryLayout", 0.1f);
            
        }
    }

    void diseableInventoryLayout() 
    {
        Inventory.GetComponent<HorizontalLayoutGroup>().enabled = false;
    }
}

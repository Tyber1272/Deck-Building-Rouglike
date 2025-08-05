using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    GameObject player;
    GameObject[] enemies;
    List<GameObject> aliveEnemies = new List<GameObject>();
    List<HealthScript> healthScripts = new List<HealthScript>();
    List<slotClass> slotsList = new List<slotClass>();
    List<GameObject> slotsObjectsList = new List<GameObject>();
    [SerializeField] GameObject slotPrefab;
    [SerializeField] battleManager BattleManager;
    actionsMethods actionsMethods;
    int actionCount;
    public float coolDownAction;
    public bool turnInProcess;
    void Start()
    {
        actionsMethods = GameObject.FindGameObjectWithTag("actionsMethods").GetComponent<actionsMethods>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<HealthScript>().alive == true)
            {
                aliveEnemies.Add(enemy);
            }
            else
            {
                aliveEnemies.Remove(enemy);
            }
        }
        newTurn();
    }

    void Update()
    {

    }

    void newTurn()
    {
        healthScripts.Clear();

        healthScripts.Add(player.GetComponent<HealthScript>());
        foreach (var enemy in enemies)
        {
            healthScripts.Add(enemy.GetComponent<HealthScript>());
            enemy.GetComponent<actionInventory>().newTurn();
        }
        player.GetComponent<actionInventory>().newTurn();
        BattleManager.endTurn();
        if (BattleManager.won)
        {
            return;
        }
        slotsList.Clear();
        slotsObjectsList.Clear();
        foreach (var script in healthScripts)
        {
            if (script.alive == true)
            {
                foreach (var order in script.speeds)
                {
                    slotsList.Add(new slotClass(order, script, script.gameObject));
                }
            }
        }

        slotsList.Sort((left, right) => right.speedOrder.CompareTo(left.speedOrder));


        foreach (var slot in slotsList)
        {
            GameObject currentSlot = Instantiate(slotPrefab, transform.position, transform.rotation, gameObject.transform);
            SlotScript slotScript = currentSlot.GetComponent<SlotScript>();
            slotsObjectsList.Add(currentSlot);
            slotScript.speed = slot.speedOrder;
            slotScript.unitTeam = slot.healthScript.team;
            if (slotScript.unitTeam == 1)
            {
                slotScript.user = slot.user;
            }
        }
    }

    public void startTurn() 
    {
        if (turnInProcess)
        {
            return;
        }
        turnInProcess = true;
        actionCount = 0;
        Invoke("doAction", coolDownAction);
    }
    public void doAction() 
    {
        
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<HealthScript>().alive == true)
            {
                aliveEnemies.Add(enemy);
            }
            else
            {
                aliveEnemies.Remove(enemy);
            }
        }
        if (actionCount != 0)
        {
            slotsObjectsList[actionCount - 1].GetComponent<SlotScript>().highLightShow(false);
        }
        if (slotsObjectsList.Count <= actionCount)
        {
            turnInProcess = false;
            foreach (var slot in slotsObjectsList)
            {
                Destroy(slot);
            }
            GameObject[] actions = GameObject.FindGameObjectsWithTag("action");
            foreach (var action in actions) 
            {
                Destroy(action);
            }
            GameObject[] holders = GameObject.FindGameObjectsWithTag("actionHolder");
            foreach (var holder in holders)
            {
                Destroy(holder);
            }
            newTurn();

            return; 
        }
        slotsObjectsList[actionCount].GetComponent<SlotScript>().highLightShow(true);
        SlotScript script = slotsObjectsList[actionCount].GetComponent< SlotScript>();
        if (script.actionHolderScript.heldSlot != null)
        {

            actionPrefabScript action = script.actionHolderScript.heldSlot.GetComponent<actionPrefabScript>();
            if (action.target != null)
            {
                actionsMethods.doAction(action._name, action.power, action.inventoryOrderCount, action.target, action.user);
            }
            else
            {
                if (slotsObjectsList[actionCount].GetComponent<SlotScript>().unitTeam == 0)
                {
                    actionsMethods.doAction(action._name, action.power, action.inventoryOrderCount, player, action.user);
                }
                else
                {
                    actionsMethods.doAction(action._name, action.power, action.inventoryOrderCount, aliveEnemies[Random.Range(0, aliveEnemies.Count)], action.user);
                }

            }
        }
        Invoke("doAction", coolDownAction);
        actionCount++;
    }
    class slotClass
    {
        public int speedOrder;
        public HealthScript healthScript;
        public GameObject user;

        public slotClass(int _speedOrder, HealthScript _healthScript, GameObject _user) 
        {
            speedOrder = _speedOrder;
            healthScript = _healthScript;
            user = _user;
        }
    }
    
}

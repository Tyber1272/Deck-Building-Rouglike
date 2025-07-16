using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    GameObject player;
    GameObject[] enemies;
    List<HealthScript> healthScripts = new List<HealthScript>();
    List<slotClass> slotsList = new List<slotClass>();
    List<GameObject> slotsObjectsList = new List<GameObject>();
    [SerializeField] GameObject slotPrefab;
    actionsMethods actionsMethods;
    int actionCount;
    public float coolDownAction;
    public bool turnInProcess;
    void Start()
    {
        actionsMethods = GameObject.FindGameObjectWithTag("actionsMethods").GetComponent<actionsMethods>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
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
        }
        slotsList.Clear();
        slotsObjectsList.Clear();
        foreach (var script in healthScripts)
        {
            foreach (var order in script.speeds)
            {
                slotsList.Add(new slotClass(order, script));
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
        if (actionCount != 0)
        {
            slotsObjectsList[actionCount - 1].GetComponent<SlotScript>().highLightShow(false);
        }
        if (slotsObjectsList.Count <= actionCount)
        {
            turnInProcess = false;


            return; 
        }
        slotsObjectsList[actionCount].GetComponent<SlotScript>().highLightShow(true);
        SlotScript script = slotsObjectsList[actionCount].GetComponent< SlotScript>();
        if (script.actionHolderScript.heldSlot != null)
        {
            actionPrefabScript action = script.actionHolderScript.heldSlot.GetComponent<actionPrefabScript>();
            if (action.target != null)
            {
                actionsMethods.doAction(action._name, action.power, action.target);
            }
            else
            {
                if (slotsObjectsList[actionCount].GetComponent<SlotScript>().unitTeam == 0)
                {
                    actionsMethods.doAction(action._name, action.power, player);
                }
                else
                {
                    actionsMethods.doAction(action._name, action.power, enemies[Random.Range(0, enemies.Length)]);
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

        public slotClass(int _speedOrder, HealthScript _healthScript) 
        {
            speedOrder = _speedOrder;
            healthScript = _healthScript;
        }
    }
    
}

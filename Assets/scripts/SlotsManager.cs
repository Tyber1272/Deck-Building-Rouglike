using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    GameObject player;
    GameObject[] enemies;
    List<HealthScript> healthScripts = new List<HealthScript>();
    List<slotClass> slotsList = new List<slotClass>();
    [SerializeField] GameObject slotPrefab;
    void Start()
    {
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

                slotScript.speed = slot.speedOrder;
                slotScript.unitTeam = slot.healthScript.team;
        }
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

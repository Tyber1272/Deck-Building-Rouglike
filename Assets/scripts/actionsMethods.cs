using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsMethods : MonoBehaviour
{
    [SerializeField] GameObject[] effects;
    public void doAction(string name, float power, GameObject target) 
    {
        switch (name)
        {
            case "strike":
                strike(power, target);
                break;
            case "defend":
                defend(power, target);
                break;
            case "heal":
                heal(power, target);
                break;
        }
    }

    void strike(float power, GameObject target) 
    {
        print("attack");
        target.GetComponent<HealthScript>().getDamage(power);
        Instantiate(effects[0], target.transform.position, transform.rotation);
    }

    void defend(float power, GameObject target)
    {
        print("block");
        target.GetComponent<HealthScript>().changeBlock(power);
        Instantiate(effects[1], target.transform.position, transform.rotation);
    }

    void heal(float power, GameObject target) 
    {
        print("heal");
        target.GetComponent<HealthScript>().changeHealth(power);
        Instantiate(effects[2], target.transform.position, transform.rotation);
    }
}

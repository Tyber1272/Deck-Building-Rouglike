using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsMethods : MonoBehaviour
{
    [SerializeField] GameObject[] effects;
    public void doAction(string name, float power,int order, GameObject target, GameObject user) 
    {
        if (user.GetComponent<HealthScript>().alive == false || target.GetComponent<HealthScript>().alive == false)
        {
            return;
        }

        user.GetComponent<actionInventory>().actionUsed(order);

        switch (name)
        {
            case "strike":
                strike(power, target, user);
                break;
            case "defend":
                defend(power, target, user);
                break;
            case "heal":
                heal(power, target, user);
                break;
        }
    }

    void strike(float power, GameObject target, GameObject user) 
    {
        print("attack");
        target.GetComponent<HealthScript>().getDamage(power);
        Instantiate(effects[0], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shot", 0, target.transform);
    }

    void defend(float power, GameObject target, GameObject user)
    {
        print("block");
        target.GetComponent<HealthScript>().changeBlock(power);
        Instantiate(effects[1], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shotUp", 1, target.transform);
    }

    void heal(float power, GameObject target, GameObject user) 
    {
        print("heal");
        target.GetComponent<HealthScript>().changeHealth(power);
        Instantiate(effects[2], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shotUp", 2, target.transform);
    }



}

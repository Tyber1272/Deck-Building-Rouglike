using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsMethods : MonoBehaviour
{
    public void doAction(string name, float power, GameObject target) 
    {
        print(name);
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
        target.GetComponent<HealthScript>().changeHealth(-power);
    }

    void defend(float power, GameObject target)
    {
        print("block");
        //blok
    }

    void heal(float power, GameObject target) 
    {
        print("heal");
        target.GetComponent<HealthScript>().changeHealth(power);
    }
}

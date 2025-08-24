using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsMethods : MonoBehaviour
{
    [SerializeField] GameObject[] effects;
    string Name; float power; int order; GameObject target; GameObject user;
    buffsClass buffsClassScript;
    [SerializeField] AudioClip[] sfx; // blank, slash, defend, heal, poinson
    AudioClip audioToPlay;
    private void Start()
    {
        buffsClassScript = GameObject.FindGameObjectWithTag("buffsClass").GetComponent<buffsClass>();
    }
    public void doAction(string name_, float power_, int order_, GameObject target_, GameObject user_) 
    {
        if (user_.GetComponent<HealthScript>().alive == false || target_.GetComponent<HealthScript>().alive == false)
        {
            return;
        }
        Name = name_; power = power_; order = order_; target = target_; user = user_;
        user.GetComponent<actionInventory>().actionUsed(order);
        switch (Name)
        {
            case "strike":
                strike();
                break;
            case "defend":
                defend();
                break;
            case "heal":
                heal();
                break;
            case "poison":
                poison();
                break;
        }
        AudioSource.PlayClipAtPoint(audioToPlay, target.transform.position);

    }

    void strike()
    {
        print("attack");
        target.GetComponent<HealthScript>().getDamage(power);
        Instantiate(effects[0], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shot", target.transform);
        audioToPlay = sfx[1];
    }

    void defend()
    {
        print("block");
        target.GetComponent<HealthScript>().changeBlock(power);
        Instantiate(effects[1], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shotUp", target.transform);
        audioToPlay = sfx[2];
    }

    void heal() 
    {
        print("heal");
        target.GetComponent<HealthScript>().changeHealth(power);
        Instantiate(effects[2], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shotUp", target.transform);
        audioToPlay = sfx[3];
    }

    void poison()
    {
        print("poison");
        target.GetComponent<HealthScript>().addBuff(buffsClassScript.poison, power, (int)power);
        Instantiate(effects[3], target.transform.position, transform.rotation);
        user.GetComponent<HealthScript>().triggerAnimation("shot", target.transform);
        audioToPlay = sfx[4];
    }

}

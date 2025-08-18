using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffsClass : MonoBehaviour
{
    public class existingBuffs
    {
        public string name;
        public bool debuff;
        public bool stackable;
        public existingBuffs(string _name, bool debuff_, bool stackable)
        {
            name = _name;
            debuff = debuff_;
            this.stackable = stackable;
        }
    }
    public existingBuffs poison = new existingBuffs("poison", true, true);

    public class buff
    {
        public existingBuffs buffType;
        public string name;
        public float stack;
        public int turnsLeft;

        public buff(existingBuffs buffType_, string name_, float stack_, int turnsLeft_)
        {
            buffType = buffType_;
            name = name_;
            stack = stack_;
            turnsLeft = turnsLeft_;
        }
    }
    public void newTurnForUnit(GameObject unit, HealthScript healthScript) // ka¿da jednoska podczas nowej tury wysy³a tutaj swój skrypt a ta komenda robi swoje
    {
        foreach (var buff in healthScript.unitBuffs)
        {
            switch (buff.name) 
            {
                case "poison":
                    healthScript.getDamage(buff.stack);
                    buff.stack = buff.stack - 1;
                    buff.turnsLeft = (int)buff.stack + 1;     // +1 jest po to aby sumowa³o siê z --- buff.turnsLeft -= 1; ---
                    break;
            }

            buff.turnsLeft -= 1;
            if (buff.turnsLeft == 0)
            {
                healthScript.removeBuff(healthScript.unitBuffs.IndexOf(buff));
            }
            else
            {
                healthScript.buffsPrefabsList[healthScript.unitBuffs.IndexOf(buff)].GetComponent<buffPrefabScript>().setStats(buff.name, buff.stack);
            }
        }
    }
}

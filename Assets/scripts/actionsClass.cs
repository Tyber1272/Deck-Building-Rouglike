using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsClass : MonoBehaviour
{
    public class action 
    {
        public string name;
        public float power;
        public int coolDown;
        public int tier;
        public int inventoryOrder;
        public action(string _name, float _power, int _coolDown) 
        {
            name = _name;
            power = _power;
            coolDown = _coolDown;
            tier = 0;
        }
    }
}

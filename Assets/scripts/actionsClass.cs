using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionsClass : MonoBehaviour
{
    public class action 
    {
        public string name;
        public float power;

        public action(string _name, float _power) 
        {
            name = _name;
            power = _power;
        }
    }
}

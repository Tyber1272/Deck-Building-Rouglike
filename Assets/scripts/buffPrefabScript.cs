using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buffPrefabScript : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Text stack; 
    public void setStats(string name_, float stack_) 
    {
        name.text = name_;
        stack.text = stack_.ToString();
    }
}

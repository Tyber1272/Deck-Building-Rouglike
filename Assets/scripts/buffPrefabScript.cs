using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buffPrefabScript : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text stack;
    int turnsLeft;
    string isDebuffString;
    [SerializeField] GameObject stackObject;
    buffsClass.existingBuffs buffClass;

    infoBoxScript infoBox;

    [SerializeField] Sprite[] icons;  // poison, 
    [SerializeField] Color[] colors;
    private void Start()
    {
        infoBox = GameObject.FindGameObjectWithTag("infoBox").GetComponent<infoBoxScript>();
    }
    public void setStats(string name_, float stack_, bool enemy, buffsClass.existingBuffs class_, int turnsLeft_)  
    {
        buffClass = class_;
        turnsLeft = turnsLeft_;
        int colorIndex = 0;
        switch (name_) 
        {
            case ("poison"):
                colorIndex = 0;
                break;
        }
        if (buffClass.debuff == true)
        {
            isDebuffString = "Debuff";
        }
        else
        {
            isDebuffString = "Buff";
        }
        icon.sprite = icons[colorIndex];
        icon.color = colors[colorIndex];
        stack.text = stack_.ToString();

        
        if (enemy != true)
        {
            stackObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            stackObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
    
    private void OnMouseOver()
    {
        infoBox.showInfo(buffClass.description,
            $"{buffClass.name} \n" +
            $"{isDebuffString} \n" +
            $"Stack: {stack.text} \n" +
            $"Turns left: {turnsLeft} ",
            gameObject);
    }
    
}

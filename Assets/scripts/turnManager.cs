using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManager : MonoBehaviour
{
    GameObject[] slots;
    void Start()
    {
        newTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void newTurn() 
    {
        slots = GameObject.FindGameObjectsWithTag("slot");
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class actionPrefabScript : MonoBehaviour
{
    public Text nameText;
    public Text powerText;
    public Image Image;

    [SerializeField] GameObject holderPrefab;
    //[SerializeField] GameObject Canvas;

    [SerializeField] Camera mainCam;
    [SerializeField] float slotPlaceDistance;
    GameObject[] actionHolders;
    GameObject currentSlot;
    bool isTouchingMouse;
    bool inRange;
    GameObject targetHolder = null;

    private void Start()
    {
        mainCam = Camera.main;
        actionHolders = GameObject.FindGameObjectsWithTag("actionHolder");
        Invoke("Drop", 0.02f);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Drop();
        }
    }
    public void setStats(string name, float power, GameObject slot) 
    {
        nameText.text = name;
        powerText.text = power.ToString();
        currentSlot = slot;

        switch (name)
        {
            case "strike":
                Image.color = Color.red;
                break;
            case "defend":
                Image.color = Color.blue;
                break;
            case "heal": 
                Image.color = Color.green;
                break;
        }  
        
        
    }

    public void Drag()
    {
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        transform.position = Input.mousePosition;
        transform.SetAsLastSibling();
        float bestDistance = 1000;
        float distance;
        foreach (var actionHolder in actionHolders)
        {
            distance = Vector2.Distance(transform.position, actionHolder.transform.position);
            actionHolderScript script = actionHolder.GetComponent<actionHolderScript>();
            script.slotInRange(false);
            if (distance < bestDistance && (script.heldSlot == null || script.heldSlot == gameObject) && script.player == true && distance <= slotPlaceDistance)
            {
                if (targetHolder != actionHolder)
                {
                    foreach (var _actionHolder in actionHolders)
                    {
                        actionHolderScript _script = _actionHolder.GetComponent<actionHolderScript>();
                        _script.slotInRange(false);
                    }
                }
                bestDistance = distance;
                targetHolder = actionHolder;
                script.slotInRange(true);
                inRange = true;
            }

        }
    }
    public void Drop() 
    {


        //float bestDistance = 1000;
        //float distance;
        //GameObject targetHolder = null;
        //foreach (var actionHolder in actionHolders) 
        //{
        //    distance = Vector2.Distance(transform.position, actionHolder.transform.position);
        //    actionHolderScript script = actionHolder.GetComponent<actionHolderScript>();
        //    if (distance < bestDistance && (script.heldSlot == null || script.heldSlot == gameObject) && script.player == true && distance <= slotPlaceDistance)
        //    {
        //        bestDistance = distance;
        //        targetHolder = actionHolder;
        //    }
        //}

        
        if (targetHolder != null)
        {
            if (Vector2.Distance(transform.position, targetHolder.transform.position) <= slotPlaceDistance)
            {
                inRange = true;
            }
            else
            {
                inRange = false;
            }
            if (inRange)
            {
                targetHolder.GetComponent<actionHolderScript>().slotInRange(false);
                transform.position = targetHolder.transform.position;
                if (currentSlot != null)
                {
                    currentSlot.GetComponent<actionHolderScript>().heldSlot = null;
                }
                currentSlot = targetHolder;
                targetHolder.GetComponent<actionHolderScript>().heldSlot = gameObject;
            }
            else
            {
                transform.position = currentSlot.transform.position;
            }
        }
        else
        {
            transform.position = currentSlot.transform.position;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEditor.Build;


public class actionPrefabScript : MonoBehaviour
{
    public Text nameText;
    public Text powerText;
    public Image Image;

    public string _name;
    public float power;
    public GameObject target;
    public GameObject user;
    bool canHaveTarget;


    [SerializeField] GameObject holderPrefab;
    //[SerializeField] GameObject Canvas;

    [SerializeField] Camera mainCam;
    [SerializeField] float slotPlaceDistance;
    [SerializeField] GameObject diseableObject;
    GameObject[] enemies;
    GameObject[] actionHolders;
    GameObject currentSlot;
    bool isHeld;
    bool inRange;
    GameObject targetHolder = null;
    SlotsManager slotsManager;
    Vector3 baseSize;
    [SerializeField] Vector3 popUpSize;

    private void Start()
    {
        baseSize = transform.localScale;
        slotsManager = GameObject.FindGameObjectWithTag("slotManager").GetComponent<SlotsManager>();
        mainCam = Camera.main;
        actionHolders = GameObject.FindGameObjectsWithTag("actionHolder");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        Invoke("Drop", 0.02f);
        if (canHaveTarget && enemies.Length > 0)
        {
            target = enemies[0];
        }
        if (canHaveTarget && user.tag == "enemy")
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
    }
    private void Update()
    {
        if (isHeld)
        {
            enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponent<HealthScript>().mouseOver == true && Input.GetMouseButtonDown(1) && canHaveTarget)
                {
                    target = enemy;
                    showTargetIcon();
                }
            }
        }
        if (user != null)
        {
            if (user.GetComponent<HealthScript>().alive == false)
            {
                diseableObject.SetActive(true);    
            }
        }
        if (target != null && user.CompareTag("Player") && enemies.Length > 0)
        {
            if (target.GetComponent<HealthScript>().alive == false)
            {
                target = enemies[Random.Range(0, enemies.Length)];
            }
        }
    }
    public void setStats(string name, float _power, GameObject slot, GameObject _user) 
    {
        nameText.text = name;
        powerText.text = _power.ToString();
        currentSlot = slot;
        currentSlot.GetComponent<actionHolderScript>().heldSlot = gameObject;
        user = _user;
        
        _name = name;
        power = _power;

        switch (name)
        {
            case "strike":
                Image.color = Color.red;
                canHaveTarget = true;
                break;
            case "defend":
                Image.color = Color.blue;
                canHaveTarget = false;
                break;
            case "heal": 
                Image.color = Color.green;
                canHaveTarget = false;
                break;
        }  
        
        
    }
    void showTargetIcon() 
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (var enemy in enemies)
        {
            if (target != enemy)
            {
                enemy.GetComponent<HealthScript>().showTargetIcon(false);
            }
            else if (enemy == target)
            {
                enemy.GetComponent<HealthScript>().showTargetIcon(true);
            }
            if (enemy == user)
            {
                enemy.GetComponent<HealthScript>().showHighLight(true);
            }
            else if(enemy != user)
            {
                enemy.GetComponent<HealthScript>().showHighLight(false);
            }
        }

    }
    public void mouseOver()
    {
        showTargetIcon();
    }
    public void mouseExit()
    {
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<HealthScript>().showTargetIcon(false);
        }
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<HealthScript>().showHighLight(false);
        }
    }
    public void Drag()
    {
        if (!Input.GetMouseButton(0) || slotsManager.turnInProcess || user.tag == "enemy")
        {
                return;
        }
        isHeld = true;
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
        if (Input.GetMouseButton(0))
        {
            return;
        }
        isHeld = false;
        if (targetHolder != null)
        {
            foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
            {
                enemy.GetComponent<HealthScript>().showTargetIcon(false);
            }

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

    public void popUp(bool bigger) 
    {
        if (bigger)
        {
            transform.localScale = baseSize + popUpSize;
        }
        else 
        {
            transform.localScale = baseSize;
        }
    }
}

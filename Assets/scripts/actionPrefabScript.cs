using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Rendering;


public class actionPrefabScript : MonoBehaviour
{
    public Image icon;
    public Text powerText;
    public Image Image;
    public GameObject coolDownIcon; public Text coolDownText;
    [SerializeField] GameObject[] iconsList;
    public Color[] bulletsColors;

    public string _name;
    public float power;
    int maxCooldown;
    public int tier;
    string tierString;
    string description;
    public bool coolDownReady = true;
    public GameObject target;
    public GameObject user;
    bool canHaveTarget;
    public int inventoryOrderCount;

    
    [SerializeField] GameObject holderPrefab;
    //[SerializeField] GameObject Canvas;

    [SerializeField] Image tierImage; [SerializeField] Color[] tiersColors;

    [SerializeField] Camera mainCam;
    infoBoxScript infoBox;
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
    battleManager battleManager;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        baseSize = transform.localScale;
        infoBox = GameObject.FindGameObjectWithTag("infoBox").GetComponent<infoBoxScript>();
        slotsManager = GameObject.FindGameObjectWithTag("slotManager").GetComponent<SlotsManager>();
        mainCam = Camera.main;
        actionHolders = GameObject.FindGameObjectsWithTag("actionHolder");
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        battleManager = GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>();
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
        else if (currentSlot != null)
        {
            transform.position = currentSlot.transform.position;
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
        if (GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>().won == true)//aktywuje to akcje nawet jak nie s¹ gotowe po wygranej
        {
            diseableObject.SetActive(false);
            coolDownReady = true;
            coolDownIcon.SetActive(false);
        }
    }
    public void setStats(string name, float _power, int _maxCooldown, GameObject slot, GameObject _user, int coolDown, int inventoryOrder, int _tier) 
    {
        powerText.text = _power.ToString();
        currentSlot = slot;
        currentSlot.GetComponent<actionHolderScript>().heldSlot = gameObject;
        user = _user;
        
        _name = name;
        power = _power;
        maxCooldown = _maxCooldown;
        tier = _tier;

        tierString = tier.ToString();
        if (tier == 3) // <<< Maksymalny tier tutaj
        {
            tierString = "MAX";
        }
        tierImage.color = tiersColors[tier];
        if (coolDown > 0 && GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>().won == false)
        {
            diseableObject.SetActive(true);
            coolDownReady = false;
            coolDownIcon.SetActive(true);
            coolDownText.text = coolDown.ToString();
        }
        else
        {
            coolDownReady = true;
        }
        inventoryOrderCount = inventoryOrder;
        foreach (var icon in iconsList)
        {
            icon.SetActive(false);
        }
        switch (name)
        {
            case "strike":
                setVariantVaribles(0, true);
                break;
            case "defend":
                setVariantVaribles(1, false);
                break;
            case "heal":
                setVariantVaribles(2, false);
                break;
            case "poison":
                setVariantVaribles(3, true);
                break;
        }  
        
        
    }
    void setVariantVaribles(int index, bool canTarget) 
    {
        iconsList[index].SetActive(true);
        Image.color = bulletsColors[index];
        canHaveTarget = canTarget;
        switch (index) 
        {
            case 0:
                description = $"Deals {power} damage to the target";
                break;
            case 1:
                description = $"The user gains {power} block";
                break;
            case 2:
                description = $"Heal the user for {power} HP";
                break;
            case 3:
                description = $"Apply {power} poison to the target";
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
        if (coolDownReady == true)
        {
            showTargetIcon();
            anim.SetBool("mouseOver", true);
        }
        if (tierString == "MAX")
        {
            battleManager.holdingMaxTier = true;
        }
        infoBox.showInfo(
            description,
            $"{_name} \n" +
            $"Power: {power} \n" +
            $"Cooldown: {maxCooldown} \n" +
            $"Tier: {tierString}"
            , gameObject
             );
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
        anim.SetBool("mouseOver", false);
        battleManager.holdingMaxTier = false;
    }
    public void Drag()
    {
        if (!Input.GetMouseButton(0) || slotsManager.turnInProcess || user.tag != "Player" || coolDownReady == false)
        {
                return;
        }
        actionHolders = GameObject.FindGameObjectsWithTag("actionHolder");
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
                if ((tierString != "MAX" && script.upgreadeSlot == true) || script.upgreadeSlot == false)
                {
                    if (targetHolder != actionHolder)
                    {
                        foreach (var _actionHolder in actionHolders)
                        {
                            actionHolderScript _script = _actionHolder.GetComponent<actionHolderScript>();
                            _script.slotInRange(false);
                        }

                        bestDistance = distance;
                        targetHolder = actionHolder;
                        script.slotInRange(true);
                        inRange = true;
                    }
                }
            }

        }

        anim.SetBool("Grabing", true);

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
        else if (currentSlot != null)
        {
            transform.position = currentSlot.transform.position;
        }
        anim.SetBool("Grabing", false);
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

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class rewardScript : MonoBehaviour
{
    public enum rewardTypes 
    {
        Action,
        Upgrade,
        MaxHealth,

        // EXP, actions upgrade, backpack size
    }
    public rewardTypes rewardType; public string stringRewardType;

    battleManager battleManager;

    [SerializeField] GameObject actionObject, healthObject, upgradObject;

    public GameObject actionPrefab;
    public actionsClass.action actionReward;
    [SerializeField] Text powerText, cooldownText;
    [SerializeField] GameObject[] icons;
    [SerializeField] Image imageBullet;

    public float amount;

    [SerializeField] Text healthAmount;

    [SerializeField] Text upgradeAmount;
    public actionHolderScript actionHolder;
    GameObject heldAction = null;

    [SerializeField] GameObject selectObject;

    [SerializeField] GameObject denyObject;


    void Start()
    {
        battleManager = GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>();
        actionInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<actionInventory>();
        int rndNum = Random.Range(0, 4);
        rewardType = (rewardTypes)Random.Range(0, 3);
        if (playerInventory.unitActions.Count >= playerInventory.maxInventorySpace && rewardType == rewardTypes.Action)
        {
            rewardType = rewardTypes.MaxHealth;
        }
        stringRewardType = rewardType.ToString();
        print(stringRewardType);
        if (rewardType == rewardTypes.Action)
        {
            actionObject.SetActive(true);
            actionReward = battleManager.possiblesActionsRewards[Random.Range(0, battleManager.possiblesActionsRewards.Count)];
            actionReward.power = actionReward.power + battleManager.gameManager.encounterCount + Random.Range(-2, 6);
            powerText.text = actionReward.power.ToString();
            cooldownText.text = actionReward.coolDown.ToString();
            foreach (var icon in icons) 
            {
                icon.SetActive(false);
            }
            switch (actionReward.name)
            {
                case "strike":
                    imageBullet.color = actionPrefab.GetComponent<actionPrefabScript>().bulletsColors[0];
                    icons[0].SetActive(true);
                    break;
                case "defend":
                    imageBullet.color = actionPrefab.GetComponent<actionPrefabScript>().bulletsColors[1];
                    icons[1].SetActive(true);
                    break;
                case "heal":
                    imageBullet.color = actionPrefab.GetComponent<actionPrefabScript>().bulletsColors[2];
                    icons[2].SetActive(true);
                    break;
                case "poison":
                    imageBullet.color = actionPrefab.GetComponent<actionPrefabScript>().bulletsColors[3];
                    icons[3].SetActive(true);
                    break;
            }
        }
        if (rewardType == rewardTypes.MaxHealth)
        {
            healthObject.SetActive(true);
            amount = 6 + battleManager.gameManager.encounterCount + Random.Range(-5, 6);
            healthAmount.text = "+" + amount.ToString();
        }
        if (rewardType == rewardTypes.Upgrade) 
        {
            upgradObject.SetActive(true);
            amount = battleManager.gameManager.encounterCount + Random.Range(1, 5);
            upgradeAmount.text = "+" + amount.ToString();
        }
    }
    //private void OnMouseOver()
    //{
    //    infoBox.showInfo(
    //        actionReward.,
    //        $"{actionReward.name} \n" +
    //        $"Power: {actionReward.power} \n" +
    //        $"Cooldown: {actionReward.coolDown} \n"
    //        , gameObject
    //         );
    //}

    void Update()
    {
        if (battleManager.selectedReward == this)
        {
            selectObject.SetActive(true);
        }
        else
        {
            selectObject.SetActive(false);
        }

        if (actionHolder.heldSlot != heldAction && actionHolder.heldSlot != null)
        {
            heldAction = actionHolder.heldSlot;
            select();
        }
        if (actionHolder.heldSlot == null)
        {
            heldAction = null;
        }
        if (battleManager.holdingMaxTier == true) 
        {
            denyObject.SetActive(true);
        }
        else
        {
            denyObject.SetActive(false);
        }
    }
  
    public void select()
    {
        battleManager.selectedReward = this;
    }
    
}

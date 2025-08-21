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

    battleManager battleMagaer;

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

    infoBoxScript infoBox;

    void Start()
    {
        battleMagaer = GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>();
        int rndNum = Random.Range(0, 4);
        rewardType = (rewardTypes)Random.Range(0, 3);
        stringRewardType = rewardType.ToString();
        print(stringRewardType);
        if (rewardType == rewardTypes.Action)
        {
            actionObject.SetActive(true);
            actionReward = battleMagaer.possiblesActionsRewards[Random.Range(0, battleMagaer.possiblesActionsRewards.Count)];
            actionReward.power = actionReward.power + battleMagaer.gameManager.encounterCount + Random.Range(-2, 6);
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
            amount = 2 + battleMagaer.gameManager.encounterCount + Random.Range(-2, 4);
            healthAmount.text = "+" + amount.ToString();
        }
        if (rewardType == rewardTypes.Upgrade) 
        {
            upgradObject.SetActive(true);
            amount = battleMagaer.gameManager.encounterCount + Random.Range(1, 4);
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
        if (battleMagaer.selectedReward == this)
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
    }

    public void select()
    {
        battleMagaer.selectedReward = this;
    }
    
}

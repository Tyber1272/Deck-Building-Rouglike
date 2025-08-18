using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rewardScript : MonoBehaviour
{
    public enum rewardTypes 
    {
        None,
        Action,
        Health,
        MaxHealth,

        // EXP, actions upgrade, backpack size
    }
    public rewardTypes rewardType; public string stringRewardType;

    public GameObject actionPrefab;
    public actionsClass.action actionReward;
    battleManager battleMagaer;

    [SerializeField] Text powerText, cooldownText;
    [SerializeField] GameObject[] icons;
    [SerializeField] Image imageBullet;
    
    [SerializeField] GameObject selectObject;

    void Start()
    {
        battleMagaer = GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>();
        rewardType = rewardTypes.Action;
        stringRewardType = rewardType.ToString();   
        if (rewardType == rewardTypes.Action)
        {
            actionReward = battleMagaer.possiblesActionsRewards[Random.Range(0, battleMagaer.possiblesActionsRewards.Count)];
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
    }

    
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
    }

    public void select()
    {
        battleMagaer.selectedReward = this;
    }
    
}

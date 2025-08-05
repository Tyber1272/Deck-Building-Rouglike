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

    [SerializeField] Text nameText, powerText, cooldownText;
    [SerializeField] Image imageIcon;
    [SerializeField] GameObject selectObject;

    void Start()
    {
        battleMagaer = GameObject.FindGameObjectWithTag("battleManager").GetComponent<battleManager>();
        rewardType = rewardTypes.Action;
        stringRewardType = rewardType.ToString();   
        if (rewardType == rewardTypes.Action)
        {
            actionReward = battleMagaer.possiblesActionsRewards[Random.Range(0, battleMagaer.possiblesActionsRewards.Count)];
            nameText.text = actionReward.name; 
            powerText.text = actionReward.power.ToString();
            cooldownText.text = actionReward.coolDown.ToString();
            switch (actionReward.name)
            {
                case "strike":
                    imageIcon.color = Color.red;
                    break;
                case "defend":
                    imageIcon.color = Color.blue;
                    break;
                case "heal":
                    imageIcon.color = Color.green;
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

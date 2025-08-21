using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class battleManager : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> aliveEnemies = new List<GameObject>();
    public bool won = false;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject rewardPrefab, rewardsHolder;
    [SerializeField] GameObject actionPrefab;

    public List<actionsClass.action> possiblesActionsRewards = new List<actionsClass.action>();
    public rewardScript selectedReward;
    public bool holdingMaxTier;

    GameObject player;
    public GameManager gameManager;

    void Start()
    {
        won = false;
        winMenu.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = player.GetComponent<GameManager>();

        possiblesActionsRewards.Add(new actionsClass.action("strike", 7, 0));
        possiblesActionsRewards.Add(new actionsClass.action("strike", 10, Random.Range(0, 2)));
        possiblesActionsRewards.Add(new actionsClass.action("defend", 8, 0));
        possiblesActionsRewards.Add(new actionsClass.action("defend", 13, Random.Range(0, 3)));
        possiblesActionsRewards.Add(new actionsClass.action("heal", 10, Random.Range(1, 3)));
        possiblesActionsRewards.Add(new actionsClass.action("poison", 5, Random.Range(1, 3)));
        foreach (var action in possiblesActionsRewards)
        {
            action.power = action.power + gameManager.encounterCount + Random.Range(-2, 6);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void nextBattle() 
    {
        if (selectedReward != null)
        {
            if (selectedReward.stringRewardType == "MaxHealth")
            {
                player.GetComponent<HealthScript>().increaseMaxHealth(selectedReward.amount);
            }
            else if (selectedReward.stringRewardType == "Action")
            {
                player.GetComponent<actionInventory>().addAction(selectedReward.actionReward);
            }
            else if (selectedReward.stringRewardType == "Upgrade" && selectedReward.actionHolder.heldSlot != null)
            {
                actionPrefabScript script = selectedReward.actionHolder.heldSlot.GetComponent<actionPrefabScript>();
                player.GetComponent<actionInventory>().unitActions[script.inventoryOrderCount].power += selectedReward.amount;
                player.GetComponent<actionInventory>().unitActions[script.inventoryOrderCount].tier += 1;
            }
                
        }
        SceneManager.LoadScene(0);
        player.GetComponent<actionInventory>().newBattle();
    }
    public void endTurn() 
    {
        Invoke("delayedEndTurn", 0.1f);
    }
    void delayedEndTurn() 
    {
        enemies.Clear();
        aliveEnemies.Clear();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(enemy);
            if (enemy.GetComponent<HealthScript>().alive == true)
            {
                aliveEnemies.Add(enemy);
            }
        }
        if (aliveEnemies.Count == 0)
        {
            win();
        }
    }
    void win() 
    {
        won = true;
        winMenu.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            GameObject currentReward = Instantiate(rewardPrefab, transform.position, transform.rotation, rewardsHolder.transform);
            rewardScript script = currentReward.GetComponent<rewardScript>();

            script.actionPrefab = actionPrefab;

        }
    }
    
}

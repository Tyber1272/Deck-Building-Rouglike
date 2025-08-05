using System.Collections;
using System.Collections.Generic;
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

    GameObject player;

    void Start()
    {
        won = false;
        winMenu.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");

        possiblesActionsRewards.Add(new actionsClass.action("strike", 8, 0));
        possiblesActionsRewards.Add(new actionsClass.action("defend", 9, 2));
        possiblesActionsRewards.Add(new actionsClass.action("(:", 666, 0));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void nextBattle() 
    {
        player.GetComponent<actionInventory>().addAction(selectedReward.actionReward);
        SceneManager.LoadScene(0);
        player.GetComponent<actionInventory>().newBattle();
    }
    public void endTurn() 
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

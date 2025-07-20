using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleManager : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    List<GameObject> aliveEnemies = new List<GameObject>();
    public bool won = false;
    [SerializeField] GameObject winMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class enemiesSpawnScript : MonoBehaviour
{
    [SerializeField] GameObject[] enemies; 
    [SerializeField] GameObject[] enemiesSpawnPoints;
    GameManager gameManager;
    void Start()
    {
        print(name);
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        if (gameManager.encounterCount == 0)
        {
            Instantiate(enemies[0], enemiesSpawnPoints[0].transform.position, transform.rotation);
        }
        else if (gameManager.encounterCount <= 2)
        {
            Instantiate(enemies[Random.Range(0, 2)], enemiesSpawnPoints[0].transform.position, transform.rotation);
        }
        else if (gameManager.encounterCount <= 5)
        {
            for (int i = 0; i < 2; i++)
            {
                Instantiate(enemies[Random.Range(0, 3)], enemiesSpawnPoints[i].transform.position, transform.rotation);
            }
        }
        else if (gameManager.encounterCount > 5)
        {
            for (int i = 0; i < 3; i++)
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], enemiesSpawnPoints[i].transform.position, transform.rotation);
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}



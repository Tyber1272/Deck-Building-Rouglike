using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float block;
    public int[] speeds;
    public int team; // player - 0, enemy - 1
    public bool alive = true;

    public bool mouseOver = false;

    [SerializeField] Text HPText;
    [SerializeField] Image HPImage;
    [SerializeField] Text blockText;
    [SerializeField] GameObject targetIcon; [SerializeField] GameObject highLight;
    void Start()
    {
        updateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getDamage(float amount) 
    {
        block = block - amount;
        if (block < 0)
        {
            changeHealth(block);
            block = 0;
        }
        updateStats();
    }
    public void changeHealth(float amount) 
    {
        Health = Health + amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Health <= 0)
        {
            die();   
        }
        updateStats();
    }
    public void changeBlock(float amount) 
    {
        block = block + amount;
        updateStats();
    }
    public void updateStats() 
    {
        HPText.text = Health + "/" + MaxHealth;
        HPImage.fillAmount = Health / MaxHealth;
        blockText.text = block.ToString();
        if (block <= 0)
        {
            blockText.text = "";
        }
    }
    public void die() 
    {
        alive = false;
        gameObject.SetActive(false);
    }
    public void showTargetIcon(bool show) 
    {
        if (show)
        {
            targetIcon.SetActive(true);
        }
        else 
        {
            targetIcon.SetActive(false);
        }
    }
    public void showHighLight(bool show)
    {
        if (show)
        {
            highLight.SetActive(true);
        }
        else
        {
            highLight.SetActive(false);
        }
    }
    private void OnMouseEnter()
    {
        mouseOver = true;

        GameObject[] actions = GameObject.FindGameObjectsWithTag("action");
        actionPrefabScript script;
        foreach (GameObject action in actions) 
        {
            script = action.GetComponent<actionPrefabScript>();
            if (script.user == gameObject)
            {
                script.popUp(true);
            }
            else if (script.user != gameObject)
            {
                script.popUp(false);
            }
        }
    }
    void OnMouseExit() 
    {
        mouseOver = false;

        GameObject[] actions = GameObject.FindGameObjectsWithTag("action");
        actionPrefabScript script;
        foreach (GameObject action in actions)
        {
            script = action.GetComponent<actionPrefabScript>();
            script.popUp(false);
        }
    }
}

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

    public bool mouseOver = false;

    [SerializeField] Text HPText;
    [SerializeField] Image HPImage;
    [SerializeField] Text blockText;
    [SerializeField] GameObject targetIcon;
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

    }
    public void changeBlock(float amount) 
    {
        block = block + amount;
        updateStats();
    }
    void updateStats() 
    {
        HPText.text = Health + "/" + MaxHealth;
        HPImage.fillAmount = Health / MaxHealth;
        blockText.text = block.ToString();
        if (block <= 0)
        {
            blockText.text = "";
        }
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
    private void OnMouseEnter()
    {
        mouseOver = true;
    }
    void OnMouseExit() 
    {
        mouseOver = false;
    }
}

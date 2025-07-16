using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public int[] speeds;
    public int team; // player - 0, enemy - 1

    public bool mouseOver = false;

    [SerializeField] Text HPText;
    [SerializeField] Image HPImage;
    [SerializeField] GameObject targetIcon;
    void Start()
    {
        HPText.text = Health + "/" + MaxHealth;
        HPImage.fillAmount = Health/MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeHealth(float amount) 
    {
        Health = Health + amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }

        HPText.text = Health + "/" + MaxHealth;
        HPImage.fillAmount = Health / MaxHealth;
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

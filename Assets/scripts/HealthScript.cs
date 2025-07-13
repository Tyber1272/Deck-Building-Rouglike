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

    [SerializeField] Text HPText;
    [SerializeField] Image HPImage;
    void Start()
    {
        HPText.text = Health + "/" + MaxHealth;
        HPImage.fillAmount = Health/MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    [SerializeField] Image Image;
    [SerializeField] Text speedText;

    public float speed;
    public Color[] teamsColors;
    public int unitTeam;
    void Start()
    {
        Image.color = teamsColors[unitTeam];
        speedText.text = speed.ToString();

        if (unitTeam == 0)
        {
            transform.GetComponentInChildren<actionHolderScript>().player = true;
        }
        else
        {
            transform.GetComponentInChildren<actionHolderScript>().player = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

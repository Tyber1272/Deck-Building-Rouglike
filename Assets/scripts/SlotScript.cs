using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    [SerializeField] Image Image;
    [SerializeField] Text speedText;
    [SerializeField] Image highLight;
    [SerializeField] Color[] highlightColors; //0 - active, 1 -unactive
    public actionHolderScript actionHolderScript;

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

        highLightShow(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void highLightShow(bool show) 
    {
        if (show) 
        {
            highLight.color = highlightColors[1];
        }
        else 
        {
            highLight.color = highlightColors[0];
        }
    }
}

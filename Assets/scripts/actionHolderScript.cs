using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class actionHolderScript : MonoBehaviour
{
    public GameObject heldSlot = null;
    public bool player = true;
    [SerializeField] Image image;
    [SerializeField] Color[] colors; // 0 - normal, 1 - highlight

    public void slotInRange(bool inRange) 
    {
        if (inRange)
        {
            image.color = colors[1];
        }
        else
        {
            image.color=colors[0];
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class infoBoxScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI StatsText;
    [SerializeField] float showDelay;
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("show", false);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            showBox();
        }
        else
        {
            hideBox();
        }
    }
    public void showInfo(string info, string stats, GameObject transform) 
    {
        descriptionText.text = info;
        StatsText.text = stats;
    }
    void showBox() 
    {
        anim.SetBool("show", true);
    }
    public void hideBox()
    {
        anim.SetBool("show", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonSFX : MonoBehaviour
{
    [SerializeField] AudioClip sfxHoverMouse;
    
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseEnter()
    {
        AudioSource.PlayClipAtPoint(sfxHoverMouse, new Vector3(0, 0, 0));
    }
}

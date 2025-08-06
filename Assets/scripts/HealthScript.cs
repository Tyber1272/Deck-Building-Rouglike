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

    [SerializeField] Animator anim;
    [SerializeField] Transform shotPoint;
    [SerializeField] GameObject[] shotTrailsEffects; // 0-attack, 1-block, 2-heal
    void Start()
    {
        updateStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void triggerAnimation(string name, int shotTrail, Transform shotTarget) 
    {
        if (anim == null)
            return;
        anim.SetTrigger(name);
        if (shotTrailsEffects.Length >= shotTrail && shotTarget != null)
        {
            GameObject a = Instantiate(shotTrailsEffects[shotTrail]);
            a.GetComponent<LineRenderer>().SetPosition(0, shotPoint.position);
            a.GetComponent<LineRenderer>().SetPosition(1, shotTarget.position);
        }
        
    }
    public void boolAnimation(string name, bool boolean)
    {
        if (anim == null)
            return;
        anim.SetBool(name, boolean);
    }
    public void getDamage(float amount) 
    {
        block = block - amount;
        if (block < 0)
        {
            changeHealth(block);
            block = 0;
        }
        triggerAnimation("hit", 69, null);
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

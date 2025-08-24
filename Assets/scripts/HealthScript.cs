using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    public float Health;
    public float MaxHealth;
    public float block;
    public int[] speeds;
    public int team; // player - 0, enemy - 1
    bool Player;
    public bool alive = true;
    public List<buffsClass.buff> unitBuffs = new List<buffsClass.buff>();

    public bool mouseOver = false;

    [SerializeField] Text HPText;
    [SerializeField] Image HPImage;
    [SerializeField] Text blockText; [SerializeField] GameObject blockIcon;
    [SerializeField] GameObject targetIcon; [SerializeField] GameObject highLight;
    [SerializeField] GameObject buffPrefab; [SerializeField] Transform buffsHolder;
    public List<GameObject> buffsPrefabsList = new List<GameObject>(); [SerializeField] buffsClass buffClassScript;

    [SerializeField] Animator anim;
    [SerializeField] Transform shotPoint;
    Animator blackScreenAnim;
    [SerializeField] AudioClip[] sfx; // 0-gunshot, 1-spin

    public int encouterCount;
    public float highestHP;
    public int killCount;

    public bool firstBattle = true;

    GameManager gameManager;
    void Start()
    {
        Player = team == 0;
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        highestHP = Health;
        unitBuffs.Clear();
        buffClassScript = GameObject.FindGameObjectWithTag("buffsClass").GetComponent<buffsClass>();
        updateStats();
        if (team == 1)
        {
            MaxHealth = MaxHealth + 5 + gameManager.encounterCount + (Random.Range(1, 10));
            Health = MaxHealth;

            //Increase speed by each battle
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void newBattle() 
    {
        firstBattle = false;
        boolAnimation("aim", false);
        buffClassScript = GameObject.FindGameObjectWithTag("buffsClass").GetComponent<buffsClass>();
        for (int i = 0; i < unitBuffs.Count; i++)
        {
            removeBuff(i);
        }
        gameObject.GetComponent<GameManager>().newBattle();
    }
    public void newTurn() 
    {
        if (unitBuffs.Count != 0)
        {
            buffClassScript.newTurnForUnit(gameObject, this);
        }
        block = 0; updateStats();
        boolAnimation("aim", false);
        if (team == 0)
        {
            AudioSource.PlayClipAtPoint(sfx[1], transform.position);
        }
    }
    public void triggerAnimation(string name, Transform shotTarget) 
    {
        if (anim == null)
        { return; }
        anim.SetTrigger(name);
        if (name == "shot")
        {
            AudioSource.PlayClipAtPoint(sfx[0], transform.position); 
        }
    }
    public void boolAnimation(string name, bool boolean)
    {
        if (anim == null) 
        { return; }
        anim.SetBool(name, boolean);
    }
    public void increaseMaxHealth(float amount) 
    { 
        MaxHealth = MaxHealth + amount;
        changeHealth(amount);
    }
    public void getDamage(float amount) 
    {
        block = block - amount;
        if (block < 0)
        {
            changeHealth(block);
            block = 0;
        }
        triggerAnimation("hit", null);
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
        if (Health > highestHP)
        {
            highestHP = Health;
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
            blockIcon.SetActive(false);
        }
        else
        {
            blockIcon.SetActive(true);
        }
    }
    public void addBuff(buffsClass.existingBuffs buffType, float stack, int turnsLeft) 
    {
        bool newBuff = true;
        int count = 0;
        foreach (var buff in unitBuffs)
        {
            if (buff.buffType.stackable == true)
            {
                buff.stack = buff.stack + stack;
                newBuff = false;
                buffsPrefabsList[count].GetComponent<buffPrefabScript>().setStats(buff.name, buff.stack, team == 0, buff.buffType, buff.turnsLeft);
            }
            count++;
        }
        if (newBuff == true)
        {
            unitBuffs.Add(new buffsClass.buff(buffType, buffType.name, stack, turnsLeft));
            updateBuff(unitBuffs.Count - 1, true);
        }
    }

    int indexForDelayBuff; // basicly zmienna po to tutaj na dole
    public void removeBuff(int indexOrder) 
    {
        indexForDelayBuff = indexOrder;
        Invoke("delayedRemoveBuff", 0.01f); // ma≥e opuünienie aby for each loop nie büikowa≥ w buffClass.
    }
    void delayedRemoveBuff() 
    {
        updateBuff(indexForDelayBuff, false);
        unitBuffs.Remove(unitBuffs[indexForDelayBuff]);
    }

    public void updateBuff(int indexOrder, bool add) 
    {
        buffsClass.buff buff = unitBuffs[indexOrder];
        if (add == true)
        {
            GameObject currectBuff = Instantiate(buffPrefab, transform.position, transform.rotation, buffsHolder);
            currectBuff.GetComponent<buffPrefabScript>().setStats(buff.name, buff.stack, team == 0, buff.buffType, buff.turnsLeft);
            buffsPrefabsList.Add(currectBuff);
        }
        else
        {
            Destroy(buffsPrefabsList[indexOrder]);
            buffsPrefabsList.Remove(buffsPrefabsList[indexOrder]);
        }
    }
    public void die() 
    {
        alive = false;
        boolAnimation("die", true);
        if (team == 1)
        {
            Invoke("setActiveOnFalse", 1f);
            GameObject.FindGameObjectWithTag("Player").GetComponent<HealthScript>().killCount++;
        }
        else
        {
            blackScreenAnim = GameObject.FindGameObjectWithTag("blackScreen").GetComponent<Animator>();
            print(blackScreenAnim);
            blackScreenAnim.SetTrigger("fadeIn");
            encouterCount = gameManager.encounterCount;
            Invoke("delayedGameOver", 3f);
        }
    }
    void delayedGameOver() 
    {
        SceneManager.LoadScene("game over");
    }
    void setActiveOnFalse() 
    {
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

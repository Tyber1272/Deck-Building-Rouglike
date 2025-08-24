using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enviromentElementsScript : MonoBehaviour
{
    [SerializeField] float randomXmin;
    [SerializeField] float randomXmax;
    [SerializeField] float randomZmin;
    [SerializeField] float randomZmax;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        int rndChance = Random.Range(0, 11);
        if (rndChance == 0)
        {
            Destroy(gameObject);       
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.position = new Vector3(Random.Range(randomXmin, randomXmax), transform.position.y, Random.Range(randomZmin, randomZmax));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

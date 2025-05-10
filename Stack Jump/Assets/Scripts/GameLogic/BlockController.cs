using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] private Color[] color = new Color[5];

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        byte randomColor = (byte)Random.Range(0, color.Length);
        spriteRenderer.color = color[randomColor];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedDeep : MonoBehaviour
{
    // Variable para actualizar la profundidad en cada fotograma
    public bool fixEveryFrame;
    SpriteRenderer spr;
    private void Awake()
    {
        
    }
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        spr.sortingLayerName = "Player";
        spr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    void Update()
    {
        if (fixEveryFrame)
        {
            spr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }
}

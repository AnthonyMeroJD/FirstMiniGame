using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRage : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPlayerOnRage;
    public bool GetIsPlayerOnRage() { return isPlayerOnRage; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") isPlayerOnRage = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") isPlayerOnRage = false;
    }
}

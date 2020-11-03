using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeArea : MonoBehaviour
{
    private Vector2 _dir;
    private GameObject _player;
    private Collider2D _cdPlayer;
    private Rigidbody2D _rbEnemy;
    private Vector3 _prePosition;
    public bool isPlayerToRange =false;
    private void Awake()
    {
        
        _player = GameObject.FindGameObjectWithTag("Player");
        _cdPlayer =_player.GetComponent<Collider2D>();
        _rbEnemy=GetComponent<CircleCollider2D>().GetComponentInParent<Rigidbody2D>();
   
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            isPlayerToRange = true;
        }

    }

    private void FixedUpdate()
    {
       
    }
 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerToRange = false;
        }


    }
}

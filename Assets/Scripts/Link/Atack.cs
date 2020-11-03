using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atack : MonoBehaviour
{
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
        if (collision.tag == "Enemy")
        {
            GameObject enemy= collision.gameObject;
            Animator colliderAnimator=collision.gameObject.GetComponent<Animator>();
            MovesEnemy rangeEnemy=enemy.GetComponent<MovesEnemy>();
                KamikasePatrol kamikaseEnemy= enemy.GetComponent<KamikasePatrol>();
            if (rangeEnemy != null) {
                rangeEnemy.health-=1; 
            } else if (kamikaseEnemy != null) {               
                colliderAnimator.Play(kamikaseEnemy.deadState);
               
            }

        }
    }
}

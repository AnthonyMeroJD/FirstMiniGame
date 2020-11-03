using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikasePatrol : MonoBehaviour
{

    //points to ward
    public Vector3[] targets;    
    private Vector3 target;
    //components
    private Rigidbody2D rb;
    
    private Animator animator;
    private bool arrive;
    private VisionRage vr;
    //atrib
    public float speed;
    public float damage;
    public string deadState;

    bool isDead=false;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vr = GetComponentInChildren<VisionRage>();
        
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        target = targets[1];
    }

    // Update is called once per frame
    
    void Update()
    {
        isDead = animator.GetCurrentAnimatorStateInfo(0).IsName(deadState);
        if (!isDead)
        {
            if (arrive && !vr.GetIsPlayerOnRage()) StartCoroutine(ActualizarTarget());


            if (vr.GetIsPlayerOnRage())
            {
                target = GameObject.FindGameObjectWithTag("Player").transform.position;
                speed = 7f;
            }

            float distance = Vector3.Distance(target, transform.position);

            if (distance < 0.02f)
            {
                arrive = true;
                transform.position = target;

            }
            else
            {
                Vector3 dir = target - transform.position;
                rb.MovePosition(transform.position + dir * speed * Time.deltaTime);
                arrive = false;
            }
        }
    }

    IEnumerator ActualizarTarget() {


        target = targets[Random.Range(0, targets.Length)];
        arrive = false;
        yield return null;                   
        
        
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {   

        if (collision.transform.tag == "Player") {
            GameObject player = collision.gameObject;            
            player.transform.GetComponent<AttriLink>().Damage(5);
            animator.Play(deadState);
            isDead = true;
            yield return new WaitForSeconds(2f);
        }
        
    }

    void DisableCollider()
    {
        
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }

    }

    void Dead() {
        Destroy(gameObject,2f);
    }

}

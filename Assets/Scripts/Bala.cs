using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bala : MonoBehaviour
    
{
    public float livingTime = 3f;
    public  float velocidad=1.3f;
    public float damage;
    public string deadState;
    //components
    private Rigidbody2D rb;
    private Collider2D cd;
    private Animator animator;
    private GameObject player;

    //targe to arrow and direccion
    private Vector3 target, dir;
    AnimatorStateInfo inf;
    bool isDestoying;
    private void Awake() {
        cd = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {

        Destroy(this.gameObject, livingTime);

        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform.position;
            dir = (player.transform.position - transform.position).normalized;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        inf = animator.GetCurrentAnimatorStateInfo(0);
        isDestoying = inf.IsName(deadState);
       
            
    }

    void FixedUpdate() {
        if (target != Vector3.zero && !isDestoying) {
            rb.MovePosition(transform.position + (dir * velocidad) * Time.deltaTime);
        }
        
    }

    IEnumerator OnTriggerEnter2D(Collider2D collider) {
        print(collider.gameObject);
        if (collider.transform.tag == "Player") {
            AttriLink atributos = collider.GetComponent<AttriLink>();
            atributos.Damage(damage);          
            animator.Play(deadState);            
            yield return new WaitForSeconds(0.5f);

        }
        if (collider.transform.tag == "Atacks")
        {            
            animator.Play(deadState);
            yield return new WaitForSeconds(0.5f);

        }

    }
    void OnBecameInvisible()
    {
        // Si se sale de la pantalla borramos el gameobject
        Destroy(this);
    }

    
}

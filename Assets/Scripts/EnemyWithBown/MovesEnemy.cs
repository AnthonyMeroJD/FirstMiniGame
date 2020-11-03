using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesEnemy : MonoBehaviour
{


    //atrb
    public float speed;
    public float visionRadius;
    public float atackRage;
    public float atackSpeed;
    public string deadState;
    public float health,maxHealth;

    
    //gameobjects
    public Transform player;
    public GameObject arrowPrefab;
    //components
    private Rigidbody2D _rb;
    private Animator _animator;
    private Transform _frontPoint;    
    private float distance;
    private Vector3 dir, initialPosition, target;

    private bool isKeep;
    private bool isAtack;
    private bool isPlayerRageVision;
    private bool isDead;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _frontPoint = transform.Find("FrontPoint");
      
    }
    // Start is called before the first frame update
    void Start()
    {

        initialPosition = transform.position;
        _animator.SetBool("IsKeep", true);
    }

    // Update is called once per frame
    void Update()
    {
        isDead = _animator.GetCurrentAnimatorStateInfo(0).IsName(deadState);
        isDead = health < 1;
        target = initialPosition;
        if (!isDead)
        {
          
            Vector3 direccion = player.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, visionRadius,
                1 << LayerMask.NameToLayer("Default")
                );

            //debufg raycast
            //param is direccion to look                
            Debug.DrawLine(transform.position, player.transform.position, Color.red);
            

            if (hit.collider != null)
            {
                if (player.GetComponent<AttriLink>().health > 0)
                {
                    if (hit.collider.tag == "Player")
                    {
                        Debug.Log("isPlayer");
                        isPlayerRageVision = true;
                        target = player.transform.position;
                    }
                    else isPlayerRageVision = false;


                }
                else isPlayerRageVision = false;


            }
          
            distance = Vector2.Distance(target, transform.position);
            dir = (target - transform.position).normalized;
        }
        else {
            _animator.Play(deadState);
        }

    }
    void DisableCollider()
    {
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }

    }
    IEnumerator DestroyEnemy(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    
    private void FixedUpdate()
    {

        //si esta llegando a la posicion inicial
        if (target == initialPosition && distance < 0.02f)
        {
            _rb.velocity = Vector2.zero;
            transform.position = initialPosition;
            isKeep = true;
            isAtack = false;
        }//si pierde de vista al player y el target es la posicion inicial
        else if (target == initialPosition && !isPlayerRageVision)
        {
            moveTo();
        }
    
        //si el target entra al campo de vision hasta el rtango de ataque se mueve
        if (target != initialPosition && (distance > atackRage && distance < visionRadius))
        {
            //aqui solo se mueve si esque ya entro en el campo de vision
            moveTo();

        }// esta al rango de ataque
        else if (target != initialPosition && distance <= atackRage )
        {
        if(!isAtack) StartCoroutine(Attack(atackSpeed));
        }



    }
   
    IEnumerator Attack(float seconds)
    {
        
        isKeep = true;
        isAtack = true;        
        yield return new WaitForSeconds(seconds);
        isAtack = false;
        
    }

    void moveTo()
    {
        _rb.MovePosition(transform.position + (dir * speed) * Time.deltaTime);
        isKeep = false;
        isAtack = false;

    }

    private void LateUpdate()
    {

        if (isKeep && !isAtack)
        {
            _animator.SetBool("IsKeep", true);
            _animator.SetBool("IsAtack", false);
        }
        else if (!isKeep && !isAtack)
        {
            _animator.speed = 1;
            _animator.SetBool("IsKeep", false);
            _animator.SetBool("IsAtack", false);
            _animator.SetFloat("MoveX", dir.x);
            _animator.SetFloat("MoveY", dir.y);
        }
        else if (isKeep && isAtack)
        {
            _animator.SetBool("IsKeep", true);
            _animator.SetBool("IsAtack", true);
            _animator.SetFloat("MoveX", dir.x);
            _animator.SetFloat("MoveY", dir.y);

        }

    }

    void trigerBow()
    {
        Debug.Log("lanzo");
        GameObject arrow = Instantiate(arrowPrefab, _frontPoint.transform.position, Quaternion.identity);
        if (dir.x > 0)
        {
            arrow.transform.GetComponent<SpriteRenderer>().flipX = true;
        }


    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, atackRage);
    }
    private void OnGUI()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        print(pos);
        GUI.Box(
      new Rect(
          pos.x - 20,                   // posición x de la barra
          Screen.height - pos.y + 60,   // posición y de la barra
          40,                           // anchura de la barra    
          24                            // altura de la barra  
      ), health + "/"+maxHealth              // texto de la barra
  );
    }
}

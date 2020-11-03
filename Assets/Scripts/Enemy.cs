using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float velocidad = 0.2f;
    public float limitXRight = 2f;
    public float limitXLeft = 2f;
    public float tiempoEspera = 2f;
    public float rangoDisparo = 1f;
    private GameObject _target;
    private Animator _animator;
    private Weapon _arma;
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _arma = GetComponentInChildren<Weapon>();
    }
    // Start is called before the first frame update

    void Start()
    {
        ActualizarTarget();
        StartCoroutine("GuardearZona");

    }

    // Update is called once per frame
    void Update()
    {



    }
    void ActualizarTarget() {
        if (_target == null)
        {
            _target = new GameObject("target");
            _target.transform.position = new Vector2(limitXRight, transform.position.y);
            return;
        }
        else if (_target.transform.position.x == limitXRight)
        {
            _target.transform.position = new Vector2(limitXLeft, transform.position.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (_target.transform.position.x == limitXLeft) {
            _target.transform.position = new Vector2(limitXRight, transform.position.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private IEnumerator GuardearZona() {

        while (Vector2.Distance(transform.position, _target.transform.position) > 0.05f)
        {
            // Update animator
            _animator.SetBool("IsWalking", true);
            // let's move to the target
            Vector2 direction = _target.transform.position - transform.position;
            float xDirection = direction.x;

            transform.Translate(direction.normalized * velocidad * Time.deltaTime);

            // IMPORTANT
            yield return null;
        }


        transform.position = new Vector2(_target.transform.position.x, transform.position.y);
        ActualizarTarget();


        _animator.SetBool("IsWalking", false);
        //_animator.SetTrigger("EmoteShow");
        //StartCoroutine("DispararPlayer");
        Vector2 selfPosition = transform.position;
        GameObject player = GameObject.Find("Player");
        Vector2 playerPosition = player.GetComponent<Transform>().position;
        float distance = Vector2.Distance(selfPosition, playerPosition);
        while (distance <= rangoDisparo)
        {
            _animator.SetTrigger("Disparar");
            selfPosition = transform.position;
            Debug.Log(selfPosition);           
    
            if (Vector2.Distance(selfPosition, playerPosition) <= rangoDisparo) {
                yield return null;
            }
            
        }
            yield return new WaitForSeconds(tiempoEspera); // IMPORTANT
        StartCoroutine("GuardearZona");
      

    }



    private IEnumerator DispararPlayer(){
        Vector2 selfPosition = transform.position;        
        GameObject player = GameObject.Find("Player");
        Vector2 playerPosition = player.GetComponent<Transform>().position;        
        float distance = Vector2.Distance(selfPosition,playerPosition );
       
        while (distance <= rangoDisparo)
        {            

            Vector2 direccion = selfPosition - playerPosition;                                    
            _animator.SetTrigger("Disparar");                        
            yield return null;
        }

        

    }

     void PuedeDisparar() {
        if (_arma!=null) {
        
            _arma.Disparar();
        }
    }
}

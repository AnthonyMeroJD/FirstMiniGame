using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTest : MonoBehaviour
{
    bool isMobibleFrontMe = false;
    GameObject movible;

    public float speed = 1f;
    //components
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _colliderAtack;
    
    private Animator _animator;
    private AttriLink attri;
    private Vector2 _movement,premovement;
    private bool isAtack;
    private bool isKeep;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _colliderAtack = transform.GetChild(0).GetComponent<CircleCollider2D>();
        attri = GetComponent<AttriLink>();

    }
    // Start is called before the first frame update
    void Start()
    {
        _animator.SetBool("isKeep", true);
        _colliderAtack.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {


         if (attri.health > 0)
        {
            _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            isAtack = Input.GetButtonDown("Fire1");
        }
        else {
            _rigidbody.velocity = Vector3.zero;
                
           }
        
      
        
    }

    private void FixedUpdate()
    {
        isKeep = _movement == Vector2.zero;
        if (!isKeep && !isAtack)  {
            _rigidbody.MovePosition(_rigidbody.position + _movement * speed * Time.deltaTime);
            premovement = _movement;
        }
        if (isKeep) _rigidbody.velocity= Vector2.zero; 
    }

    private void LateUpdate()
    {
        GestionarAnimacionMovimientos();
        GestionarAnimacionAtaque();  

    }
void GestionarAnimacionAtaque() {
        if (isAtack)
        {
            AnimatorStateInfo infoAnimation = _animator.GetCurrentAnimatorStateInfo(0);
            bool isAtacking = infoAnimation.IsName("Atack");
            if (!isAtacking)
            {
                if (_movement != Vector2.zero) _colliderAtack.offset = new Vector2(_movement.x / 10, _movement.y / 10);
                else
                {
                    if (premovement != Vector2.zero) _colliderAtack.offset = new Vector2(premovement.x / 10, premovement.y / 10);
                    else _colliderAtack.offset = new Vector2(0f, -0.1f);
                }
                _animator.SetTrigger("atacking");
                _animator.SetFloat("MoveX", _movement.x);
                _animator.SetFloat("MoveY", _movement.y);
            }
        }

        }
    void GestionarAnimacionMovimientos() {

        if (!isKeep)
        {
            _animator.SetBool("isKeep", false);
            _animator.SetFloat("MoveX", _movement.x);
            _animator.SetFloat("MoveY", _movement.y);
        }
        else
        {
            _animator.SetBool("isKeep", true);
            _animator.SetFloat("MoveX", premovement.x);
            _animator.SetFloat("MoveY", premovement.y);
        }
    }
    void AtackActivate() {
        _colliderAtack.enabled = true;
    }

    void AtackDesactivate() {
        _colliderAtack.enabled = false;
    }

}

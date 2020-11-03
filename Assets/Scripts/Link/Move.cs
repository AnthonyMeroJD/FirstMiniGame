using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    private Animator _animator;
    public float speed=1f;
    // Start is called before the first frame update

    private void Awake()
    {
        _animator = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Animator>();
    }
    void Start()
    {
        _animator.SetBool("isKeep",true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("Vertical"));
        ResetTrigers();
        if (Input.GetAxis("Horizontal") > 0)
        {
            _animator.SetBool("isKeep", false);
            _animator.SetTrigger("walkRight");
            MoveTo(Vector3.right);
            

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            _animator.SetBool("isKeep", false);
            _animator.SetTrigger("walkLeft");
            MoveTo(Vector3.left);
      

        }
        else if (Input.GetAxis("Vertical") > 0) {
            _animator.SetBool("isKeep", false);
            _animator.SetTrigger("walkUp");
            MoveTo(Vector3.up);


        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            _animator.SetBool("isKeep", false);
            _animator.SetTrigger("walkDown");
            MoveTo(Vector3.down);


        }
        else {
            _animator.SetBool("isKeep", true);
        }        
    }

    void ResetTrigers()
    {

        _animator.ResetTrigger("walkDown");
        _animator.ResetTrigger("walkUp");
        _animator.ResetTrigger("walkRight");
        _animator.ResetTrigger("walkLeft");
    }
    void MoveTo(Vector3 direccion) {
        
        transform.Translate(direccion.normalized * speed * Time.deltaTime);
        
    }
    
}

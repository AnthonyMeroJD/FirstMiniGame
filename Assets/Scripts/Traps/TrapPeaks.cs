using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPeaks : MonoBehaviour
{


    //components 
    private Animator animator;
    private bool isActivate;
    private bool setDamagePre;
    public string nameAnimationActivate, nameAnimationDesactivate,nameIdleAnimation;
    public float damage, timeToDesactivate,timeToActivate;

    
    // Start is called before the first frame update
    private void Awake()
    {
        InitializeComponents();
    }
    void Start()
    {
        isActivate = false;

        setDamagePre = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        
        if (GetStateAnimation(nameIdleAnimation) && !isActivate ) StartCoroutine("ActivateTrap");
        
    }
    IEnumerator ActivateTrap() {
        isActivate = true;
        PlayAnimation(nameAnimationActivate);
        yield return new WaitForSeconds(timeToActivate);
        PlayAnimation(nameAnimationDesactivate);
        yield return new WaitForSeconds(timeToDesactivate);
        PlayAnimation(nameIdleAnimation);
        isActivate = false;        
        
 


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject gameObject = collision.gameObject;
        if (gameObject != null && !setDamagePre) StartCoroutine(SetDamage(gameObject));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObject=collision.gameObject;
        if (gameObject != null && !setDamagePre) StartCoroutine(SetDamage(gameObject));
            
    }

    IEnumerator SetDamage(GameObject gameObject) {
        setDamagePre = true;
        if (gameObject.CompareTag("Player"))
        {
            AttriLink attriLink = gameObject.GetComponent<AttriLink>();
            attriLink.Damage(damage);
            yield return new WaitForSeconds(0.3f);
            setDamagePre = false;
        }
        setDamagePre = false;
    }
    bool GetStateAnimation(string animationName) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
    void PlayAnimation(string animationName) {
        animator.Play(animationName);
    
    }
    void InitializeComponents() {
        animator = GetComponent<Animator>();
        
    }

}

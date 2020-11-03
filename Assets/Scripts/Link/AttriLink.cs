using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AttriLink : MonoBehaviour
{
    public float health;
    public float armor;
    public float damage;
    public string state_dead;
    public RectTransform hpImage;
    public RectTransform hpValueText;
    //components
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            health = 0;
            animator.Play(state_dead);
        }
    }

    public void Damage(float damage){
        if (health > 0)
        {
            health -= Mathf.RoundToInt(damage - (damage * armor / 100));
            animator.Play("LinkReciveAtack");
            hpImage.GetComponent<UnityEngine.UI.Image>().fillAmount = (health / 100);            
            hpValueText.GetComponent<Text>().text = "" + Mathf.RoundToInt(health) + "%";
        }
    }
    private void LateUpdate()
    {
        
        
    }
}

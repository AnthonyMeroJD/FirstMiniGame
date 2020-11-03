using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject balaPrefact;
    public GameObject disparador;
    private Transform firePoint;
    
     void Awake()
    {
        //en este caso weapon
        firePoint = transform.Find("firePoint");//retorna el hijo que contenga el padre transform
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
 

    }

   public void Disparar() {
        if (firePoint != null && balaPrefact != null && disparador!=null)
        {
            GameObject bala = Instantiate(balaPrefact, firePoint.position, Quaternion.identity) as GameObject;
            Bala balaComponent = bala.GetComponent<Bala>();
            if (disparador.transform.localScale.x >= 1f)
            {
                //balaComponent.direccion = Vector2.right;
            }
            else
            {
                //balaComponent.direccion = Vector2.left;
            }
        }
    }
}

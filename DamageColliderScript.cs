using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColliderScript : MonoBehaviour
{

    public bool playerTwo;
    public playerMove playerReference;

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
        if (!playerTwo)
        {
            

            if (collision.gameObject.CompareTag("p2"))
            {
                Debug.Log("Detect Hit");

                playerReference.hitEnemy();

                Debug.Log("TriggerEnterPlayer 1");
            }
        }
        else if(playerTwo)
        {
            if (collision.gameObject.CompareTag("p1"))
            {
                playerReference.hitEnemy();

                
                Debug.Log("TriggerEnterPlayer 2");
            }
        }


    }
}

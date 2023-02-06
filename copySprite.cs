using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class copySprite : MonoBehaviour
{

    public SpriteRenderer target;

    float globalY;

    // Start is called before the first frame update
    void Start()
    {
        globalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sprite = target.sprite;

        transform.position = new Vector3(transform.position.x, globalY, transform.position.z);
    }
}

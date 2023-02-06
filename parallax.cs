using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{

	private float lengthX, startPosX;
	private float lengthY, startPosY;
	public GameObject cam;
	public float parallexEffect;

	public bool Y = true;
	public bool move = true;

	float tempX;
	float distX;

	float tempY;
	float distY;



	void Start()
	{
		startPosX = transform.position.x;
		lengthX = GetComponent<SpriteRenderer>().bounds.size.x;

		startPosY = transform.position.y;
		lengthY = GetComponent<SpriteRenderer>().bounds.size.y;
	}

	void Update()
	{
		tempX = (cam.transform.position.x * (1 - parallexEffect));
		distX = (cam.transform.position.x * parallexEffect);

        if (Y)
        {
			tempY = (cam.transform.position.y * (1 - parallexEffect));
			distY = (cam.transform.position.y * parallexEffect);
        }
		
		transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if (move)
        {
			if (tempX > startPosX + lengthX) startPosX += lengthX;
			else if (tempX < startPosX - lengthX) startPosX -= lengthX;
		}
		
		
		if(Y && move)
        {
			if (tempY > startPosY + lengthY) startPosY += lengthY;
			else if (tempY < startPosY - lengthY) startPosY -= lengthY;
        }
	}

}

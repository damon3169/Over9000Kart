using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public float speed = 10;

	 Vector2 couloir;

	Camera camera;



	// Use this for initialization
	void Start () {
		camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		goLeft();
		
	}

	void goLeft()
	{
		transform.Translate(Vector2.left * Time.deltaTime * speed);
	}

	void destroyObject() 
	{
		Destroy(this.gameObject);
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == ("MainCamera")) {
			destroyObject();
		}
	}
}

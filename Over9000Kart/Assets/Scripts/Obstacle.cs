using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public float speed = 10;

	Vector2 couloir;

<<<<<<< HEAD
	Camera camera;
=======
>>>>>>> 7bdc08c176a4512f1eb16f458b6d9f4dfd52f6f8

	// Use this for initialization
	void Start () {
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

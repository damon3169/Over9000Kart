using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
<<<<<<< HEAD
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
        if (collision.gameObject.tag == "Ship")
        {
            Debug.Log("collision");
            collision.gameObject.GetComponent<Ship>().drawback();
=======
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    
		if (transform.position.x + transform.localScale.x / 2 < GameManager.instance.cam.transform.position.x - GameManager.instance.getCameraWidth() / 2)
        {
            Destroy(this.gameObject);
>>>>>>> ef97d82707e956a4b9301bb5f38b9932a5e93233
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public float speed;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (transform.position.x + transform.localScale.x / 2 < GameManager.instance.cam.transform.position.x - GameManager.instance.getCameraWidth() / 2)
        {
            Destroy(this.gameObject);
        }
	}
}

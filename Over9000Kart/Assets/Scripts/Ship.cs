using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float speed; // vitesse du vaisseau
    public Vector2 position; // position x y du vaisseau

	// Use this for initialization
	void Start () {
        speed = -0.1f; ; // vitesse de base du vaisseau
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("SpeedUp")) speed += 0.3f;
        if (speed > 0) speed -= 0.05f;
        else speed = 0;
        gameObject.transform.Translate(Time.deltaTime*speed, 0, 0);
        
        Debug.Log("Vitesse=" + speed);
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float speed; // vitesse du vaisseau
    public float score; // score de vitesse du vaisseau 
    //public Vector2 position; // position x y du vaisseau
    public int idJoueur; // numéro du joueur controlant le vaisseau
    string controleurJoueur; // nom du bouton correspondant au numéro de joueur

	// Use this for initialization
	void Start () {
        transform.position = new Vector3((GameManager.instance.getCameraWidth() / 10) - (GameManager.instance.getCameraWidth()/2), transform.position.y, transform.position.z);
        speed = 0f; ; // vitesse de base du vaisseau
        switch(idJoueur) // 
        {
            case 1:
                controleurJoueur = "Player1";
                break;
            case 2:
                controleurJoueur = "Player2";
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        score = transform.position.x*100 + 8000 - GameManager.instance.getCameraWidth();
        if (Input.GetButtonDown(controleurJoueur+"_SpeedUp") && speed<GameManager.instance.speedMax) speed += GameManager.instance.acceleration;
        if (speed > GameManager.instance.bufferSpeed) speed -= GameManager.instance.frein; // frein naturel
        //score = gameObject.transform.position.x;

        if (speed > GameManager.instance.speedMin) gameObject.transform.Translate(Time.deltaTime * speed, 0, 0);
        else speed = GameManager.instance.speedMin +0.1f;
        Debug.Log(score);
	}
}

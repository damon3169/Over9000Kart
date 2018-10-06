using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float speed; // vitesse du vaisseau
    public int score; // score de vitesse du vaisseau 
    //public Vector2 position; // position x y du vaisseau
    public int idJoueur; // numéro du joueur controlant le vaisseau
    string controleurJoueur; // nom du bouton correspondant au numéro de joueur
    public float speedMin;
    GameManager gameManager;

	// Use this for initialization
	void Start () {
        //gameManager = new GameManager();
        //transform.position = new Vector3(gameManager.getCameraWidth() % 25, transform.position.y, transform.position.z);
        score = 0;
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
        Debug.Log(controleurJoueur);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(controleurJoueur+"_SpeedUp") && speed<3) speed += 0.2f;
        if (speed > -1) speed -= 0.02f; // frein naturel
        //score = gameObject.transform.position.x;

        if (speed > speedMin) gameObject.transform.Translate(Time.deltaTime * speed, 0, 0);
        else speed = speedMin+0.1f;
	}
}

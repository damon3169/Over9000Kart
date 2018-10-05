using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public float speed; // vitesse du vaisseau
    public Vector2 position; // position x y du vaisseau
    public int idJoueur; // numéro du joueur controlant le vaisseau
    string controleurJoueur; // nom du bouton correspondant au numéro de joueur

	// Use this for initialization
	void Start () {
        speed = -0.1f; ; // vitesse de base du vaisseau
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
        if (Input.GetButtonDown(controleurJoueur+"_SpeedUp") && speed<3) speed += 0.2f;
        if (speed > -1) speed -= 0.03f; // frein naturel
        
        if (speed > 0) gameObject.transform.Translate(Time.deltaTime*speed, 0, 0);
	}
}

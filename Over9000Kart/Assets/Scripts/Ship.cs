using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

	public float speed; // vitesse du vaisseau
	public int idJoueur; // numéro du joueur controlant le vaisseau
	string controleurJoueur; // nom du bouton correspondant au numéro de joueur
	private int actualCorridor; // couloir dans lequel le vaisseau se trouve
	private couloirs corridor; 
	public float score; // score du vaisseau


	// Use this for initialization
	void Start () {
        transform.position = new Vector3((GameManager.instance.getCameraWidth() / 10) - (GameManager.instance.getCameraWidth()/2), transform.position.y, transform.position.z);
        speed = 0f; ; // vitesse de base du vaisseau
		corridor = GameManager.instance.couloirs;

		switch (idJoueur) // 
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
	void Update()
	{
        if (!GameManager.instance.finished)
        {
            score = transform.position.x * 100 + 8000 - GameManager.instance.getCameraWidth(); // calcul du score selon la position en x du vaisseau

            // freine le vaisseau en continu tant qu'on est au dessus de la vitesse minimum
            if (speed > GameManager.instance.speedMin) speed -= GameManager.instance.frein;

            // mouvement du vaisseau
            if (speed > GameManager.instance.speedMin && transform.position.x > GameManager.instance.xMin) transform.Translate(Time.deltaTime * speed, 0, 0);
            //else speed = GameManager.instance.speedMin + 0.1f;

            // déplacement de couloir
		    if (Input.GetButtonDown(controleurJoueur + "_ChangeCorridor_K") || Input.GetButtonDown(controleurJoueur + "_ChangeCorridor_J"))
		    {
			    if (Input.GetAxis(controleurJoueur + "_ChangeCorridor_K") < 0 || Input.GetAxis(controleurJoueur + "_ChangeCorridor_J") < 0)
                {
				    if (actualCorridor > 0)
				    {
					    setActualCorridor(actualCorridor - 1);
					    this.transform.position = new Vector3(transform.position.x, corridor.couloirsList[actualCorridor].y, transform.position.z);
				    }

			    }
			    if (Input.GetAxis(controleurJoueur + "_ChangeCorridor_K") > 0 || Input.GetAxis(controleurJoueur + "_ChangeCorridor_J") > 0)
			    {
				    if (actualCorridor < corridor.couloirsList.Count-1)
				    {
					    setActualCorridor(actualCorridor + 1);
					    this.transform.position = new Vector3(transform.position.x, corridor.couloirsList[actualCorridor].y, transform.position.z);
				    }
			    }
		    }

            // si le joueur mash les boutons pour accelerer et que sa vitesse n'est pas supérieure à la vitesse maximale ni inférieure à la vitesse minimale
            if ((Input.GetButtonDown(controleurJoueur + "_SpeedUp_K") || Input.GetButtonDown(controleurJoueur + "_SpeedUp_J")) && speed < GameManager.instance.speedMax)
            {
                speed += GameManager.instance.acceleration; // on augmente la vitesse selon le niveau d'acceleration
            }
        } else {
            this.transform.Translate(Vector3.left * Time.deltaTime * transform.position.x);
        }
	}

	public void setActualCorridor(int corridor)
	{
		actualCorridor = corridor;
	}

    public void drawback()
    {
        if (transform.position.x - GameManager.instance.drawbackObstacle > GameManager.instance.xMin) transform.Translate(-GameManager.instance.drawbackObstacle, 0, 0);
        else
        {
            Vector3 v = new Vector3(GameManager.instance.xMin, transform.position.y, transform.position.z);
            transform.position = v;
        }
    }
}

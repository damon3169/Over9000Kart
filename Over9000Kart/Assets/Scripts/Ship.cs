using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float speed; // vitesse du vaisseau
	public int idJoueur; // numéro du joueur controlant le vaisseau
	public string controleurJoueur; // nom du bouton correspondant au numéro de joueur
	private int actualCorridor; // couloir dans lequel le vaisseau se trouve
	private couloirs corridor; 
	public float score; // score du vaisseau
    public float scoreFight = 0;
    public bool fightingUp;
    public float cooldownFightBegin;
    public float cooldownFightDuration = 6f;
    public bool isFigtingInCooldown = false;
    public bool isThereAndCooldown = false;

	float range;
	private float totalDistance;
    bool dPadPressed;


	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // on récupère le sprite dans le gameobject vide fils
        transform.position = new Vector3((GameManager.instance.getCameraWidth() / 10) - (GameManager.instance.getCameraWidth()/2), transform.position.y, transform.position.z);
        speed = 0f; ; // vitesse de base du vaisseau
		corridor = GameManager.instance.couloirs;
        dPadPressed = false;

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

        // animation vaisseau
        range = (speed / 500) + 0.01f;
        Vector3 anim = new Vector3(spriteRenderer.transform.position.x + Random.Range(-range, range), spriteRenderer.transform.position.y + Random.Range(-range, range), spriteRenderer.transform.position.z);
        spriteRenderer.transform.position = anim;

		if (!GameManager.instance.finished)
        {
			if (!GameManager.instance.isStarting)
			{
            if (!GameManager.instance.isInFight)
            {
					totalDistance = GameManager.instance.xMax - GameManager.instance.xMin;
					totalDistance = totalDistance / 100;
					float distanceFromStart =	transform.position.x - GameManager.instance.xMin ;
					distanceFromStart = distanceFromStart / totalDistance;
					score = 8000 + distanceFromStart * 10;
					//score = transform.position.x * 100 + 8000 - GameManager.instance.getCameraWidth(); // calcul du score selon la position en x du vaisseau

                // freine le vaisseau en continu tant qu'on est au dessus de la vitesse minimum
                if (speed > GameManager.instance.speedMin) speed -= GameManager.instance.frein;

                // mouvement du vaisseau
                if (speed > GameManager.instance.speedMin && transform.position.x > GameManager.instance.xMin) transform.Translate(Time.deltaTime * speed, 0, 0);
                else speed = GameManager.instance.speedMin + 0.1f;

                // déplacement de couloir
                if (Input.GetButtonDown(controleurJoueur + "_ChangeCorridor_K") || (Input.GetAxis(controleurJoueur + "_ChangeCorridor_J") != 0 && !dPadPressed))
                {
                    dPadPressed = true;
                    if (Input.GetAxis(controleurJoueur + "_ChangeCorridor_K") < 0 || Input.GetAxis(controleurJoueur + "_ChangeCorridor_J") < 0)
                    {
                        if (actualCorridor > 0)
                        {
                            GameManager.instance.isCorridorUse(idJoueur, actualCorridor - 1);
                            if (!GameManager.instance.isInFight && !isThereAndCooldown)
                            {
                                setActualCorridor(actualCorridor - 1);
                            }
                        }

                    }
                    if (Input.GetAxis(controleurJoueur + "_ChangeCorridor_K") > 0 || Input.GetAxis(controleurJoueur + "_ChangeCorridor_J") > 0)
                    {
                        if (actualCorridor < corridor.couloirsList.Count - 1)
                        {
                            GameManager.instance.isCorridorUse(idJoueur, actualCorridor + 1);
                            if (!GameManager.instance.isInFight && !isThereAndCooldown)
                            {
                                setActualCorridor(actualCorridor + 1);
                            }
                        }
                    }
                }
                else if (Input.GetAxis(controleurJoueur + "_ChangeCorridor_J") == 0 && dPadPressed) dPadPressed = false;

					// si le joueur mash les boutons pour accelerer et que sa vitesse n'est pas supérieure à la vitesse maximale ni inférieure à la vitesse minimale
                if ((Input.GetButtonDown(controleurJoueur + "_SpeedUp_K") || Input.GetButtonDown(controleurJoueur + "_SpeedUp_J")) && speed < GameManager.instance.speedMax)
                {
                    speed += GameManager.instance.acceleration; // on augmente la vitesse selon le niveau d'acceleration
                    if (transform.position.x <= GameManager.instance.xMin)
                    {
                        Vector3 v = new Vector3(GameManager.instance.xMin + 0.01f, transform.position.y, transform.position.z);
                        transform.position = v;
                    }
                }
            }

            if (isFigtingInCooldown)
            {
                if (Time.time > cooldownFightBegin + cooldownFightDuration)
                {
                    isThereAndCooldown = false;
                    isFigtingInCooldown = false;
                }
            }
			}
		} else {
            this.transform.Translate(Vector3.left * Time.deltaTime * transform.position.x);
            if (GameManager.instance.finished && Input.GetButtonDown(controleurJoueur + "_Reset"))
            {
                GameManager.instance.debut_de_partie();
            }
        }
	}

    public int getActualCorridor()
    {
        return actualCorridor;
    }

    public void setActualCorridor(int newcorridor)
    {
        actualCorridor = newcorridor;
        corridor = GameManager.instance.couloirs;
        transform.position = new Vector3(transform.position.x, corridor.couloirsList[actualCorridor].y, transform.position.z);
    }

    public void drawback()
    {
        if (transform.position.x - GameManager.instance.drawbackObstacle > GameManager.instance.xMin) transform.Translate(-GameManager.instance.drawbackObstacle, 0, 0);
        else
        {
            Vector3 v = new Vector3(GameManager.instance.xMin, transform.position.y, transform.position.z);
            transform.position = v;
        }
        if (GameManager.instance.isInFight)
        {
            if (idJoueur == 1)
            {
                Ship otherShip = GameManager.instance.getListShip()[0];
                if (otherShip.transform.position.x - GameManager.instance.drawbackObstacle > GameManager.instance.xMin) otherShip.transform.Translate(-GameManager.instance.drawbackObstacle, 0, 0);
                else
                {
                    Vector3 v = new Vector3(GameManager.instance.xMin, otherShip.transform.position.y, otherShip.transform.position.z);
                    otherShip.transform.position = v;
                }
            } else {
                Ship otherShip = GameManager.instance.getListShip()[1];
                if (otherShip.transform.position.x - GameManager.instance.drawbackObstacle > GameManager.instance.xMin) otherShip.transform.Translate(-GameManager.instance.drawbackObstacle, 0, 0);
                else
                {
                    Vector3 v = new Vector3(GameManager.instance.xMin, otherShip.transform.position.y, otherShip.transform.position.z);
                    otherShip.transform.position = v;
                }
            }
        }
    }
}

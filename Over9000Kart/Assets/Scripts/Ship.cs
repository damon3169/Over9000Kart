﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
	public RectTransform chargeBar;
	public SpriteRenderer laserIcon;

	float range;
	private float totalDistance;
    bool dPadPressed;

    SpriteRenderer animation;
    float opacitéAnimation;

    AudioSource sourceShip;

    public GameObject laserOrigin;
    bool isShootingLaser;
    float timerLaserBegin;

	bool isCooldownShootLaser = false;

	float cooldownLaserbegin;
	float cooldownLaserDuration = 5;
	float cooldownLaserPercentage;
	public GameObject textLaser;
	float adaptiveCooldownLaserDuration;


	// Use this for initialization
	void Start () {
        isShootingLaser = false;
        animation = GetComponentInChildren<Animator>().gameObject.GetComponent<SpriteRenderer>();
        opacitéAnimation = 0;
        animation.color = new Color(1, 1, 1, opacitéAnimation);
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
        range = 0.005f;
        Vector3 anim = new Vector3(spriteRenderer.transform.position.x + Random.Range(-range, range), spriteRenderer.transform.position.y + Random.Range(-range, range), spriteRenderer.transform.position.z);
        spriteRenderer.transform.position = anim;
        // animation vitesse
        opacitéAnimation = speed*2;
        animation.color = new Color(1, 1, 1, opacitéAnimation);

        if (!GameManager.instance.finished)
        {
			if (!GameManager.instance.isStarting)
			{
            if (!GameManager.instance.isInFight)
            {
					if (idJoueur == 1)
					{
						adaptiveCooldownLaserDuration = cooldownLaserDuration - (cooldownLaserDuration * (((GameManager.instance.getListShip()[0].score - score) / 10) / 100));
					}
					else
					{
						adaptiveCooldownLaserDuration = cooldownLaserDuration - (cooldownLaserDuration * (((GameManager.instance.getListShip()[1].score - score) / 10) / 100));
					}
					if (isCooldownShootLaser)
					{
						if (Time.time > cooldownLaserbegin + adaptiveCooldownLaserDuration)
						{
							textLaser.GetComponent<TextMeshProUGUI>().text = "100";
							laserIcon.color = Color.white;
							isCooldownShootLaser = false;
						}
						else
						{
							cooldownLaserPercentage = (cooldownLaserbegin - Time.time)/ (adaptiveCooldownLaserDuration / 100);
							textLaser.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(cooldownLaserPercentage).ToString();
							laserIcon.color = Color.gray;
						}
					}
                    if(isShootingLaser)
                    {
                        // rotation de l'origine du laser selon la position du joueur adverse
                        Vector2 direction = GameManager.instance.getOtherShip(idJoueur).transform.position - transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                        laserOrigin.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Slerp(laserOrigin.GetComponentInChildren<SpriteRenderer>().transform.rotation, rotation, 10);
                        GameManager.instance.getOtherShip(idJoueur).drawback();
                        if (Time.time > timerLaserBegin + GameManager.instance.timerLaserDuration)
                        {
                            stopLaser();
                        }
                    }

                    totalDistance = GameManager.instance.xMax - GameManager.instance.xMin;
					totalDistance = totalDistance / 100;
					float distanceFromStart =	transform.position.x - GameManager.instance.xMin ;
					distanceFromStart = distanceFromStart / totalDistance;
					score = 8000 + distanceFromStart * 10;
                    //score = transform.position.x * 100 + 8000 - GameManager.instance.getCameraWidth(); // calcul du score selon la position en x du vaisseau

                    // freine le vaisseau en continu tant qu'on est au dessus de la vitesse minimum
                    if (speed > GameManager.instance.speedMin) speed -= GameManager.instance.frein;
                    else speed += Time.deltaTime * -speed;

                // mouvement du vaisseau
                if (speed > GameManager.instance.speedMin && transform.position.x > GameManager.instance.xMin) transform.Translate(Time.deltaTime * speed, 0, 0);
                else speed = GameManager.instance.speedMin + 0.3f;

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

				float distanceSpeed = GameManager.instance.speedMax - GameManager.instance.speedMin;
				float onePercentSpeed = distanceSpeed / 100;
				float actualPercentageSpeed = speed / onePercentSpeed;
				chargeBar.sizeDelta = new Vector2(actualPercentageSpeed,100);

                if ((Input.GetButtonDown(controleurJoueur + "_SpeedUp_K") || Input.GetButtonDown(controleurJoueur + "_SpeedUp_J")) && speed < GameManager.instance.speedMax)
                {
                    speed += GameManager.instance.acceleration; // on augmente la vitesse selon le niveau d'acceleration
                    if (transform.position.x <= GameManager.instance.xMin)
                    {
                        Vector3 v = new Vector3(GameManager.instance.xMin + 0.01f, transform.position.y, transform.position.z);
                        transform.position = v;
                    }
                }

                if ((Input.GetButtonDown(controleurJoueur + "_SpeedUp_K") || Input.GetButtonDown(controleurJoueur + "_SpeedUp_J")) && speed < GameManager.instance.speedMax)
                {
                    speed += GameManager.instance.acceleration; // on augmente la vitesse selon le niveau d'acceleration
                    if (transform.position.x <= GameManager.instance.xMin)
                    {
                        Vector3 v = new Vector3(GameManager.instance.xMin + 0.01f, transform.position.y, transform.position.z);
                        transform.position = v;
                    }
                }

                if ((Input.GetButtonDown(controleurJoueur + "_Laser")))
                {
						if (!isCooldownShootLaser)
						{
							fireLaser();
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
            if (score >= 9000)
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * transform.position.x);
            } else if (transform.position.x > -GameManager.instance.getCameraWidth()) {
                this.transform.Translate(Vector3.left * Time.deltaTime * -transform.position.x);
            }
            if (GameManager.instance.finished && Input.GetButtonDown(controleurJoueur + "_Reset"))
            {
                GameManager.instance.debut_de_partie();
                GameManager.instance.Compteur.GetComponent<TextMeshProUGUI>().fontSize *= 4;
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
        speed = -1;
        if (GameManager.instance.isInFight)
        {
            if (idJoueur == 1)
            {
                GameManager.instance.getListShip()[0].speed = -1;
            } else {
                GameManager.instance.getListShip()[1].speed = -1;
            }
        }
    }

	public void drawbackFight()
	{
		transform.position = new Vector3 (transform.position.x-1.5f, transform.position.y, transform.position.z); 
	}

    public void fireLaser()
    {
        if(!isShootingLaser)
        {
            timerLaserBegin = Time.time;
            Ship target=GameManager.instance.getOtherShip(idJoueur);
            
            GameObject laser = Instantiate(GameManager.instance.laser, new Vector3(laserOrigin.transform.position.x, laserOrigin.transform.position.y, 1),Quaternion.identity);
            laser.transform.parent = laserOrigin.transform;
            laserOrigin.GetComponent<SpriteRenderer>().enabled = true;
            isShootingLaser = true;
        }
        
    }

    public void stopLaser()
    {
        if(isShootingLaser)
        {
            isShootingLaser = false;
			cooldownLaserbegin = Time.time;
			isCooldownShootLaser = true;
			laserOrigin.GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform child in laserOrigin.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            laserOrigin.transform.rotation = new Quaternion(0,0,0,0);
        }
    }
}

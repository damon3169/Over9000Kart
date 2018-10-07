using System.Collections;
using System.Collections.Generic;
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

	float range;
	private float totalDistance;
    bool dPadPressed;

    SpriteRenderer animation;
    float opacitéAnimation;

    AudioSource sourceShip;

    GameObject laserOrigin;

	// Use this for initialization
	void Start () {
        //laserOrigin=FindObjectInChildOfType

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
        range = (speed / 500) + 0.01f;
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
                        fireLaser();
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
        Ship target=GameManager.instance.getOtherShip(idJoueur);
        GameObject laser = Instantiate(GameManager.instance.laser, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        laser.transform.parent = transform;

        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, 10);
    }
}

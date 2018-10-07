using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

	//variables du jeu
	public bool finished;

	// variables statiques du vaisseau
	public float speedMin; // vitesse minimale à laquelle un vaisseau peut aller (peut être négative)
	public float speedMax; // vitesse maximale à laquelle un vaisseau peut aller
	public float frein; // frein naturel contre l'acceleration
	public float acceleration; // modificateur d'acceleration d'un vaisseau
	public float drawbackObstacle; // distance de recul suite à une collision d'obstacle

	List<Ship> listShip; // liste des vaisseaux dans une partie

	public float xMin; // position x minimale d'un vaisseau sur l'écran
	public float xMax;
	// ui
	public GameObject GameObjectUIManager;
	UiManager uiManager;

	// couloirs
	public couloirs couloirs;
	private List<int> usedCouloirs;

	//obstacles
	public float timerObstaclesRange = 3;
	public float timerObstacles;
	private float timerObstaclesBegin;
	private int numberObstacle;
	public GameObject obstacle;
	private bool isObstacleSpawn;
	private int SpawnIn;

	// stars et background
	public GameObject star;
	float timeSinceLastStarGenerated = 0.0f;
	float starGenerationCooldown = 0.01f;
	public GameObject background;
	public float randomDistance = 3;
    public float starSpeed;
    public float starSize;

	// main camera
	public Camera cam;

	// singleton gamemanager
	public static GameManager instance = null;
	public bool isInFight = false;
	public float timerFightBegin = 3f;
	public float timerFightDuration = 3f;
	private int idFighter;
	private int idDefenser;
	private int CorridorVisee;
	public bool isStarting = true;
	private float timerStart = 5;
	public GameObject Compteur;
	public GameObject spark;
	private GameObject activeSpark;
	private bool dPadPressedFighter;
	private bool dPadPressedDefenser;

    // musique
    public AudioClip menu;
    public AudioClip decompte;
    public AudioClip intro;
    public AudioClip refrain;
    private AudioSource sourceMusique;
    public float volumeMusique;
    bool hasPlayedIntro;

    // sons
    public AudioClip speedup;
    public AudioClip speeddown;
    public AudioClip collision;
    public AudioClip comet;
    public int nbPaliers;
    float[] paliersMusique;

	public void debut_de_partie()
	{
		finished = false;
		isStarting = true;
		//hasPlayedIntro = false;
		Compteur.GetComponent<TextMeshProUGUI>().text = "";


		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		listShip[0].setActualCorridor(couloirs.couloirsList.Count - 1);
		listShip[0].transform.position = new Vector3(cam.transform.position.x - width / 2 + 2, couloirs.couloirsList[couloirs.couloirsList.Count - 1].y, -1);
		listShip[1].setActualCorridor(0);
		listShip[1].transform.position = new Vector3(cam.transform.position.x - width / 2 + 2, couloirs.couloirsList[0].y, -1);

		foreach (Ship ship in listShip)
		{
			ship.score = 8000;
			ship.transform.position = new Vector3((GameManager.instance.getCameraWidth() / 10) - (GameManager.instance.getCameraWidth() / 2), ship.transform.position.y, ship.transform.position.z);
			ship.speed = 0f; ; // vitesse de base du vaisseau
		}
        
        starSize = 0.2f;

        timerStart = 5.0f;
	}

	void Awake()
	{
        sourceMusique = GetComponent<AudioSource>();
		cam = Camera.main; // assigne la main camera
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		// liste des ship
		listShip = new List<Ship>(); // initalise la liste des ship
		GameObject[] tempListShip = GameObject.FindGameObjectsWithTag("Ship"); // tableau temporaire des ship
		foreach (GameObject ship in tempListShip) // assignation des ship du tableau temporaire dans la liste
		{
			listShip.Add(ship.GetComponent<Ship>());
		}

		couloirs.createCorridors();
	}

	private float random_width()
	{
		return cam.transform.position.x - cam.orthographicSize * cam.aspect + Random.Range(0.0f, cam.orthographicSize * cam.aspect * 2);
	}

	private float random_height()
	{
		return cam.transform.position.y - cam.orthographicSize + Random.Range(0.0f, cam.orthographicSize * 2);
	}

	void Start()
	{
        paliersMusique = new float[nbPaliers];
        for (int i = 0; i < nbPaliers; i++)
        {
            paliersMusique[i] = 8000 + ((1000 / nbPaliers) * i + 1);
        }
        hasPlayedIntro = false;
		xMin = -getCameraWidth() / 2 + getCameraWidth() / 10;
		xMax = getCameraWidth() / 2 - getCameraWidth() / 10;
		uiManager = GameObjectUIManager.GetComponent<UiManager>(); // on récupère l'uiManager

		timerObstaclesBegin = 0;
		timerObstacles = Random.Range(1, timerObstaclesRange);
		usedCouloirs = new List<int>(); // intialisation de la liste des couloirs

        int nbStars = Random.Range(500, 5000); // nombre aléatoire d'étoiles à l'écran

        for (int i = 0; i < nbStars; i++) // génération des étoiles sur le background
        {
            GameObject newStar = Instantiate(star);
            newStar.transform.position = new Vector3(random_width(), random_height(), 0);
        }
        debut_de_partie();
    }


	void Update()
	{
        if (!sourceMusique.isPlaying && !hasPlayedIntro && !isStarting)
        {
            sourceMusique.PlayOneShot(intro, volumeMusique);
            hasPlayedIntro = true;
        }
        if (hasPlayedIntro && !sourceMusique.isPlaying)
        {
            sourceMusique.PlayOneShot(refrain, volumeMusique);
            //hasPlayedIntro = false;
        }
        
        if (getHighestSpeed() < 8200) { sourceMusique.pitch = 1f; starSpeed = 1f; starSize = 0.2f; }
        if (getHighestSpeed() > 8300) { sourceMusique.pitch = 1.2f; starSpeed = 8f; starSize = 1f; }
        if (getHighestSpeed() > 8600) { sourceMusique.pitch = 1.4f; starSpeed = 32f; starSize = 3f; }
        if (getHighestSpeed() > 8800) { sourceMusique.pitch = 1.6f; starSpeed = 128f; starSize = 5f; }
        for(int i=0; i<nbPaliers;i++)
        {
            if (getHighestSpeed() > paliersMusique[i]) sourceMusique.pitch = 1+(0.1f*(i));
        }

        timeSinceLastStarGenerated += Time.deltaTime;
		if (timeSinceLastStarGenerated >= starGenerationCooldown)
		{
			timeSinceLastStarGenerated = 0.0f;
            for (int i = 0; i < starSpeed; i++)
            {
			    GameObject newStar = Instantiate(star);
			    newStar.transform.position = new Vector3(cam.transform.position.x + cam.orthographicSize * cam.aspect, random_height(), 0);
            }
		}

		if (isStarting)
		{
			timerStart -= Time.deltaTime;
			Compteur.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(timerStart).ToString();
            if (timerStart < 4 && !hasPlayedIntro)
            {
                sourceMusique.PlayOneShot(decompte, 1);
                hasPlayedIntro = true;
            }
            if (timerStart < 0)
			{
				timerStart = 0;
				Compteur.GetComponent<TextMeshProUGUI>().text = "";
				isStarting = false;
			}
		}

		if (finished)
		{
			Compteur.GetComponent<TextMeshProUGUI>().text = "Restart: R / Menu: ESCAPE";
			if (Input.GetKeyDown("escape"))
			{
				SceneManager.LoadScene("Menu");
				GameObject.Destroy(gameObject);
			}
		}

		if (Time.time > timerObstaclesBegin + timerObstacles && !finished && !isStarting)
		{
			timerObstacles = Random.Range(1, timerObstaclesRange);
			numberObstacle = Random.Range(1, couloirs.numberOfCorridor);
			timerObstaclesBegin = Time.time;
			usedCouloirs.Clear();
			for (int i = 0; i < numberObstacle; i++)
			{
				isObstacleSpawn = false;
				while (!isObstacleSpawn)
				{
					SpawnIn = Random.Range(0, couloirs.numberOfCorridor);
					float distanceBetweenObstacles = Random.Range(0.5f, randomDistance);
					if (usedCouloirs == null || !usedCouloirs.Contains(SpawnIn))
					{
						usedCouloirs.Add(SpawnIn);
						GameObject obst = Instantiate(obstacle);
						float height = 2f * cam.orthographicSize;
						float width = height * cam.aspect;
						obst.transform.position = new Vector3(cam.transform.position.x + width / 2 + distanceBetweenObstacles, couloirs.couloirsList[SpawnIn].y, couloirs.couloirsList[SpawnIn].z);
						isObstacleSpawn = true;
					}
				}
			}
		}

		if (isInFight)
		{
			if (Time.time > GameManager.instance.timerFightBegin + GameManager.instance.timerFightDuration)
			{
				GameObject.Destroy(activeSpark);
				isInFight = false;
				if (listShip[idFighter].scoreFight > listShip[idDefenser].scoreFight)
				{

					listShip[idFighter].setActualCorridor(CorridorVisee);
					listShip[idDefenser].setActualCorridor(listShip[idDefenser].getActualCorridor());
					listShip[idDefenser].drawback();
					listShip[idDefenser].drawbackFight();
				}
				else
				{
					listShip[idFighter].setActualCorridor(listShip[idFighter].getActualCorridor());
					listShip[idFighter].drawback();
					listShip[idDefenser].setActualCorridor(listShip[idDefenser].getActualCorridor());
					listShip[idFighter].drawbackFight();
				}
				listShip[idFighter].scoreFight = 0;
				listShip[idDefenser].scoreFight = 0;
				Compteur.GetComponent<TextMeshProUGUI>().text = "";

			}
			else
			{
				if (listShip[idFighter].fightingUp)
				{
					if (Input.GetButtonDown(listShip[idFighter].controleurJoueur + "_ChangeCorridor_K") || (Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_J") != 0 && !dPadPressedFighter))
					{
						dPadPressedFighter = true;
						if (Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_K") > 0 || Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_J") > 0)
						{
							listShip[idFighter].scoreFight++;
						}
					}
					else if (Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_J") == 0 && dPadPressedFighter) dPadPressedFighter = false;

					if (Input.GetButtonDown(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_K") || (Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_J") != 0 && !dPadPressedDefenser))
					{
						dPadPressedDefenser = true;
						if (Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_K") < 0 || Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_J") < 0)
						{
							listShip[idDefenser].scoreFight++;
						}
					}
					else if (Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_J") == 0 && dPadPressedDefenser) dPadPressedDefenser = false;

				}

				else
				{
					if (Input.GetButtonDown(listShip[idFighter].controleurJoueur + "_ChangeCorridor_K") || (Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_J") != 0 && !dPadPressedFighter))
					{
						dPadPressedFighter = true;
						if (Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_K") < 0 || Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_J") < 0)
						{
							listShip[idFighter].scoreFight++;
						}
					}
					else if (Input.GetAxis(listShip[idFighter].controleurJoueur + "_ChangeCorridor_J") == 0 && dPadPressedFighter) dPadPressedFighter = false;
					if (Input.GetButtonDown(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_K") || (Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_J") != 0 && !dPadPressedDefenser))
					{
						dPadPressedDefenser = true;
						if (Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_K") > 0 || Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_J") > 0)
						{
							listShip[idDefenser].scoreFight++;
						}
					}
					else if (Input.GetAxis(listShip[idDefenser].controleurJoueur + "_ChangeCorridor_J") == 0 && dPadPressedDefenser) dPadPressedDefenser = false;

				}
				Compteur.GetComponent<TextMeshProUGUI>().text = "J"+ listShip[idDefenser].idJoueur + " "+ listShip[idDefenser].scoreFight + "  /  " + "J" + listShip[idFighter].idJoueur + " "+ listShip[idFighter].scoreFight;
			}

		}

		foreach (Ship ship in listShip)
		{
			if (ship.score > 9000)
			{
				finished = true;
			}
		}
	}

	// permet d'obtenir la liste des ship
	public List<Ship> getListShip()
	{
		return listShip;
	}

	// permet d'obtenir la taille en hauteur de la caméra
	public float getCameraHeight()
	{
		float height = 2f * cam.orthographicSize;
		return height;
	}

	// permet d'obtenir la taille en largeur de la caméra
	public float getCameraWidth()
	{
		float width = getCameraHeight() * cam.aspect;
		return width;
	}

	public void isCorridorUse(int idJoueur, int CorridorVisee)
	{
		this.CorridorVisee = CorridorVisee;
		switch (idJoueur)
		{
			case 1:
				idFighter = 1;
				idDefenser = 0;
				break;
			case 2:
				idFighter = 0;
				idDefenser = 1;
				break;
		}

		float ecart2 = listShip[1].transform.position.x - listShip[0].transform.position.x;
		if (ecart2 < 0) ecart2 = -ecart2;
		if (ecart2 < listShip[0].transform.localScale.x)
		{
			if (listShip[idDefenser].getActualCorridor() == CorridorVisee)
			{
				if (!listShip[idFighter].isFigtingInCooldown)
				{
					if (listShip[idDefenser].getActualCorridor() < listShip[idFighter].getActualCorridor())
					{
						listShip[idDefenser].transform.position = new Vector3(listShip[idDefenser].transform.position.x, couloirs.couloirsList[CorridorVisee].y - listShip[idFighter].transform.localScale.x / 2, listShip[idDefenser].transform.position.z);
						listShip[idFighter].transform.position = new Vector3(listShip[idFighter].transform.position.x, couloirs.couloirsList[CorridorVisee].y + listShip[idFighter].transform.localScale.x / 2, listShip[idFighter].transform.position.z);

						activeSpark = Instantiate(spark);
						activeSpark.transform.position = new Vector3(listShip[idFighter].transform.position.x, listShip[idFighter].transform.position.y - transform.localScale.y / 2, listShip[idFighter].transform.position.z);
						listShip[idFighter].fightingUp = false;
						listShip[idDefenser].fightingUp = true;
					}
					else
					{
						listShip[idDefenser].transform.position = new Vector3(listShip[idDefenser].transform.position.x, couloirs.couloirsList[CorridorVisee].y + listShip[idFighter].transform.localScale.x / 2, listShip[idDefenser].transform.position.z);
						listShip[idFighter].transform.position = new Vector3(listShip[idFighter].transform.position.x, couloirs.couloirsList[CorridorVisee].y - listShip[idFighter].transform.localScale.x / 2, listShip[idFighter].transform.position.z);
						activeSpark = Instantiate(spark);
						activeSpark.transform.position = new Vector3(listShip[idFighter].transform.position.x, listShip[idFighter].transform.position.y + transform.localScale.y / 2, listShip[idFighter].transform.position.z);
						listShip[idFighter].fightingUp = true;
                        listShip[idDefenser].fightingUp = false;

                    }

                    isInFight = true;
                    timerFightBegin = Time.time;
                    listShip[idFighter].cooldownFightBegin = Time.time;
                    listShip[idFighter].isFigtingInCooldown = true;
                }
                else
                {
                    listShip[idFighter].isThereAndCooldown = true;
                }
            }
			else  {
				listShip[idFighter].isThereAndCooldown = false;
			}
        }
		else
		{
			listShip[idFighter].isThereAndCooldown = false;
		}

	}

    public float getHighestSpeed()
    {
        float f = 0;
        foreach(Ship ship in listShip)
        {
            if (ship.score> f) f = ship.score;
        }
        return f;
    }
}

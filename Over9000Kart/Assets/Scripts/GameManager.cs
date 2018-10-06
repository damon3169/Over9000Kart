using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //variables du jeu
    public bool finished;

    // variables statiques du vaisseau
    public float speedMin; // vitesse minimale à laquelle un vaisseau peut aller (peut être négative)
    public float speedMax; // vitesse maximale à laquelle un vaisseau peut aller
    public float frein; // frein naturel contre l'acceleration
    public float acceleration; // modificateur d'acceleration d'un vaisseau
    public float bufferSpeed; // buffer nécéssaire de mash de bouton avant de commencer à accelerer
    public float xMin; // position x minimale d'un vaisseau sur l'écran

    List<Ship> listShip; // liste des vaisseaux dans une partie

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
    const float starGenerationCooldown = 0.01f; 
	public GameObject background;
	public float randomDistance = 3;

    // main camera
	public Camera cam;

    // singleton gamemanager
	public static GameManager instance = null;

	void Awake()
	{
		cam = Camera.main; // assigne la main camera
		
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
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
		listShip[0].setActualCorridor(couloirs.couloirsList.Count - 1);
		listShip[0].transform.position = new Vector3(cam.transform.position.x - width / 2+2, couloirs.couloirsList[couloirs.couloirsList.Count - 1].y, -1);
		listShip[1].setActualCorridor(0);
		listShip[1].transform.position = new Vector3(cam.transform.position.x - width / 2+2, couloirs.couloirsList[0].y, -1);
	}

    private float random_width()
    {
        return cam.transform.position.x - cam.orthographicSize * cam.aspect + Random.Range(0.0f, cam.orthographicSize * cam.aspect * 2);
    }

    private float random_height()
    {
        return cam.transform.position.y - cam.orthographicSize + Random.Range(0.0f, cam.orthographicSize * 2);
    }

    void Start () {
        finished = false;

        uiManager = GameObjectUIManager.GetComponent<UiManager>(); // on récupère l'uiManager
       
		timerObstaclesBegin = 0;
		timerObstacles = Random.Range(1, timerObstaclesRange);
		usedCouloirs = new List<int>(); // intialisation de la liste des couloirs

        int nbStars = Random.Range(500, 5000); // nombre aléatoire d'étoiles à l'écran

        for (int i = 0; i < nbStars; i++) // génération des étoiles sur le background
        {
            GameObject newStar = Instantiate(star);
            newStar.transform.position = new Vector3(random_width(), random_height(), -1);
        }
    }

	void Update () {
        if (Time.time > timerObstaclesBegin + timerObstacles && !finished)
        {
            timerObstacles = Random.Range(1, timerObstaclesRange + 1);
            numberObstacle = Random.Range(1, couloirs.numberOfCorridor);
            timerObstaclesBegin = Time.time;
            usedCouloirs.Clear();
            //Debug.Log(numberObstacle);
            for (int i = 0; i < numberObstacle; i++)
            {
                isObstacleSpawn = false;
                while (!isObstacleSpawn)
                {
                    SpawnIn = Random.Range(0, couloirs.numberOfCorridor);
                    if (usedCouloirs == null || !usedCouloirs.Contains(SpawnIn))
                    {
                        usedCouloirs.Add(SpawnIn);
                        GameObject obst = Instantiate(obstacle);
                        float height = 2f * cam.orthographicSize;
                        float width = height * cam.aspect;
                        obst.transform.position = new Vector3(cam.transform.position.x + width / 2 + 2, couloirs.couloirsList[SpawnIn].y, couloirs.couloirsList[SpawnIn].z);
                        isObstacleSpawn = true;
                    }
                }
            }
        }

        timeSinceLastStarGenerated += Time.deltaTime;
        if (timeSinceLastStarGenerated >= starGenerationCooldown)
        {
            timeSinceLastStarGenerated = 0.0f;
            GameObject newStar = Instantiate(star);
            newStar.transform.position = new Vector3 (cam.transform.position.x + cam.orthographicSize * cam.aspect, random_height(), -1);
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
}

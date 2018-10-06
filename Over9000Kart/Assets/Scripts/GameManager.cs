using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    List<Ship> listShip; // liste des vaisseaux dans une partie
    public GameObject GameObjectUIManager;
    UiManager uiManager;
	public couloirs couloirs;
	public float timerObstaclesRange = 5;
	public float timerObstacles;
	private float timerObstaclesBegin;
	private int numberObstacle;
	private List<int> usedCouloirs;
	public GameObject obstacle;
    public GameObject star;
	private int SpawnIn;
	private bool isObstacleSpawn;
	public GameObject background;
	Camera cam;

    float timeSinceLastStarGenerated = 0.0f;
    const float starGenerationCooldown = 0.01f;

    private float random_width()
    {
        return cam.transform.position.x - cam.orthographicSize * cam.aspect + Random.Range(0.0f, cam.orthographicSize * cam.aspect * 2);
    }

    private float random_height()
    {
        return cam.transform.position.y - cam.orthographicSize + Random.Range(0.0f, cam.orthographicSize * 2);
    }

    // Use this for initialization
    void Start () {
        uiManager = GameObjectUIManager.GetComponent<UiManager>();
        listShip = new List<Ship>(); // initalisation de la liste
        GameObject[] tempListShip = GameObject.FindGameObjectsWithTag("Ship"); // tableau temporaire
        foreach(GameObject ship in tempListShip)
        {
            listShip.Add(ship.GetComponent<Ship>());
        }
		timerObstaclesBegin = 0;
		timerObstacles = Random.Range(1, timerObstaclesRange);
		usedCouloirs = new List<int>();
		cam = Camera.main;

        int nbStars = Random.Range(500, 5000);

        for (int i = 0; i < nbStars; i++)
        {
            GameObject newStar = Instantiate(star);
            newStar.transform.position = new Vector3(random_width(), random_height(), -1);
        }
    }

	// Update is called once per frame
	void Update () {
		foreach(Ship ship in listShip)
        {
            switch(ship.idJoueur)
            {
                case 1:
                    uiManager.textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = ship.speed.ToString();
                    break;
                case 2:
                    uiManager.textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = ship.speed.ToString();
                    break;
            }
        }

        if (Time.time > timerObstaclesBegin + timerObstacles)
        {
            timerObstacles = Random.Range(1, timerObstaclesRange + 1);
            numberObstacle = Random.Range(1, couloirs.numberOfCorridor);
            timerObstaclesBegin = Time.time;
            usedCouloirs.Clear();
            Debug.Log(numberObstacle);
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
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    List<Ship> listShip; // liste des vaisseaux dans une partie
    public GameObject GameObjectUIManager;
    UiManager uiManager;
	public couloirs couloirs;
	public float timerObstaclesRange = 10;
	public float timerObstacles;
	private float timerObstaclesBegin;
	private int numberObstacle;
	private List<int> usedCouloirs;
	public GameObject obstacle;
	private int SpawnIn;
	private bool isObstacleSpawn;
	public GameObject background;


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
	}

	// Update is called once per frame
	void Update () {
		for(int i=0; i<listShip.Count; i++)
        {
            switch(i)
            {
                case 0:
                    uiManager.textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = listShip[i].speed.ToString();
                    break;
                case 1:
                    uiManager.textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = listShip[i].speed.ToString();
                    break;
            }
        }

		if (Time.time > timerObstaclesBegin + timerObstacles) {
			timerObstacles = Random.Range(1, timerObstaclesRange+1);
			numberObstacle = Random.Range(1, couloirs.numberOfCorridor);
			timerObstaclesBegin = Time.time;
			usedCouloirs.Clear();
			Debug.Log(numberObstacle);
			for (int i = 0; i< numberObstacle; i++)
			{
				isObstacleSpawn = false;
				while (!isObstacleSpawn) { 
					 SpawnIn= Random.Range(0, couloirs.numberOfCorridor);
					if (usedCouloirs == null || !usedCouloirs.Contains(SpawnIn))
					{
						usedCouloirs.Add(SpawnIn);
						GameObject obst = Instantiate(obstacle);
						obst.transform.position= new Vector3(background.transform.position.x+background.transform.localScale.x/2 + 2, couloirs.couloirsList[SpawnIn].y, couloirs.couloirsList[SpawnIn].z);
						isObstacleSpawn = true;
					}
				}
			}
		}


	}
}

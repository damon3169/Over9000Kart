using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    List<Ship> listShip; // liste des vaisseaux dans une partie
    public GameObject GameObjectUIManager;
    UiManager uiManager; 

	// Use this for initialization
	void Start () {
        uiManager = GameObjectUIManager.GetComponent<UiManager>();
        listShip = new List<Ship>(); // initalisation de la liste
        GameObject[] tempListShip = GameObject.FindGameObjectsWithTag("Ship"); // tableau temporaire
        foreach(GameObject ship in tempListShip)
        {
            listShip.Add(ship.GetComponent<Ship>());
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
	}
}

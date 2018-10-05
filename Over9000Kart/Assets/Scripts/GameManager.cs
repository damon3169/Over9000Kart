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
	}
}

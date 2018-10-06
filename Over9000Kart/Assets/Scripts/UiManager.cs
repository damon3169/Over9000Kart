using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour {

    public GameObject textSpeedJ1Value;
    public GameObject textSpeedJ2Value;
	
	void Update () {
        foreach (Ship ship in GameManager.instance.getListShip()) // pour chaque ship in game
        {
            switch (ship.idJoueur) // selon son numéro de joueur
            {
                case 1:
                    // on met à jour son score sur l'interface
                    textSpeedJ1Value.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(ship.score).ToString(); 
                    break;
                case 2:
                    textSpeedJ2Value.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(ship.score).ToString();
                    break;
            }
        }
    }
}

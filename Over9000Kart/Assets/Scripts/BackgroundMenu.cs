using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMenu : MonoBehaviour {
    // stars et background
    public GameObject star;
    float timeSinceLastStarGenerated = 0.0f;
    float starGenerationCooldown = 0.01f;
    public GameObject background;
    public float randomDistance = 3;
    public float starSpeed;
    public float starSize;

    public Camera cam;

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
        cam = Camera.main;
        Star[] stars = FindObjectsOfType<Star>();
        for (int i = 0; i < stars.Length; i++)
        {
            Destroy(stars[i]);
        }

        int nbStars = Random.Range(500, 2000); // nombre aléatoire d'étoiles à l'écran

        for (int i = 0; i < nbStars; i++) // génération des étoiles sur le background
        {
            GameObject newStar = Instantiate(star);
            newStar.transform.position = new Vector3(random_width(), random_height(), 0);
        }

        starSize = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        starSpeed = 1;
        starSize = 0.2f;

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
    }
}

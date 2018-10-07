using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Star
{
    // sprites disponibles
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    SpriteRenderer spriteRenderer;

    AudioSource source;

    // lorsque l'obstacle entre en collision avec quelque chose
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ship") // si c'est un vaisseau
        {
            collision.gameObject.GetComponent<Ship>().drawback(); // on fait reculer le vaisseau touché
            //source.pitch = Random.Range(0, 5);
            source.PlayOneShot(GameManager.instance.collision, 0.01f);
            Destroy(this.gameObject); // on détruit l'obstacle 
        }
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // on récupère le sprite dans le gameobject vide fils
        int r = Random.Range(1, 4); // on génère un nombre entre 1 et 3
        switch(r) // selon le nombre obtenu on applique le sprite 1 2 ou 3 à l'obstacle
        {
            case 1:
                spriteRenderer.sprite = sprite1;
                break;
            case 2:
                spriteRenderer.sprite = sprite2;
                break;
            case 3:
                spriteRenderer.sprite = sprite3;
                break;
        }

        source.pitch = Random.Range(0.5f, 3);
        source.PlayOneShot(GameManager.instance.comet,0.1f);

        // génère une rotation aléatoire à la création de l'obstacle
        spriteRenderer.transform.Rotate(Vector3.forward*Random.Range(0,360));
    }

    void Update()
    {
        // déplace l'obstacle de la droite vers la gauche
        transform.Translate(Vector2.left * Time.deltaTime * 10.0f);
        // rotate l'obstacle sur lui même d'une vitesse aléatoire de 2 à 15)
        spriteRenderer.transform.Rotate(Vector3.forward*Random.Range(2,16));

        // si l'obstacle sort de l'écran
        if (transform.position.x + transform.localScale.x / 2 < GameManager.instance.cam.transform.position.x - GameManager.instance.getCameraWidth() / 2)
        {
            Destroy(this.gameObject); // on le détruit
        }
    }
}

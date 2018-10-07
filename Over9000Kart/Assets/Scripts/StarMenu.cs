using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMenu : MonoBehaviour
{

    Camera cam;
    // Use this for initialization
    void Start() { cam = Camera.main; }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * 1);

        transform.localScale = new Vector3(0.2f, 0.1f, 0.1f);

        if (transform.position.x + transform.localScale.x / 2 < cam.transform.position.x - getCameraWidth() / 2)
        {
            Destroy(this.gameObject);
        }
    }

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

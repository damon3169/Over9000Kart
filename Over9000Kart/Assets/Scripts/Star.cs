using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * GameManager.instance.starSpeed);

        transform.localScale = new Vector3 (GameManager.instance.starSize, 0.1f, 0.1f);

        if (transform.position.x + transform.localScale.x / 2 < GameManager.instance.cam.transform.position.x - GameManager.instance.getCameraWidth() / 2)
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Star
{
    void destroyObject()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ("MainCamera"))
        {
            destroyObject();
        }
        if (collision.gameObject.tag == "Ship")
        {
            Debug.Log("collision");
            collision.gameObject.GetComponent<Ship>().drawback();
            
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            if (transform.position.x + transform.localScale.x / 2 < GameManager.instance.cam.transform.position.x - GameManager.instance.getCameraWidth() / 2)
            {
                Destroy(this.gameObject);

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Star
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            Debug.Log("collision");
            collision.gameObject.GetComponent<Ship>().drawback();
            
            transform.Translate(Vector2.left * Time.deltaTime * speed);

            Destroy(this.gameObject);
        }
    }
}

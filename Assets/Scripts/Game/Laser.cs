using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f; // laser speed

    private bool _isEnemyLaser = false; // is the laser an enemy laser

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = _isEnemyLaser ? Vector3.down : Vector3.up;
        transform.Translate(_speed * Time.deltaTime * direction);

        // destroy the laser when it leaves the screen
        if (transform.position.y > 8.0f || transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the laser is an enemy laser and the player is hit
        if (other.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
        // if the laser is a player laser and an enemy is hit
        //else if (other.tag == "Enemy" && !_isEnemyLaser)
        //{
        //    Enemy enemy = other.GetComponent<Enemy>();
        //    if (enemy != null)
        //    {
        //        enemy.Damage();
        //    }
        //    Destroy(this.gameObject);
        //}
    }

    public void SetEnemyLaser()
    {
        _isEnemyLaser = true;
    }
}

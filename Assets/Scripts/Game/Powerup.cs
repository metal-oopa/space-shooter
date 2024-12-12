using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f; // powerup speed

    [SerializeField]
    private AudioClip _audioClip; // audio clip

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // move down at 3 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // destroy ourselves when we leave the screen
        if (transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // play powerup sound
            AudioSource.PlayClipAtPoint(_audioClip, transform.position);
            // access the player
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                // switch on powerup type
                switch (this.gameObject.name)
                {
                    case "Triple_Shot_Powerup(Clone)":
                        player.TripleShotPowerup();
                        break;
                    case "Speed_Powerup(Clone)":
                        player.SpeedPowerup();
                        break;
                    case "Shield_Powerup(Clone)":
                        player.ShieldPowerup();
                        break;
                    default:
                        Debug.Log("Unknown powerup type: " + this.gameObject.name);
                        break;
                }
            }

            // destroy ourselves
            Destroy(this.gameObject);
        }
    }
}

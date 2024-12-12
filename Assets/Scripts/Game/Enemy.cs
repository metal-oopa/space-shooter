using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f; // enemy speed
    [SerializeField]
    private GameObject _laserPrefab; // laser prefab


    private Player _player; // player
    private Animator _animator; // animator
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;

    // Start is called before the first frame update
    private void Start()
    {
        float randomX = Random.Range(-9.0f, 9.0f);
        transform.position = new Vector3(randomX, 7.5f, 0);
        GetAnimator();
        GetPlayer();
        GetAudioSource();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();

        FireLaser();
    }

    private void Move()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -5.0f)
        {
            float randomX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomX, 7.5f, 0);
        }
    }

    private void FireLaser()
    {
        if (Time.time > _fireRate && _speed != 0)
        {
            _fireRate = Time.time + Random.Range(3f, 7f);
            GameObject Laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = Laser.GetComponentsInChildren<Laser>();

            foreach (Laser laser in lasers)
            {
                laser.SetEnemyLaser();
            }
        }
    }

    private void GetAudioSource()
    {
        if (!TryGetComponent<AudioSource>(out _audioSource))
        {
            Debug.LogError("The AudioSource is NULL.");
        }
    }

    private void GetAnimator()
    {
        if (!TryGetComponent<Animator>(out _animator))
        {
            Debug.LogError("The Animator is NULL.");
        }
    }

    private void GetPlayer()
    {
        GameObject PlayerObject = GameObject.Find("Player");

        if (PlayerObject)
        {
            if (!PlayerObject.TryGetComponent<Player>(out _player))
            {
                Debug.LogError("The Player Component is NULL.");
            }
        }
        else
        {
            Debug.LogError("The Player is NULL.");
        }
    }

    private void AddScore()
    {
        int points = Random.Range(5, 11);
        _player.AddScore(points);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                player.Damage();
            }
            TriggerAnimationAndDestroy();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            // add 10 to the score
            if (_player != null)
            {
                AddScore();
            }
            TriggerAnimationAndDestroy();
        }
    }

    private void TriggerAnimationAndDestroy()
    {
        _audioSource.Play();
        _animator.SetTrigger("OnEnemyDeath");
        _speed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2f);
    }
}
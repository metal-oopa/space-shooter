using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f; // player speed

    [SerializeField]
    private float _speedMultiplier = 2f; // speed multiplier

    [SerializeField]
    private float _speedBoostDuration = 5f; // speed boost duration

    [SerializeField]
    private GameObject _laserPrefab; // laser prefab

    [SerializeField]
    private GameObject _tripleShotPrefab; // triple shot prefab

    [SerializeField]
    private float _fireRate = 0.5f; // fire rate
    private float _nextFire = -1f; // can fire

    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private bool _isTripleShotActive = false;

    [SerializeField]
    private bool _isShieldActive = false;
    private GameObject _leftEngineDamage,
        _rightEngineDamage;

    [SerializeField]
    private AudioClip _laserSoundClip;

    [SerializeField]
    private GameObject _shieldVisualizer;
    
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;

    private int _score;

    // Start is called before the first frame update
    void Start()
    {
        _score = 0;

        if (!GameObject.Find("Game_Manager").TryGetComponent<GameManager>(out _gameManager))
        {
            Debug.LogError("The Game Manager is NULL.");
        }


        if (!GameObject.Find("Spawn_Manager").TryGetComponent<SpawnManager>(out _spawnManager))
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        _shieldVisualizer = transform.Find("Shield").gameObject;
        _shieldVisualizer.SetActive(false);

        
        if (!GameObject.Find("Canvas").TryGetComponent<UIManager>(out _uiManager))
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        
        if (!TryGetComponent<AudioSource>(out _audioSource))
        {
            Debug.LogError("The Audio Source is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (!_gameManager.IsCoOpMode())
        {
            // set the player's position to the origin
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Time.time > _nextFire)
        {
            FireLaser();
        }

#else
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
#endif

    }

    void CalculateMovement()
    {

#if UNITY_ANDROID || UNITY_IOS
        // move player on tilt input
        float tiltInput = Input.acceleration.x;
        transform.Translate(Vector3.right * tiltInput * _speed * Time.deltaTime);

#else
        // player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // move the player
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        // player boundaries
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, -3.8f, 0),
            0
        );

#endif

        // player boundaries
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }

    private void GetEngineDamageVisualizer()
    {
        _leftEngineDamage = transform.Find("Left_Engine_Damage").gameObject;
        _rightEngineDamage = transform.Find("Right_Engine_Damage").gameObject;

        if (_leftEngineDamage == null)
        {
            Debug.LogError("The Left Engine Damage is NULL.");
        }

        if (_rightEngineDamage == null)
        {
            Debug.LogError("The Right Engine Damage is NULL.");
        }
    }

    void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(
                _laserPrefab,
                transform.position + new Vector3(0, 1.05f, 0),
                Quaternion.identity
            );
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            return;
        }
        lives--;
        EngineDamageActivate();
        _uiManager.UpdateLives(lives);

        if (lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            UpdateBestScore();
            Destroy(this.gameObject);
        }
    }

    private void EngineDamageActivate()
    {
        if (_leftEngineDamage == null || _rightEngineDamage == null)
        {
            GetEngineDamageVisualizer();
        }

        if (lives == 2)
        {
            _leftEngineDamage.SetActive(true);
        }
        else if (lives == 1)
        {
            _rightEngineDamage.SetActive(true);
        }
    }

    public void TripleShotPowerup()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedPowerup()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(_speedBoostDuration);
        _speed /= _speedMultiplier;
    }

    public void ShieldPowerup()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);

        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _shieldVisualizer.SetActive(false);
        _isShieldActive = false;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void UpdateBestScore()
    {
            _uiManager.UpdateBestScore(_score);
    }
}

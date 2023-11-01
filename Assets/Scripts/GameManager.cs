using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum RandomSpawnerShape
{
    Box,
    Sphere,
}
public class GameManager : MonoBehaviour
{
    #region CLASS INSTANCE
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    public int CurrentLevel;
    public GameObject DeathScreen;
    public CameraShaker CameraShaker;
    [Header("Player SETTINGS")]
    #region Player
    bool isDead = false;
    public PlaneController planeController;
    public bool allowSlowTime;
    public int maxHealthPlayer;
    public PlaneHealth PlaneHealth;
    public int killCounter;
    public int minUpgradeKills;
    public int minHealthKills;
    #endregion
    [Header("AI SETTINGS")]
    #region AI

    public float NormalAISpeed;
    public float RedAISpeed;
    public float timeBetweenAttackAI;
    public int maxHealthAI;
    public List<GameObject> AiPlayers;
    public List<Transform> AiSpawnPositions;
    public List<int> AI_PerLevel;
    public int AI_countThisLevel;
    public float NextAiSpawnAfter;
    public int maxAIcount;
    public int AIWithBulletNum;


    public int AiCount;
    
    public float AiTimer;
    
    #endregion

    #region
    [Header("DAMAGE(IN PERCENT)")]
    public List<int> DamageList;
    public List<int> DamageListAI;
    #endregion

    #region
    [Header("PAUSE MENU")]
    public bool isPaused;
    public GameObject PauseMenu;
    #endregion
    [Header("Asteroid Spawning General settings:")]

    public Transform prefab;
    public Transform AsteroidObj;
    public RandomSpawnerShape spawnShape = RandomSpawnerShape.Sphere;

    public Vector3 shapeModifiers = Vector3.one;

    public int asteroidCount = 50;
    public float range = 1000.0f;

    public bool randomRotation = true;
    public Vector2 scaleRange = new Vector2(1.0f, 3.0f);

    public float velocity = 0.0f;
    public float angularVelocity = 0.0f;
    public bool scaleMass = true;


    void Start()
    {
        if (prefab != null)
        {
            for (int i = 0; i < asteroidCount; i++)
                CreateAsteroid();
        }
    }
    private void Update()
    {


        if (!isDead && AiTimer > NextAiSpawnAfter && AiCount <= AI_PerLevel[CurrentLevel])
        {
            SpawnAIPlayer(1);

            AiTimer = 0f;
        }
        
        AiTimer += Time.deltaTime;


        if (allowSlowTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                planeController.isHoldingCtrl = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                planeController.isHoldingCtrl = false;
            }
        }


 
    }

    private void SpawnAIPlayer(int count)
    {
        if ((killCounter > 0 && AIWithBulletNum > 0) && (killCounter % AIWithBulletNum == 0))
        {
            for (int i = 0; i < count; i++)
            {
                int r_pos = Random.Range(0, AiSpawnPositions.Count);

                Instantiate(AiPlayers[0], AiSpawnPositions[r_pos].position, AiSpawnPositions[r_pos].rotation);
                AiPlayers[0].GetComponent<AIPlaneController>().playerPlane = planeController.transform;
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int r_pl = Random.Range(1, AiPlayers.Count);
                int r_pos = Random.Range(0, AiSpawnPositions.Count);
                if (AiPlayers.Count > 0)
                    Instantiate(AiPlayers[r_pl], AiSpawnPositions[r_pos].position, AiSpawnPositions[r_pos].rotation);
                AiPlayers[r_pl].GetComponent<AIPlaneController>().playerPlane = planeController.transform;
            }
        }
        AiCount += count;

    }

  

    public void PlayerDeath()
    {
        StartCoroutine(SlowMotion(0, 2f, 0.2f));
    }

    public IEnumerator SlowMotion(int task, float delay, float slowness)
    {
        Time.timeScale = slowness;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        float slowEndTime = Time.realtimeSinceStartup + delay;

        while (Time.realtimeSinceStartup < slowEndTime)
        {
            yield return 0;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (task == 0)
        {
            //SceneManager.LoadScene(0);
            onResetLevel();
        }
        
    }
    void onResetLevel()
    {
        // You died
        // reset inspector values
        isDead = true;
        DeathScreen.SetActive(true);
        AiCount = 0;
        AI_countThisLevel = 0;
        AiTimer = 0f;

    }
    public void onResumeGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);

    }


    private void CreateAsteroid()
    {
        Vector3 spawnPos = Vector3.zero;

        if (spawnShape == RandomSpawnerShape.Box)
        {
            spawnPos.x = Random.Range(-range, range) * shapeModifiers.x;
            spawnPos.y = Random.Range(-range, range) * shapeModifiers.y;
            spawnPos.z = Random.Range(-range, range) * shapeModifiers.z;
        }
        else if (spawnShape == RandomSpawnerShape.Sphere)
        {
            spawnPos = Random.insideUnitSphere * range;
            spawnPos.x *= shapeModifiers.x;
            spawnPos.y *= shapeModifiers.y;
            spawnPos.z *= shapeModifiers.z;
        }

        spawnPos += transform.position;

        Quaternion spawnRot = (randomRotation) ? Random.rotation : Quaternion.identity;

        Transform _transform = Instantiate(prefab, spawnPos, spawnRot) as Transform;
        _transform.SetParent(AsteroidObj);

        float scale = Random.Range(scaleRange.x, scaleRange.y);
        _transform.localScale = Vector3.one * scale;

        Rigidbody _rigidbody = _transform.GetComponent<Rigidbody>();
        if (_rigidbody)
        {
            if (scaleMass)
                _rigidbody.mass *= scale * scale * scale;

            _rigidbody.AddRelativeForce(Random.insideUnitSphere * velocity, ForceMode.VelocityChange);
            _rigidbody.AddRelativeTorque(Random.insideUnitSphere * angularVelocity * Mathf.Deg2Rad, ForceMode.VelocityChange);
        }
    }

}

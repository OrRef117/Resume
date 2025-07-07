using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // For UI elements

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private StarFieldController starFieldController;
    public static SpawnManager Instance { get; private set; }

    private List<GameObject> currentLevelEnemies;
    public GameObject levelUpPanel;  // Reference to the level-up panel UI
                                     // public Text levelUpText;  // Text to display the level-up message
    private int currentLevelIndex = 0;  // Current level in the levels array
    private int enemiesRemaining;  // To track remaining enemies in the wave
    private GameObject nearestEnemy;

    public GameObject spawnEffectPrefab;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Enforce the singleton pattern
        }
    }

    void Start()
    {
        // Initialize the first level and spawn enemies
        currentLevelEnemies = new List<GameObject>();
        GameManager.Instance.SetupLevels();
        StartLevel(GameManager.Instance.levels[currentLevelIndex]);
    }

    // Method to start the level
    void StartLevel(Level level)
    {
        enemiesRemaining = level.enemyPrefabs.Length;
        StartCoroutine( SpawnEnemies(level));
    }

    // Method to spawn enemies from the level


    IEnumerator SpawnEnemies(Level level)
    {
        int count = 0;
        foreach (GameObject enemyPrefab in level.enemyPrefabs)
        {


            if (spawnEffectPrefab != null)
            {
                Instantiate(spawnEffectPrefab, level.enemySpawnPos[count], Quaternion.identity);
            }

            // Spawn the enemy
            GameObject enemy = Instantiate(enemyPrefab, level.enemySpawnPos[count], Quaternion.identity);
            currentLevelEnemies.Add(enemy);

            count++;

            // Wait before spawning the next one
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Method to handle when an enemy is destroyed
    public void EnemyDestroyed(GameObject gamObj)
    {
        currentLevelEnemies.Remove(gamObj);
        UIManager.Instance.ChangeCash();
        enemiesRemaining--;

        // Check if all enemies are destroyed
        if (enemiesRemaining <= 0)
        {
            OnWaveCompleted();
        }
    }

    // Method to handle when the wave is completed
    void OnWaveCompleted()
    {
        if (GameManager.Instance.HasLeveledUp())
        {
            ShowLevelUpPanel();
        }
        else if(isShoppingTime())
        {
            UIManager.Instance.ShoppingTime();
        }
        else
        {
            StartCoroutine(goToNextLevelEffect());

        }
    }

    // Show the level-up panel
    void ShowLevelUpPanel()
    {
        UIManager.Instance.RankUp();
    }

    // Handle when the level-up button is clicked
    public void OnLevelUpConfirmed()
    {
        levelUpPanel.SetActive(false);  // Hide the level-up panel
        GameManager.Instance.playerExperience = 0;  // Reset XP after level up
        GameManager.Instance.SetExpTarget();
        OnWaveCompleted();
    }


    IEnumerator goToNextLevelEffect()
    {
        starFieldController.SetSpeedInstantly(5f);

        yield return new WaitForSeconds(2.4f);
        starFieldController.SetSpeedInstantly(1f);

        yield return new WaitForSeconds(0.4f);

        NextLevel();
    }

    // Move to the next level
    void NextLevel()
    {
        currentLevelIndex++;

        // Check if there are more levels to go
        if (currentLevelIndex < GameManager.Instance.levels.Length)
        {
            StartLevel(GameManager.Instance.levels[currentLevelIndex]);
        }
        else
        {
            Debug.Log("All levels completed!");
            GoNextTier();
            // cutascene//
        }
    }

    public bool isShoppingTime()
    {
        if(currentLevelIndex == 4|| currentLevelIndex == 8|| currentLevelIndex == 9 || (GameManager.Instance.playerTier > 1 && currentLevelIndex == 0))
        {
            return true;
        }

        return false;
    }

    public void GoNextTier()
    {
        GameManager.Instance.currentTierMultiplyer += 0.25f;
        GameManager.Instance.playerTier++;
    }

    public void SpawnMeteor(GameObject enemy)
    {

        //spawn meatball
    }



    public Vector3 FindNearestEnemy(Collider2D collision)
    {
        float closestDistance = Mathf.Infinity; // Start with an infinitely large distance
        nearestEnemy = null;
        int index = 0;
        if (currentLevelEnemies.Contains(collision.gameObject))
        {
            index = currentLevelEnemies.IndexOf(collision.gameObject);


        }

        // Iterate through all the enemies to find the nearest one
        foreach (GameObject enemy in currentLevelEnemies)
        {
            if (!GameObject.Equals(enemy, collision.gameObject))
            {


                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy;  // Set the closest enemy
                }
            }
        }
        return nearestEnemy.transform.position;

    }
}

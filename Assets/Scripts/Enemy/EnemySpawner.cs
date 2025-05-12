using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    class EnemiesT{
        public int[] Enemies;
    }
    // Public
    public Text waveText;
    public bool jopas;
    // Private
    private List<GameObject> waypoints;
    private GameObject enemyPrefab;
    [SerializeField]private float spawnDelay = 1f;
    [SerializeField]private int waves = 2;
    private int maxEnemy=5;
    private EnemiesT enemies;
    private int index2=0;
    private int myInt;
    List<GameObject> GetAllChildren(GameObject obj)
    {
        List<GameObject> children = new List<GameObject>();
        foreach(Transform child in obj.transform)
        {
            children.Add(child.gameObject);
            children.AddRange(GetAllChildren(child.gameObject));
        }
        return children;
    }
    private void Awake()
    {
        myInt = MyVariables._int;
    }

    // Start
    void Start()
    {
        enemyPrefab = Resources.Load<GameObject>("Enemy");
        waypoints = GetAllChildren(transform.GetChild(1).gameObject);
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab not found in Resources folder!");
            return;
        }

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Waypoints array is not assigned or empty!");
            return;
        }

        string filePath = Path.Combine(Application.dataPath, "Data", "Waves " + myInt + ".json");

        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError("JSON file does NOT exist at path: " + filePath);
                return;
            }

            string str = File.ReadAllText(filePath);
            enemies = JsonUtility.FromJson<EnemiesT>(str);

            if (enemies == null)
            {
                Debug.LogError("Failed to parse JSON - enemies is null");
                return;
            }

            if (enemies.Enemies == null || enemies.Enemies.Length == 0)
            {
                Debug.LogError("Enemies array is null or empty after parsing!");
                return;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading JSON file: " + e.Message);
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waves; i++){
            yield return new WaitForSeconds(5f); // Задержка между волнами
            if (i+1 < maxEnemy)
                StartCoroutine(SpawnWave(i+1));
            else StartCoroutine(SpawnWave(maxEnemy));
            waveText.text = $"Wave: {i}";
        }
    }

    IEnumerator SpawnWave(int a)
    {
        for (int i=0; i < a; i++){
            try{
                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                newEnemy.GetComponent<EnemyScript>().waypoints=waypoints;
                newEnemy.GetComponent<EnemyScript>().lvl=enemies.Enemies[index2];
                index2++;
            }catch{
                jopas = true;
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
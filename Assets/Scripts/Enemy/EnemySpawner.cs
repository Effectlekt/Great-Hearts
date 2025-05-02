using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    class EnemiesT{
        public int[] Enemies;
    }
    // Public
    public GameObject[] waypoints;
    public Text waveText;

    // Private
    private GameObject enemyPrefab;
    [SerializeField]private float spawnDelay = 1f;
    [SerializeField]private int waves = 5;
    private int maxEnemy=5;
    private EnemiesT[] enemies;

    // Start
    void Start()
    {
        enemyPrefab =  Resources.Load<GameObject>("Enemy");
        string str = File.ReadAllText(Application.dataPath + "/Data/Waves.json");
        enemies = JsonUtility.FromJson<EnemiesT[]>(str);

        Debug.Log(enemies[1].Enemies[1]);
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waves; i++){
            yield return new WaitForSeconds(5f); // Задержка между волнами
            if (2*i+1 < maxEnemy)
                StartCoroutine(SpawnWave(i, 2*i+1));
            else StartCoroutine(SpawnWave(i, maxEnemy));
            waveText.text = $"Wave: {i}";
        }
    }

    IEnumerator SpawnWave(int index, int endex)
    {
        for (int i=index; i < endex; i++){
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            newEnemy.GetComponent<EnemyScript>().waypoints=waypoints;
            newEnemy.GetComponent<EnemyScript>().lvl=enemies[(endex-1)/2].Enemies[i];
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
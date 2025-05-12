using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [System.Serializable]
    public class Enemy {
        public int Speed;
        public int Health;
        public int Hide;
        public int Damage;
        public int Cost;
    }
    [System.Serializable]
    public class EnemyWrapper {
        public Enemy[] enemies;
    }
    // Waypoints
    public Enemy enemy;
    public List<GameObject> waypoints;
    public int lvl=1;
    private int currentWaypointIndex = 0;
    private Enemy[] enemies;


    void Start()
    {
        string jsonString = File.ReadAllText(Application.dataPath + "/Data/Namaz.json");
        EnemyWrapper wrapper = JsonUtility.FromJson<EnemyWrapper>(jsonString);
        Enemy[] enemies = wrapper.enemies;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("namaz"+(lvl+1));
        enemy = enemies[lvl-1];
    }
    
    void Update()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            GameObject target = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, enemy.Speed * Time.deltaTime);

            // Поворот в сторону движения (опционально)
            Vector2 direction = (target.transform.position - transform.position).normalized;
            if (direction.x > 0) transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта

            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            // Враг дошёл до конца (игрок теряет здоровье)
            Destroy(gameObject);
            GameManager.Instance.TakeDamage(enemy.Damage);
        }
    }
}
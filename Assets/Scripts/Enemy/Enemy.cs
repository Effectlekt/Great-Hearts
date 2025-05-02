using System.IO;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public class Namaz{
        // Stats
        //public float Speed;
        //public int Health;
        //public int Hide;
        //public int Damage;
        //public int Cost;
        public int[] ints;
    }

    class Enemies{
        public Namaz[] enemies;
    }

    // Waypoints
    public Namaz enemy;
    public GameObject[] waypoints;
    public int lvl=1;
    private int currentWaypointIndex = 0;
    private Enemies enemies;

    void Start()
    {
        string str = File.ReadAllText(Application.dataPath + "/Data/Namaz.json");
        enemies = JsonUtility.FromJson<Enemies>(str);
        Debug.Log(enemies.enemies[0]);
        enemy = enemies.enemies[lvl];
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("namaz"+(lvl+1));
    }

    
    void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            GameObject target = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, enemy.ints[0] * Time.deltaTime);

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
            GameManager.Instance.TakeDamage(enemy.ints[3]);
        }
    }
}
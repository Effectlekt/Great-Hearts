using System.IO;
using UnityEngine;


public class TowerScript : MonoBehaviour
{
    // Classes
    class Tower{
        public int Health;
        public int Damage;
        public int Speed;
        public int Vision;
        public float FireRate;
        public float Range;
        public int Cost;
    }
    [System.Serializable]
    class Towers{
        public Tower[] towers;
    }

    // Private
    private GameObject projectilePrefab;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private Transform target;
    private int lvl=0;
    private Tower tower;
    private Towers towers = new Towers();
    
    public int Cost() => tower.Cost;

    // Start
    void Start()
    {
        firePoint=transform.GetChild(0);
        projectilePrefab = (GameObject)Resources.Load("Projectile");
        string str = File.ReadAllText(Application.dataPath + "/Data/Tower.json");
        towers = JsonUtility.FromJson<Towers>(str);

        tower = towers.towers[lvl];
        projectilePrefab.GetComponent<Projectile>().damage = tower.Damage;
        projectilePrefab.GetComponent<Projectile>().speed = tower.Speed;
    }

    // Update
    void Update()
    {
        // Прицеливание
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = tower.Range;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && tower.Vision >= enemy.GetComponent<EnemyScript>().enemy.ints[1])
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        target = nearestEnemy != null ? nearestEnemy.transform:null;

        // Shoot
        if (target != null && fireCountdown <= 0f)
        {
            GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if (projectile != null) projectile.Seek(target);
            fireCountdown = 1f / tower.FireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    public bool Upgrade(){
        switch(lvl){
            default:
                return false;
            case 0:
                gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 4:
                gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
            case 5:
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                break;
        }
        lvl++;
        tower = towers.towers[lvl];
        return true;
    }
}
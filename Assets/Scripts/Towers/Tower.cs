using System.IO;
using UnityEngine;


public class TowerScript : MonoBehaviour
{
    // Classes
    [System.Serializable]
    public class Tower{
        public int Health;
        public int Damage;
        public int Speed;
        public int Vision;
        public float FireRate;
        public float Range;
        public int Cost;
    }
    [System.Serializable]
    public class TowerWrapper{
        public Tower[] towers;
    }

    // Private
    private GameObject projectilePrefab;
    private Transform firePoint;
    private float fireCountdown = 0f;
    private Transform target;
    private int lvl=0;
    private Tower tower;
    private TowerWrapper towers;
    
    public int Cost() => tower.Cost;

    // Start
    void Start()
    {
        firePoint=transform.GetChild(0);
        projectilePrefab = (GameObject)Resources.Load("Projectile");
        string str = File.ReadAllText(Application.dataPath + "/Data/Tower.json");
        TowerWrapper wrapper = JsonUtility.FromJson<TowerWrapper>(str);
        towers = wrapper;
        tower = wrapper.towers[lvl];
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
            if (distanceToEnemy < shortestDistance && tower.Vision >= enemy.GetComponent<EnemyScript>().enemy.Hide)
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
        try{
            lvl++;
            tower = towers.towers[lvl];
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tower"+(lvl+1));
        }catch{
            return false;
        }
        return true;
    }
}
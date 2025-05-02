using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Public
    public void Seek(Transform _target) => target = _target;

    // Private
    public float speed = 5f;
    public int damage = 10;
    private Transform target;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Поворот снаряда в сторону цели (опционально)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            EnemyScript enemy = other.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.enemy.ints[1]-=damage;
                if (enemy.enemy.ints[1]<=0) {
                    Destroy(enemy.gameObject);
                    GameManager.Instance.AddMoney(enemy.enemy.ints[4]);
                }
            }
            Destroy(gameObject);
        }
    }
}
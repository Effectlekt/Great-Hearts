using Unity.Loading;
using UnityEditor;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]private GameObject asd;
    private GameObject game;
    private int jopa;
    private void Start() {
        game = asd.transform.GetChild(Camera.main.GetComponent<GameManager>().myInt).gameObject;
    }
    void Update()
    {
        jopa = Camera.main.GetComponent<GameManager>().playerHealth;
        if (jopa <= 0){
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (game.GetComponent<EnemySpawner>().jopas){
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

    }
}

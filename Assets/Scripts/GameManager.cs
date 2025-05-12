using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager Instance; // Возможность вызова из класса

    // Private
    private int money=100; // Деньги
    public int playerHealth = 100;  // Здоровье
    public Text moneyText;
    public Text healthText;
    public GameObject _EnemySpawner;
    public GameObject _Grid;
    public int myInt;
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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize UI text
        moneyText.text = $"$: {money}";
        healthText.text = $"HP: {playerHealth}";
        myInt = MyVariables._int;
        // Now safely get children
        _EnemySpawner.transform.GetChild(myInt).gameObject.SetActive(true);
        _Grid.transform.GetChild(myInt).gameObject.SetActive(true);
    }
 
    public void AddMoney(int value) {money += value; moneyText.text = $"$: {money}";}

    public int GetMoney() => money; 

    public void TakeDamage(int damage) 
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Debug.Log("Game Over!");
            playerHealth=0;
            // Тут можно вызвать экран поражения
        }
        healthText.text = $"HP: {playerHealth}";
    }
}
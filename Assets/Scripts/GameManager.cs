using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager Instance; // Возможность вызова из класса

    // Private
    private int money=100; // Деньги
    private int playerHealth = 100;  // Здоровье
    public Text moneyText;
    public Text healthText;

    void Awake()
    {

        moneyText.text = $"$: {money}";
        healthText.text = $"HP: {playerHealth}";
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
 
    public void AddMoney(int value) {money += value; moneyText.text = $"$: {money}";}

    public int GetMoney() => money; 

    public void TakeDamage(int damage) 
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Debug.Log("Game Over!");
            // Тут можно вызвать экран поражения
        }
        healthText.text = $"HP: {playerHealth}";
    }
}
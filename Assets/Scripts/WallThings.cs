using System;
using UnityEngine;

public class WallThings : MonoBehaviour
{
    public int Health {  get; private set; }
    [SerializeField] int maxHealth;
    public event Action WallIsGone;
    public static WallThings instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Health = maxHealth;
        UIManager.Instance.ChangeWallHealth(Health, maxHealth);

    }

    public void ChangeHealth(int healthChangeAmount)
    {
        Health += healthChangeAmount;
        if (Health <= 0)
        {
            WallIsGone?.Invoke();
            TheDirector.Instance.SetGameState(TheDirector.GameState.GameOver);
        }
        UIManager.Instance.ChangeWallHealth(Health, maxHealth);
    }

}

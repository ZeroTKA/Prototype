using System;
using UnityEngine;

public class WallThings : MonoBehaviour
{
    public int Health {  get; private set; }
    [SerializeField] int maxHealth;
    public event Action WallIsGone;
    public static WallThings instance;

    float timer = 0;

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
    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void ChangeHealth(int healthChangeAmount)
    {
        Health += healthChangeAmount;
        if (Health <= 0)
        {
            WallIsGone?.Invoke();
            TheDirector.Instance.SetGameState(TheDirector.GameState.GameOver);
        }
        if (timer >= .1f)
        {
            UIManager.Instance.ChangeWallHealth(Health, maxHealth);
            timer = 0;
        }
    }

}

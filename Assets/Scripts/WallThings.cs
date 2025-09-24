using System;
using UnityEngine;

public class WallThings : MonoBehaviour
{
    public int Health {  get; private set; }
    [SerializeField] int maxHealth;
    public event Action WallIsGone;
    public static WallThings instance;

    private int pendingHealthChange = 0;
    private float timer = 0f;

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
   // void Update()
   // {
   //     timer += Time.deltaTime;
   //     if (pendingHealthChange != 0)
   //     {
   //         Health += pendingHealthChange;
   //         Health = Mathf.Clamp(Health, 0, maxHealth);
   //         if (Health == 0)
   //         {
   //             WallIsGone?.Invoke();
   //             TheDirector.Instance.SetGameState(TheDirector.GameState.GameOver);               
   //         }
   //         UpdateHealthbarUI();
   //         timer = 0f;
   //         pendingHealthChange = 0;
   //     }
   // }

    public void AddToPendingDamage(int healthChangeAmount)
    {
        pendingHealthChange += healthChangeAmount;
    }
    private void UpdateHealthbarUI()
    {
        UIManager.Instance.ChangeWallHealth(Health, maxHealth);    
    }

}

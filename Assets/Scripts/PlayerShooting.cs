using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private InputAction shootAction;
    private InputAction reloadAction;

    [SerializeField] int maxAmmo;
    private int _currentAmmo;
    private int CurrentAmmo
    {
        get => _currentAmmo;
        set
        {
            if (value < 0)
            {
                _currentAmmo = 0;
                Debug.LogError($"{value} is trying to be set for _currentAmmo. This must be a positive number");
            }
            _currentAmmo = value;
        }
    }

    private bool isMagazineEmpty = false;
    private bool areWeReloading = false;
    private bool canWeShoot = true;

    private void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Attack"];
        reloadAction = playerInput.actions["Reload"];
        CurrentAmmo = maxAmmo;
    }

    void Update()
    {
        if (shootAction.triggered && !isMagazineEmpty && !areWeReloading && canWeShoot)
        {
            Shoot();
        }
        if (reloadAction.triggered && CurrentAmmo <= maxAmmo && !areWeReloading)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        Debug.Log("Pew pew");
    }
    private void Reload()
    {
        Debug.Log("Click Clack curchunk POW");
    }
}

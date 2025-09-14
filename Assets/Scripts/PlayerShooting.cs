using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private InputAction shootAction;
    private InputAction reloadAction;

    [SerializeField] int maxAmmo;
    private int _currentAmmo; // Never use this anywhere except CurrentAmmo validation. 
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
            else
            {
                _currentAmmo = value;
            }
        }
    }
    private int _carriedAmmo;
    private int CarriedAmmo
    {
        get => _carriedAmmo;
        set
        {
            if (value < 0)
            {
                CarriedAmmo = 0;
                Debug.LogError($"{value} is trying to be set for _carriedAmmo. This must be a positive number");
            }
            else
            {
                _carriedAmmo = value;
            }
        }
    }

    private bool isMagazineEmpty = false;
    private bool areWeReloading = false;
    private bool canWeShoot = true;
    private readonly float shootSpeed = 1;
    private readonly float reloadSpeed = 1;
    

    private void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Attack"];
        reloadAction = playerInput.actions["Reload"];
        CurrentAmmo = maxAmmo;
        CarriedAmmo = 30;
    }

    void Update()
    {
        if (shootAction.triggered && !areWeReloading && canWeShoot)
        {
            if(!isMagazineEmpty)
            {
                Shoot();
            }
            else
            {
                // play dud sound
            }
        }
        if (reloadAction.triggered && CurrentAmmo < maxAmmo && !areWeReloading && CarriedAmmo > 0)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        CurrentAmmo -= 1;
        if (CurrentAmmo == 0) isMagazineEmpty = true;
        canWeShoot = false;
        StartCoroutine(ResetCanWeShootBool());
        Debug.Log($"{CurrentAmmo} bullets left");
    }
    private void Reload()
    {
        Debug.Log("Reloading");
        areWeReloading = true;
        StartCoroutine(Reloading()); // this contains all the logic because we want things to change at the END.
    }

    IEnumerator ResetCanWeShootBool()
    {
        yield return new WaitForSeconds(shootSpeed);
        canWeShoot = true;
    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadSpeed);
        areWeReloading = false;

        //If we have more than enough or exactly enough.
        if (CarriedAmmo >= maxAmmo - CurrentAmmo)
        {
            CarriedAmmo -= maxAmmo - CurrentAmmo;
            CurrentAmmo = maxAmmo;            
        }
        //if we don't have enough to fill
        else
        {
            CurrentAmmo += CarriedAmmo;
            CarriedAmmo = 0;
        }
    }
}

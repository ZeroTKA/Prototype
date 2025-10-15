using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform fpsCamera;
    private readonly int raycastDistance = 50;


    // -- Gun Stats -- //
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
    private int _spareAmmo; // Never use this anywhere except Carried Ammo
    private int SpareAmmo
    {
        get => _spareAmmo;
        set
        {
            if (value < 0)
            {
                _spareAmmo = 0;
                Debug.LogError($"{value} is trying to be set for _carriedAmmo. This must be a positive number");
            }
            else
            {
                _spareAmmo = value;
            }
        }
    }
    private int playerMask;

    private bool isMagazineEmpty = false;
    private bool areWeReloading = false;
    private bool canWeShoot = true;
    private readonly float shootSpeed = 1;
    private readonly float reloadSpeed = .5f;

    // -- Specialty Methods -- //

    private void Start()
    {
        // -- Error Checks -- //
        if (UIManager.Instance == null)
        {
            Debug.LogError("[PlayerShooting] UIManager is missing from the start");
        }
        else
        {
            UIManager.Instance.ChangeAmmoSituation(CurrentAmmo, maxAmmo);
        }
        if (TheDirector.Instance == null)
        {
            Debug.LogError("[PlayerShooting] TheDirector is missing from start. What happened?");
        }
        else
        {
            TheDirector.Instance.OnGameStateChanged += Restart;
        }
        if (fpsCamera == null)
        {
            Debug.LogError("[PlayerShooting] FPS camera is null. Did you forget to set it?");
        }
        if (maxAmmo <= 0)
        {
            Debug.LogError("Max Ammo is NOT greater than 0. Unacceptable.");
        }
        if (TryGetComponent<PlayerInput>(out var playerInput))
        {
            shootAction = playerInput.actions["Attack"];
            reloadAction = playerInput.actions["Reload"];
            if (shootAction == null) { Debug.LogError("[PlayerShooting] shootAction is null. Can't do actions"); }
            if (reloadAction == null) { Debug.LogError("[PlayerShooting] reloadAction is null. Can't do actions"); }
        }

        // -- Doing Cool Things -- //
        playerMask = ~LayerMask.GetMask("Ignore Raycast"); // player masked used to be ignored by raycast.

        CurrentAmmo = maxAmmo;
        SpareAmmo = 130;


    }
    void Update()
    {
        if(TheDirector.Instance.CurrentState == TheDirector.GameState.Wave || TheDirector.Instance.CurrentState == TheDirector.GameState.Shop)
        {
            // -- Can We Shoot? -- //
            if (shootAction.triggered && !areWeReloading && canWeShoot)
            {
                if (!isMagazineEmpty)
                {
                    Shoot();
                }
                else
                {
                    // play dud sound
                }
            }
            // -- Can We Reload? -- //
            if (reloadAction.triggered && CurrentAmmo < maxAmmo && SpareAmmo > 0 && !areWeReloading)
            {
                Reload();
            }
        }
    }
    private void OnEnable()
    {
        shootAction?.Enable();
        reloadAction?.Enable();
    }
    private void OnDisable()
    {
        shootAction?.Disable();
        reloadAction?.Disable();

        if (TheDirector.Instance == null)
        {
            Debug.LogError("[PlayerShooting] TheDirector is missing from OnDisable. What happened?");
        }
        else
        {
            TheDirector.Instance.OnGameStateChanged -= Restart;
        }
    }

    // -- Methods -- //
    private void Shoot()
    {
        // -- Shoot the bullet -- //
        RaycastHit hit;
        Debug.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * raycastDistance, Color.red, 1f); // debug only
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, raycastDistance, playerMask))
        {
            if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy)) // could also do tags. 
            {
                enemy.TakeDamage(10);
            }
        }

        // -- Clean up after shot -- //
        CurrentAmmo -= 1;
        if (CurrentAmmo == 0) isMagazineEmpty = true;
        canWeShoot = false;
        StartCoroutine(ResetCanWeShootBool());
        if (UIManager.Instance == null)
        {
            Debug.LogError("[PlayerShooting] UIManager is null when trying to shoot.");
        }
        else
        {
            UIManager.Instance.ChangeAmmoSituation(CurrentAmmo, maxAmmo);
        }
    }
    private void Reload()
    {
        if (UIManager.Instance == null)
        {
            Debug.LogError("[PlayerShooting] UIManager is null when trying to shoot.");
        }
        else
        {
            UIManager.Instance.ReloadIcon(reloadSpeed);
        }
        areWeReloading = true;
        StartCoroutine(Reloading()); // this contains all the logic because we want things to change at the END.
    }
    private void Restart(TheDirector.GameState gameState)
    {
        if(gameState == TheDirector.GameState.Restart)
        {
            CurrentAmmo = maxAmmo;
            SpareAmmo = 130;
            if (UIManager.Instance == null)
            {
                Debug.LogError("[PlayerShooting] UIManager is null when trying to restart the ammo.");
            }
            else
            {
                UIManager.Instance.ChangeAmmoSituation(CurrentAmmo, maxAmmo);
            }
            SyncCoordinator.Instance.RestartReady();
        }
    }

    // -- Coroutines -- //
    IEnumerator ResetCanWeShootBool()
    {
        yield return new WaitForSeconds(shootSpeed);
        canWeShoot = true;
    }
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadSpeed);

        // -- Reload Logic -- //
        //If we have more than enough or exactly enough.
        if (SpareAmmo >= maxAmmo - CurrentAmmo)
        {
            SpareAmmo -= maxAmmo - CurrentAmmo;
            CurrentAmmo = maxAmmo;
        }
        //if we don't have enough to fill
        else
        {
            CurrentAmmo += SpareAmmo;
            SpareAmmo = 0;
        }

        // -- Deal with bools and shit -- //
        areWeReloading = false;
        isMagazineEmpty = false;
        if (UIManager.Instance == null)
        {
            Debug.LogError("[PlayerShooting] UIManager is null for some reason.");
        }
        else
        {
            UIManager.Instance.ChangeAmmoSituation(CurrentAmmo, maxAmmo); // don't forget this. Someone might be sad if you do.
        }
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // -- Healthbar -- //
    [SerializeField] TextMeshProUGUI u_wallHealthSituation;
    [SerializeField] Image greemHealthBar;
    private readonly string HealthbarFormat = "{0} / {1}"; // Apparently you can save the format and put it in SetText. Weird optimization

    // -- Gun Related Things -- //
    [SerializeField] TextMeshProUGUI u_ammoSituation;
    [SerializeField] Image reloadIcon;

    // -- Menus -- //
    [SerializeField] GameObject pauseMenuObject;
    [SerializeField] private UIFadeScript fader;
    
    public static UIManager Instance;

    // -- Specialty Methods -- //
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Debug.LogError("Multiple UIManagers"); Destroy(gameObject); }   
    }
    public void Start()
    {
        if (TheDirector.Instance == null) { Debug.LogError("[UIManager] TheDirector Instance not available"); }
        else { TheDirector.Instance.OnGameStateChanged += Restart; }
    }
    private void OnDisable()
    {
        if (TheDirector.Instance == null) { Debug.Log("[UIManager] The Director is null. Can't unsub"); }
        else { TheDirector.Instance.OnGameStateChanged -= Restart; }
    }

    // -- Healthbar -- //
    public void ChangeWallHealth(int health, int maxHealth)
    {
        float newHealth = (float)health / maxHealth;
        if (newHealth < 0 || newHealth > 1)
        {
            Debug.LogError($"[UIManager] {health} / {maxHealth} is not between 0 and 1. NOT okay.");
        }
        else
        {
            greemHealthBar.fillAmount = newHealth;
            u_wallHealthSituation.SetText(HealthbarFormat, health, maxHealth); //settext is faster than .text when there's stuff going on.}
        }
    }

    // -- Gun Related Methods -- //
    public void ChangeAmmoSituation(int currentAmmo, int maxAmmo)
    {
        u_ammoSituation.SetText(HealthbarFormat, currentAmmo, maxAmmo); // 'x / x' is the format
    }
    public void ReloadIcon(float reloadTime)
    {
        StartCoroutine(ReloadIconCoroutine(reloadTime));
    }
    private IEnumerator ReloadIconCoroutine(float reloadTime)
    {
        float elapsed = 0f;
        reloadIcon.fillAmount = 0f; // this can probably be removed??.
        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            reloadIcon.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
            yield return null;
        }
        reloadIcon.fillAmount = 0f; // this one is important because when we are "full" we want to get rid of the icon"
    }
    private IEnumerator RestartComplete(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        SyncCoordinator.Instance.RestartReady();
    }

    // -- Menu Methods -- //
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    private void Restart(TheDirector.GameState state)
    {
        if (state == TheDirector.GameState.Restart)
        {
            // Is this what I actually want?  Maybe countdown? Or screen to black?
            if(pauseMenuObject.activeSelf)
            {
                TogglePauseMenu();
                fader.FadeToBlack(1.3f);
            }
            StartCoroutine(RestartComplete(5.3f));
            // rest everything relating to the UI menu.
        }
    }
    public void TogglePauseMenu()
    {
        pauseMenuObject.SetActive(!pauseMenuObject.activeSelf);
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }
}

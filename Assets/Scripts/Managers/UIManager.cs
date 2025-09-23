using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI u_ammoSituation;
    [SerializeField] TextMeshProUGUI u_wallHealthSituation;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] Image reloadIcon;
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void ChangeAmmoSituation(int currentAmmo, int maxAmmo)
    {
        u_ammoSituation.text = $"{currentAmmo} / {maxAmmo}";
    }
    public void TogglePauseMenu()
    {
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
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
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    public void ReloadIcon(float reloadTime)
    {
        StartCoroutine(ReloadIconCoroutine(reloadTime));
    }
    public void ChangeWallHealth(int health, int maxHealth)
    {
        u_wallHealthSituation.text = $"{health} / {maxHealth}";
    }
    private IEnumerator ReloadIconCoroutine (float reloadTime)
    {
        float elapsed = 0f;
        reloadIcon.fillAmount = 0f;
        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            reloadIcon.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
            yield return null;
        }
        reloadIcon.fillAmount = 0f; 
    }
}

using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI u_AmmoSituation;
    [SerializeField] GameObject pauseCanvas;
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void ChangeAmmoSituation(int currentAmmo, int maxAmmo)
    {
        u_AmmoSituation.text = $"{currentAmmo} / {maxAmmo}";
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
}

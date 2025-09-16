using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI u_AmmoSituation;
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void ChangeAmmoSituation(int currentAmmo, int maxAmmo)
    {
        u_AmmoSituation.text = $"{currentAmmo} / {maxAmmo}";
    }
}

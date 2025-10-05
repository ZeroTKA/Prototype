using UnityEngine;

public class SyncCoordinator : MonoBehaviour
{
    [SerializeField] private int totalManagersToRestart;
    private int pendingManagersToRestart;

    [SerializeField] private UIFadeScript fader;

    public static SyncCoordinator Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); Debug.LogWarning("[SyncCoordinator] already exists. Destroying this object"); }
    }
    public void Start()
    {
        pendingManagersToRestart = totalManagersToRestart;    
    }
    public void RestartReady()
    {
        pendingManagersToRestart--;
        if (pendingManagersToRestart == 0)
        {
            if (TheDirector.Instance == null)
            {
                Debug.LogWarning("[SyncCoordinator] The Director is null. What hapened??. Gamestate not changed");
            }
            else { pendingManagersToRestart = totalManagersToRestart; fader.FadeBlackToClear(); }
        }
        else if (pendingManagersToRestart < 0)
        {
            Debug.LogWarning("[SyncCoordinator] something really bad happened to get pendingMangersToRestart to a negative number. Who called multiple times??");
        }
    }
}



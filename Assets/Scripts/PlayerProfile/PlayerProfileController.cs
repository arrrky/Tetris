using UnityEngine;

public class PlayerProfileController : MonoBehaviour
{
    public static PlayerProfileController Instance { get; set; }
    public PlayerProfile playerProfile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (playerProfile == null)
        {
            playerProfile = new PlayerProfile();
        }
    }
}

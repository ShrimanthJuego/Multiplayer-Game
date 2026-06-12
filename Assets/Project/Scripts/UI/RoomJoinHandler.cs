using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class RoomJoinHandler : MonoBehaviour
{
   [SerializeField] private FusionLauncher _fusionLauncher;
   [SerializeField] private Button startHostButton, startClientButton;

    private void Start()
    {
        startHostButton.onClick.AddListener(JoinHostRoom);
        startClientButton.onClick.AddListener(JoinClientRoom);
    }

    private void OnDestroy()
    {
        startHostButton.onClick.RemoveListener(JoinHostRoom);
        startClientButton.onClick.RemoveListener(JoinClientRoom);
    }
    
    public async void JoinClientRoom()
    {
        await _fusionLauncher.StartClientMode();
        gameObject.SetActive(false);
    }

    public async void JoinHostRoom()
    {
        await _fusionLauncher.StartHostMode();
        gameObject.SetActive(false);
    }
}

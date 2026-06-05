using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionLauncher : MonoBehaviour
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    private NetworkRunner _runner;

    private async void Start()
    {
       await StartSharedSession();
    }

    private async System.Threading.Tasks.Task StartSharedSession()
    {
        if(_runner != null)
            return;

         _runner = gameObject.AddComponent<NetworkRunner>();
         _runner.ProvideInput = true;
         var sceneReference = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
         var result = await _runner.StartGame(new StartGameArgs()
         {
             GameMode = GameMode.Shared,
             SessionName = "Test",
            //  Scene = sceneReference,
             SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
         });

           if(result.Ok)
        {
            Debug.Log("Session started successfully.");
            SpawnPlayer();
        }
        else
        {
            Debug.LogError($"Failed to start session: {result.ShutdownReason}");
        }
    }

    private void SpawnPlayer()
    {
         if (!playerPrefab.IsValid)
    {
        Debug.LogError("Player prefab is not assigned or not valid.");
        return;
    }
        Vector3 position = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));
        NetworkObject networkObject = _runner.Spawn(playerPrefab, position, Quaternion.identity, _runner.LocalPlayer);
        if (networkObject != null)
        {
            Debug.Log($"Player spawned successfully. {networkObject.name}");
        }
        else
        {
            Debug.LogError("Failed to spawn player.");
        }
    }
}
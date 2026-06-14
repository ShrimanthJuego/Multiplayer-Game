using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FusionLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private NetworkObject chatSystemPrefab;
    private NetworkRunner _runner;

    public async System.Threading.Tasks.Task StartHostMode()
    {
        await StartSession(GameMode.Host);
    }

    public async System.Threading.Tasks.Task StartClientMode()
    {
        await StartSession(GameMode.Client); // Client mode allows multiple instances to run on the same machine for testing
    }

    private async System.Threading.Tasks.Task StartSession(GameMode gameMode)
    {
        if (_runner != null)
            return;

        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;
        _runner.AddCallbacks(this);
        var sceneReference = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var result = await _runner.StartGame(new StartGameArgs()
        {
            GameMode = gameMode,
            SessionName = "TestRoom",
             Scene = sceneReference,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (result.Ok)
        {
            Debug.Log("Session started successfully.");
        }
        else
        {
            Debug.LogError($"Failed to start session: {result.ShutdownReason}");
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 position = new Vector3(
                UnityEngine.Random.Range(-3f, 3f),
                1f,
                UnityEngine.Random.Range(-3f, 3f)
            );

            NetworkObject playerObject = runner.Spawn(
                playerPrefab,
                position,
                Quaternion.identity,
                player  // give InputAuthority to this player
            );
            // chatSystemPrefab.AssignInputAuthority(player);
            Debug.Log($"Spawned player for: {player}");
            
            // runner.Spawn(chatSystemPrefab, Vector3.zero, Quaternion.identity, player);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"Player left: {player}");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"Shutdown: {shutdownReason}");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("OnInput called");
        var inputData = new NetworkInputData
        {
            Direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")),
            isTakenDamage = Input.GetKey(KeyCode.Space)
        };
        input.Set(inputData);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("throw new System.NotImplementedException");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Scene loaded. Spawning players.");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("throw new System.NotImplementedException");
    }
}
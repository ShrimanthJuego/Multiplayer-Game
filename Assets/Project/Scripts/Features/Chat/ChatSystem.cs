using Fusion;
using UnityEngine;

public class ChatSystem : NetworkBehaviour
{
   private string _lastChatMessage = "";
private float _chatTimer = 0f;
public string _chatInput = "";

private void Update()
{

    if (Input.GetKeyDown(KeyCode.Return) && _chatInput.Length > 0)
    {
        // Debug.Log("Has Input Authority: " + HasInputAuthority);
        RPC_SendChatMessage(_chatInput);
        _chatInput = "";
    }
}


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SendChatMessage(string message)
    {
        Debug.Log($"Received chat message: {message}");
        RPC_ReceiveMessage(message);
    }

 [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ReceiveMessage(string message)
    {
        _lastChatMessage = message;
        Debug.Log($"Received chat message: {message}");
        _chatTimer = 5f; // Display message for 5 seconds
    }

    private void OnGUI()
{
    // existing health bar and name code stays here
    // ...

    // Chat input for local player only
  
        GUI.Label(new Rect(10, Screen.height - 60, 200, 20), "Press Enter to send:");
        _chatInput = GUI.TextField(new Rect(10, Screen.height - 40, 200, 20), _chatInput);
    

    // Chat message visible to everyone
    if (_chatTimer > 0f)
    {
        _chatTimer -= Time.deltaTime;
        GUI.color = Color.white;
        GUI.Label(new Rect(10, Screen.height - 80, 400, 20), _lastChatMessage);
    }
}
}

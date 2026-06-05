using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;        
        }
    }
}

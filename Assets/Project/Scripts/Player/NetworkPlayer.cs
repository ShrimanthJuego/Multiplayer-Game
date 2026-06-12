using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Renderer _renderer;
    
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            _renderer.material.color = Color.green;
        }
        else
        {
            _renderer.material.color = Color.red;        
        }
    }

    public override void FixedUpdateNetwork()
    {
        if ( GetInput(out NetworkInputData inputData))
        {
            Vector3 direction = inputData.Direction;
            transform.position += direction * _moveSpeed * Runner.DeltaTime;
        }
    }

}   
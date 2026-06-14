using Fusion;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int maxHealth = 100;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public int Health { get; set; }

    [Networked, OnChangedRender(nameof(OnDead))]
    public bool IsDead { get; set; }

    [Networked, OnChangedRender(nameof(OnPlayerNameChanged))]
    public NetworkString<_32> PlayerName { get; set; }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            Health = maxHealth;
            IsDead = false;
            PlayerName = $"Player {Object.InputAuthority.PlayerId}";
        }

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
        if (IsDead)
            return;

        if (GetInput(out NetworkInputData inputData))
        {
            Vector3 direction = inputData.Direction;
            transform.position += direction * _moveSpeed * Runner.DeltaTime;
            if (inputData.isTakenDamage)
            {
                TakeDamage(10);
            }
        }
    }

    public override void Render()
    {
        if (!HasInputAuthority)
            return;

        if (Camera.main == null)
            return;

        // Smooth camera follow using interpolated position
        // Vector3 targetPosition = transform.position + new Vector3(0, 10f, -7f);
        // Camera.main.transform.position = Vector3.Lerp(
        //     Camera.main.transform.position,
        //     targetPosition,
        //     Time.deltaTime * 10f
        // );

        // Camera.main.transform.LookAt(transform.position);
    }

    private void OnHealthChanged()
    {
        Debug.Log($"[{gameObject.name}] Health changed to: {Health}");
    }

    private void OnPlayerNameChanged()
    {
        Debug.Log($"[{gameObject.name}] Name changed to: {PlayerName}");
    }


    public void TakeDamage(int amount)
    {
        if (!HasStateAuthority)
            return;

        if (IsDead)
            return;

        Health -= amount;

        if (Health <= 0)
        {
            Health = 0;
            IsDead = true;
        }
    }

    public void OnDead()
    {

        if (IsDead)
        {
            // Visual death feedback

            if (_renderer != null)
            {
                _renderer.material.color = Color.black;
            }
        }
    }

    // Draw simple health bar using OnGUI
    private void OnGUI()
    {
        if (Camera.main == null)
            return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);

        if (screenPos.z < 0)
            return;

        float barWidth = 100f;
        float barHeight = 10f;
        float x = screenPos.x - barWidth / 2f;
        float y = Screen.height - screenPos.y - barHeight / 2f;

        // Background bar (red)
        GUI.color = Color.red;
        GUI.DrawTexture(new Rect(x, y, barWidth, barHeight), Texture2D.whiteTexture);

        // Health bar (green)
        float healthPercent = (float)Health / maxHealth;
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(x, y, barWidth * healthPercent, barHeight), Texture2D.whiteTexture);

        GUI.color = Color.white;

        //show player name above the health
        GUI.Label(new Rect(x, y - 20, barWidth, 20), PlayerName.ToString());
    }
}
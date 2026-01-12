using Unity.VisualScripting;
using UnityEngine;

public sealed class PlayerResetter : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;

    [Header("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FrogState frogState;

    private Vector3 initialPosition;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (frogState == null) frogState = GetComponent<FrogState>();

        // spawnPoint�� ������ ���� ��ġ�� �������� ����
    }

    public void ResetPlayer()
    {
        // ��ġ �ʱ�ȭ
        transform.position = spawnPoint.position;

        // ���� �ʱ�ȭ
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }

        // ���� �ʱ�ȭ
        if (frogState != null)
        {
            frogState.SetDead(false);
            frogState.SetJumping(false);
        }
    }

    public void SetPhysicsActive(bool active)
    {
        if (rb == null) return;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = active;
    }
}

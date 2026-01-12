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

        // spawnPoint가 없으면 시작 위치를 기준으로 저장
        initialPosition = spawnPoint != null
            ? spawnPoint.position
            : transform.position;
    }

    public void ResetPlayer()
    {
        // 위치 초기화
        transform.position = initialPosition;

        // 물리 초기화
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }

        // 상태 초기화
        if (frogState != null)
        {
            frogState.SetDead(false);
            frogState.SetJumping(false);
            //frogState.SetOnWall(false);
        }
    }
}

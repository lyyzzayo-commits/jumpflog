using UnityEngine;

public sealed class WallStickHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D targetBody;
    [SerializeField] private float stickLinearDrag = 6f;
    [SerializeField] private float maxFallSpeed = -3f;
    [SerializeField] private float maxStickDuration = 1.2f;
    [SerializeField] private bool clampFallSpeed = true;

    private float defaultDrag;
    private float stickTimer;
    private int wallContactCount;

    public bool IsWallSticking { get; private set; }

    private void Awake()
    {
        if (targetBody == null)
            targetBody = GetComponentInParent<Rigidbody2D>();

        if (targetBody != null)
            defaultDrag = targetBody.linearDamping;
    }

    private void FixedUpdate()
    {
        if (!IsWallSticking || targetBody == null)
            return;

        stickTimer += Time.fixedDeltaTime;
        if (stickTimer >= maxStickDuration)
        {
            ClearWallStick();
            return;
        }

        if (clampFallSpeed && targetBody.linearVelocity.y < maxFallSpeed)
        {
            Vector2 velocity = targetBody.linearVelocity;
            velocity.y = maxFallSpeed;
            targetBody.linearVelocity = velocity;
        }
    }

    public void NotifyWall(Collider2D other)
    {
        if (targetBody == null)
            return;

        wallContactCount++;
        if (IsWallSticking)
            return;
        Debug.Log("벽감지");
        StartWallStick();
    }

    public void NotifyWallExit(Collider2D other)
    {
        if (wallContactCount > 0)
            wallContactCount--;

        if (wallContactCount <= 0)
            ClearWallStick();
    }

    public void CancelWallStick()
    {
        ClearWallStick();
    }

    private void StartWallStick()
    {
        IsWallSticking = true;
        stickTimer = 0f;
        if (targetBody != null)
            targetBody.linearDamping = stickLinearDrag;
    }

    public void ClearWallStick()
    {
        if (!IsWallSticking)
            return;

        IsWallSticking = false;
        stickTimer = 0f;
        wallContactCount = 0;

        if (targetBody != null)
            targetBody.linearDamping = defaultDrag;
    }
}

using UnityEngine;

public enum Facing
{
    Left,
    Right
}

public sealed class FrogState : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private SpriteRenderer sr;

    [Header("Idle Sprites")]
    [SerializeField] private Sprite idleLeft;
    [SerializeField] private Sprite idleRight;

    [Header("Jump Sprites")]
    [SerializeField] private Sprite jumpLeft;
    [SerializeField] private Sprite jumpRight;

    [Header("Wall Sprites")]
    [SerializeField] private Sprite wallLeft;
    [SerializeField] private Sprite wallRight;

    [Header("Dead Sprites")]
    [SerializeField] private Sprite deadLeft;
    [SerializeField] private Sprite deadRight;

    [Header("State")]
    [SerializeField] private Facing facing = Facing.Right;

    private bool isJumping;
    private bool isOnWall;
    private bool isDead;

    public Facing CurrentFacing => facing;
    public bool IsDead => isDead;

    // 점프 상태 (공중)
    public void SetJumping(bool value, Facing? jumpFacing = null)
    {
        if (isDead) return;

        isJumping = value;

        // 점프 시작/중에 방향을 갱신하고 싶으면 전달
        if (jumpFacing.HasValue)
            facing = jumpFacing.Value;

        UpdateSprite();
    }

    // 벽 접촉 상태 (+ 어느 벽인지)
    public void SetOnWall(bool value, Facing wallSide)
    {
        if (isDead) return;

        isOnWall = value;

        if (isOnWall)
        {
            // 벽에 붙는 순간 점프 상태 해제
            isJumping = false;

            // 벽에 붙으면 방향은 그 벽 방향으로 고정
            facing = wallSide;
        }

        UpdateSprite();
    }

    // 사망 상태 (최상위)
    public void SetDead(bool value)
    {
        if (isDead == value) return;

        isDead = value;

        if (isDead)
        {
            isJumping = false;
            isOnWall = false;
        }

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (sr == null) return;

        // 1) Dead
        if (isDead)
        {
            sr.sprite = (facing == Facing.Left) ? deadLeft : deadRight;
            return;
        }

        // 2) Wall
        if (isOnWall)
        {
            sr.sprite = (facing == Facing.Left) ? wallLeft : wallRight;
            return;
        }

        // 3) Jump
        if (isJumping)
        {
            sr.sprite = (facing == Facing.Left) ? jumpLeft : jumpRight;
            return;
        }

        // 4) Idle
        sr.sprite = (facing == Facing.Left) ? idleLeft : idleRight;
    }
}

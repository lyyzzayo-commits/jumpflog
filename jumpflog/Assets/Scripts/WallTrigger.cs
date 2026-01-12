using UnityEngine;

public sealed class WallTrigger : MonoBehaviour
{
    [Header("This wall side")]
    [SerializeField] private Facing wallSide = Facing.Left; // ?�스?�터?�서 Left/Right 지??

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Frog")) return;

        // 개구�?�?컴포?�트??가?�오�?
        var stick = other.GetComponentInParent<WallStickHandler>();
        var state = GetComponentInParent<FrogState>();

        if (stick == null) return;

        // 기존 벽붙�?로직
        if (stick != null)
            stick.NotifyWall(other);
        else
            stick.NotifyWall(other); // (??구조??맞게 ??)

        // ?�프?�이???�태 갱신
        if (state != null && !state.IsDead)
            state.SetOnWall(true, wallSide);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Frog")) return;

        var stick = other.GetComponentInParent<WallStickHandler>();
        var state = GetComponentInParent<FrogState>();

        if (stick == null) return;

        if (stick != null)
            stick.NotifyWallExit(other);
        else
            stick.NotifyWallExit(other); // (??구조??맞게 ??)

        if (state != null && !state.IsDead)
            state.SetOnWall(false, state.CurrentFacing);
    }
}


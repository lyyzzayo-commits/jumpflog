using NUnit.Framework;
using UnityEngine;

public sealed class WallTrigger : MonoBehaviour
{
    [SerializeField] private WallStickHandler wallstick;
    [SerializeField] private FrogState frogState;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Frog")) return;
        var stick = other.GetComponent<WallStickHandler>();
        if (stick == null) return;

        wallstick.NotifyWall(other);
        frogState.SetOnWall(true);         
        //FrogRotator에서 Rotate함수 호출
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Frog")) return;
        var stick = other.GetComponent<WallStickHandler>();
        if (stick == null) return;

        wallstick.NotifyWallExit(other);
        frogState.SetOnWall(false);
        frogState.SetJumping(true);
    }
}

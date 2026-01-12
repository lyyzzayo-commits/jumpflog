using UnityEngine;

public sealed class FrogState : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite wallSprite;

    private bool isJumping;
    private bool isOnWall;

    public void SetJumping(bool value)
    {
        isJumping = value;
        UpdateSprite();
    }

    public void SetOnWall(bool value)
    {
        isOnWall = value;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (isOnWall) sr.sprite = wallSprite;
        else if (isJumping) sr.sprite = jumpSprite;
        else sr.sprite = idleSprite;
    }
}

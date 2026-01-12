// using UnityEngine;

// public sealed class FrogState : MonoBehaviour
// {
//     // [Header("Renderer")]
//     // [SerializeField] private SpriteRenderer sr;

//     // [Header("Sprites")]
//     // [SerializeField] private Sprite idleSprite;
//     // [SerializeField] private Sprite jumpSprite;
//     // [SerializeField] private Sprite wallSprite;
//     // [SerializeField] private Sprite deadSprite;

//     // private bool isJumping;
//     // private bool isOnWall;
//     // private bool isDead;

//     // 점프 상태 (공중)
//     // public void SetJumping(bool value)
//     // {
//     //     if (isDead) return;

//     //     isJumping = value;
//     //     UpdateSprite();
//     // }

//     // 벽 접촉 상태
//     // public void SetOnWall(bool value)
//     // {
//     //     if (isDead) return;

//     //     isOnWall = value;

//     //     // 벽에 붙는 순간 점프 상태 해제
//     //     if (isOnWall)
//     //         isJumping = false;

//     //     UpdateSprite();
//     // }

//     // 사망 상태 (최상위)
//     // public void SetDead(bool value)
//     // {
//     //     if (isDead == value) return;

//     //     isDead = value;

//     //     if (isDead)
//     //     {
//     //         // 다른 상태 정리
//     //         isJumping = false;
//     //         isOnWall = false;
//     //     }

//     //     UpdateSprite();
//     // }

//     // private void UpdateSprite()
//     // {
//     //     // 1순위: Dead
//     //     if (isDead)
//     //     {
//     //         sr.sprite = deadSprite;
//     //         return;
//     //     }

//     //     // 2순위: 벽
//     //     if (isOnWall)
//     //     {
//     //         sr.sprite = wallSprite;
//     //     }
//     //     // 3순위: 점프
//     //     else if (isJumping)
//     //     {
//     //         sr.sprite = jumpSprite;
//     //     }
//     //     // 4순위: 기본
//     //     else
//     //     {
//     //         sr.sprite = idleSprite;
//     //     }
//     // }
// }

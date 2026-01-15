using System;
using UnityEngine;
using UnityEngine.InputSystem; // 신형 인풋 시스템 추가

public class DragController : MonoBehaviour
{
    public LineRenderer line;
    public Rigidbody2D rb;

    public float dragLimit = 3f;
    public float forceToAdd = 10f;
    [SerializeField] private TrajectoryLine trajectoryLine;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip launchClip;

    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private Collider2D ballCol;
    [SerializeField] private WallStickHandler wallstickhandler;
    [SerializeField] private FrogState frogState;
    

    private Camera cam;
    private bool isDragging;
    private bool inputEnabled = true;
    private Vector3 dragStartPos;
    private Vector3 lastDragVector;
    
    public void EnableInput(bool enable)
    {
        inputEnabled = enable;

        if (!enable)
        {
            isDragging = false;
            if (line != null) line.enabled = false;
            if (trajectoryLine != null) trajectoryLine.Hide();
        }
    }


    Vector3 PointerWorldPosition
    {
        get
        {
            // Pointer.current가 null일 수 있으니 안전 처리
            if (Pointer.current == null) return (Vector3)rb.position;

            Vector2 screenPos = Pointer.current.position.ReadValue(); // 화면 좌표
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            worldPos.z = 0f;
            return worldPos;
        }
    }
    private void Start()
    {
        cam = Camera.main;
        if (line != null)
        {
            line.positionCount = 2;
            line.enabled = false;
        }

        if (rb == null) 
            rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!inputEnabled) return;
        if (Pointer.current == null) return;

        var press = Pointer.current.press;

        if (press.wasPressedThisFrame && !isDragging)
            DragStart();

        if (!isDragging) return;

        Drag();

        if (press.wasReleasedThisFrame)
            DragEnd();
    }

    private void DragStart()
    {
        if (rb == null) return;

        isDragging = true;
        dragStartPos = (Vector3)rb.position;
        lastDragVector = Vector3.zero;

        if (line != null)
        {
            line.enabled = true;
            if (line.positionCount < 2) line.positionCount = 2;
            line.SetPosition(0, dragStartPos);
            line.SetPosition(1, dragStartPos);
        }
    }

    private void Drag()
    {
            Vector3 startPos = rb != null ? (Vector3)rb.position : dragStartPos;

            if (line != null)
            {
                if (line.positionCount < 2) line.positionCount = 2;
                line.SetPosition(0, startPos);
            }

            Vector3 currentPos = PointerWorldPosition;
            Vector3 distance = currentPos - startPos;

            Vector3 endPos;
            if (distance.magnitude <= dragLimit)
                endPos = currentPos;
            else
                endPos = startPos + (distance.normalized * dragLimit);

            if (line != null)
            {
                if (line.positionCount < 2) line.positionCount = 2;
                line.SetPosition(1, endPos);
            }

            lastDragVector = endPos - startPos;
            if (trajectoryLine != null)
            {
                Vector2 v0 = -(Vector2)lastDragVector * forceToAdd / rb.mass;
                trajectoryLine.Show(startPos, v0);
            }
        }

    private void DragEnd()
    {
            isDragging = false;
            if (line != null) line.enabled = false;
            if (trajectoryLine != null) trajectoryLine.Hide();

            if (rb == null) return;

            // 발사 벡터 계산
            Vector3 dragVector = lastDragVector;

            // 발사 전 물리 리셋(이전 속도 누적 방지)
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // 던지는 방향: 당긴 반대 방향
            rb.AddForce(-(Vector2)dragVector * forceToAdd, ForceMode2D.Impulse);

            Vector2 launchVector = -(Vector2)dragVector;

            // 방향 결정 (이 순간에만!)
            Facing facing = (launchVector.x < 0f)
                ? Facing.Left
                : Facing.Right;

            // 상태 전달
            frogState.SetJumping(true, facing);


            //Frog체인지 호출함

            if (launchClip != null)
            {
                if (audioSource != null)
                    audioSource.PlayOneShot(launchClip);
                else
                    AudioSource.PlayClipAtPoint(launchClip, rb.position);
            }
        }
}


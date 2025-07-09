using UnityEngine;

public class MoveController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float minSpeed = 100f;
    [SerializeField] private float maxSpeed = 400f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 1f;

    [Header("Boundary Settings")]
    [SerializeField] private float minX = -400f;
    [SerializeField] private float maxX = 400f;
    [SerializeField] private float minY = -200f;
    [SerializeField] private float maxY = 200f;

    private RectTransform rectTransform;
    private Vector2 currentVelocity;
    private float currentSpeed = 0f;
    private Vector2 lastInputDirection = Vector2.zero;
    private bool isMoving = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        HandleMovementInput();
        ApplyDeceleration();
        ApplyMovement();
        ClampToBoundaries();
        Debug.Log(currentSpeed);
    }

    private void HandleMovementInput()
    {
        Vector2 inputDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) inputDirection.y -= 1;
        if (Input.GetKey(KeyCode.S)) inputDirection.y += 1;
        if (Input.GetKey(KeyCode.A)) inputDirection.x += 1;
        if (Input.GetKey(KeyCode.D)) inputDirection.x -= 1;



        if (inputDirection.magnitude > 0)
        {
            inputDirection.Normalize();
            isMoving = true;
            lastInputDirection = inputDirection; // 记住最后输入方向

            // 加速逻辑
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

            // 更新速度向量，保持当前移动方向
            currentVelocity = lastInputDirection * currentSpeed;
        }
        else
        {
            isMoving = false;
        }

    }

    private void ApplyDeceleration()
    {
        if (currentSpeed > 0 && !isMoving)
        {
            // 减速逻辑 - 保持最后移动方向但降低速度
            currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0);
            currentVelocity = lastInputDirection * currentSpeed;
        }
    }

    private void ApplyMovement()
    {
        if (currentSpeed > 0)
        {
            rectTransform.anchoredPosition += currentVelocity * Time.deltaTime;
        }
    }

    private void ClampToBoundaries()
    {
        Vector2 anchoredPos = rectTransform.anchoredPosition;
        Vector2 halfSize = rectTransform.rect.size * 0.5f;

        float actualMinX = minX + halfSize.x;
        float actualMaxX = maxX - halfSize.x;
        float actualMinY = minY + halfSize.y;
        float actualMaxY = maxY - halfSize.y;

        anchoredPos.x = Mathf.Clamp(anchoredPos.x, actualMinX, actualMaxX);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, actualMinY, actualMaxY);

        rectTransform.anchoredPosition = anchoredPos;

        // 更新当前速度大小
        currentSpeed = currentVelocity.magnitude;
    }

#if UNITY_EDITOR
    // 编辑器辅助：绘制可移动区域
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Vector2 halfSize = rectTransform.rect.size * 0.5f;
            float actualMinX = minX + halfSize.x;
            float actualMaxX = maxX - halfSize.x;
            float actualMinY = minY + halfSize.y;
            float actualMaxY = maxY - halfSize.y;

            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(actualMinX, actualMinY, 0);
            corners[1] = new Vector3(actualMinX, actualMaxY, 0);
            corners[2] = new Vector3(actualMaxX, actualMaxY, 0);
            corners[3] = new Vector3(actualMaxX, actualMinY, 0);

            // 转换为世界坐标
            for (int i = 0; i < 4; i++)
            {
                corners[i] = rectTransform.parent.TransformPoint(corners[i]);
            }

            Gizmos.color = Color.green;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
            }
        }
    }
#endif
}
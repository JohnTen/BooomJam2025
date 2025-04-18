using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathIdle : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 0.01f; // 移动速度
    [SerializeField] private float directionChangeInterval = 1.5f; // 方向改变的时间间隔
    [SerializeField] private float directionLerpSpeed = 0.5f; // 方向变化的平滑速度
    [SerializeField] private float maxDistanceFromStart = 2f; // 最大距离限制

    private Vector2 currentDirection; // 当前移动方向
    private Vector2 targetDirection; // 目标移动方向
    private float directionChangeTimer; // 方向改变计时器
    private Vector3 initialPosition; // 初始位置

    // Start is called before the first frame update
    void Start()
    {
        // 初始化方向和初始位置
        currentDirection = Random.insideUnitCircle.normalized;
        targetDirection = Random.insideUnitCircle.normalized;
        directionChangeTimer = directionChangeInterval;
        initialPosition = transform.position; // 记录初始位置
    }

    // Update is called once per frame
    void Update()
    {

        // 更新方向
        UpdateDirection();

        // 移动物体
        MoveObject();
    }

    private void UpdateDirection()
    {
        // 每隔一定时间生成一个新的随机方向
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            targetDirection = Random.insideUnitCircle.normalized; // 生成新的随机方向
            directionChangeTimer = directionChangeInterval; // 重置计时器
        }

        // 平滑地插值当前方向到目标方向
        currentDirection = Vector2.Lerp(currentDirection, targetDirection, directionLerpSpeed).normalized;
    }

    private void MoveObject()
    {
        // 计算移动的增量
        Vector3 movement = new Vector3(currentDirection.x, currentDirection.y, 0) * moveSpeed * Time.deltaTime;

        // 更新物体的位置
        transform.position += movement;

        // 限制物体不能离开初始位置太远
        float distanceFromStart = Vector3.Distance(transform.position, initialPosition);
        if (distanceFromStart > maxDistanceFromStart)
        {
            // 将物体拉回到范围内
            Vector3 directionToCenter = (initialPosition - transform.position).normalized;
            transform.position = initialPosition + directionToCenter * maxDistanceFromStart;
        }
    }
}

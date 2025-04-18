using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithPointer : MonoBehaviour
{
    [SerializeField] Vector2 rotationRange;
    [SerializeField] Vector2 movementRange;
    [SerializeField] float rotationDamp;
    [SerializeField] float movementDamp;

    private Vector3 movement;
    private Vector3 currentMovement;
    private Vector3 rotation;
    private Vector3 currentRotation;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        currentRotation = transform.eulerAngles;
    }

    private void Update()
    {
        // 获取鼠标在屏幕上的位置
        Vector2 mousePosition = Input.mousePosition;
        
        // 将鼠标位置归一化到 -0.5 到 0.5 的范围
        Vector2 normalizedPosition = new Vector2(
            mousePosition.x / Screen.width - 0.5f,
            mousePosition.y / Screen.height - 0.5f
        );
        
        // 根据鼠标位置计算目标旋转角度
        rotation.y = -normalizedPosition.x * rotationRange.x;
        rotation.x = normalizedPosition.y * rotationRange.y;

        movement.x = normalizedPosition.x * movementRange.x;
        movement.y = normalizedPosition.y * movementRange.y;
        
        // 使用阻尼平滑过渡到目标旋转
        currentRotation = Vector3.Lerp(currentRotation, rotation, Time.deltaTime * rotationDamp);
        currentMovement = Vector3.Lerp(currentMovement, movement, Time.deltaTime * movementDamp);
        
        // 应用旋转到物体
        transform.eulerAngles = currentRotation;
        transform.position = currentMovement;
    }
}

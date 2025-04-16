using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode] // 允许在编辑器模式下实时调整
[RequireComponent(typeof(Image))] // 确保挂载在 UI Image 上
public class UIShaderController : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float xOffset = 0f;

    [Range(-1f, 1f)]
    public float yOffset = 0f;

    private Material _material; // 存储动态创建的 Material 实例
    private Image _image; // 存储 Image 组件

    private void Awake()
    {
        _image = GetComponent<Image>();
        
        // 创建 Material 实例，避免修改原始材质
        if (_image.material != null)
        {
            _material = new Material(_image.material);
            _image.material = _material;
        }
    }

    private void Update()
    {
        if (_material != null)
        {
            _material.SetFloat("_x", xOffset);
            _material.SetFloat("_y", yOffset);
        }
    }

    // 在 Inspector 值变化时更新（适用于编辑器模式）
    private void OnValidate()
    {
        if (_material != null)
        {
            _material.SetFloat("_x", xOffset);
            _material.SetFloat("_y", yOffset);
        }
    }

    // 销毁时清理 Material 实例（可选）
    private void OnDestroy()
    {
        if (_material != null)
        {
            DestroyImmediate(_material);
        }
    }
}
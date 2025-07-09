# 优先级指针事件系统 (Prioritized Pointer Event System)

## 概述

这是一个基于优先级的指针事件处理系统，允许在拖拽时选择性触发不同类型的UI事件。系统支持提示信息、高亮效果、展开面板和放置区域等多种交互类型。

## 核心特性

- **优先级管理**：不同类型的UI元素有不同的优先级
- **拖拽感知**：拖拽时可以禁用某些事件（如提示信息）
- **遮挡处理**：支持忽略遮挡的UI元素
- **完整的事件数据**：使用真实的PointerEventData

## 系统架构

### 核心组件

1. **PrioritizedPointerManager** - 主管理器
2. **PointerEventDataAccessor** - 事件数据访问器
3. **BasePrioritizedPointerHandler** - 基础处理器
4. **具体处理器实现** - TooltipHandler、HighlightHandler等

### 优先级定义

```csharp
public enum PointerPriority
{
    Tooltip = 1,        // 提示信息 - 拖拽时禁用
    Highlight = 2,      // 高亮效果 - 拖拽时启用
    Expandable = 3,     // 展开界面 - 拖拽时启用
    DropZone = 4        // 放置区域 - 拖拽时启用
}
```

## 使用方法

### 1. 设置管理器

确保场景中有PrioritizedPointerManager：

```csharp
// 自动创建（如果不存在）
if (PrioritizedPointerManager.Instance == null)
{
    var managerGO = new GameObject("PrioritizedPointerManager");
    managerGO.AddComponent<PrioritizedPointerManager>();
}
```

### 2. 添加处理器

为UI元素添加相应的处理器组件：

```csharp
// 添加提示处理器
var tooltipHandler = gameObject.AddComponent<TooltipHandler>();

// 添加高亮处理器
var highlightHandler = gameObject.AddComponent<HighlightHandler>();

// 添加放置区域处理器
var dropZoneHandler = gameObject.AddComponent<DropZoneHandler>();

// 添加展开面板处理器
var expandablePanelHandler = gameObject.AddComponent<ExpandablePanelHandler>();
```

### 3. 配置参数

在Inspector中设置处理器的参数：

- **Priority** - 事件优先级
- **Enable During Drag** - 拖拽时是否启用
- **Ignore Obstacles** - 是否忽略遮挡

### 4. 自定义处理器

继承BasePrioritizedPointerHandler创建自定义处理器：

```csharp
public class CustomHandler : BasePrioritizedPointerHandler
{
    protected override void Awake()
    {
        base.Awake();
        priority = PointerPriority.Highlight;
        enableDuringDrag = true;
        ignoreObstacles = false;
    }
    
    protected override void OnPointerEnter(PointerEventData eventData)
    {
        // 自定义进入逻辑
    }
    
    protected override void OnPointerExit(PointerEventData eventData)
    {
        // 自定义离开逻辑
    }
}
```

## 事件类型

系统支持以下事件类型：

- **Enter** - 指针进入
- **Exit** - 指针离开
- **Click** - 点击事件
- **DragStart** - 拖拽开始
- **DragEnd** - 拖拽结束

## 拖拽行为

### 拖拽时的行为控制

- **Tooltip** - 拖拽时禁用，避免干扰
- **Highlight** - 拖拽时启用，保持视觉反馈
- **Expandable** - 拖拽时启用，允许展开操作
- **DropZone** - 拖拽时启用，支持放置操作

### 遮挡处理

- **Ignore Obstacles = false** - 被遮挡时不触发事件
- **Ignore Obstacles = true** - 即使被遮挡也触发事件

## 性能优化

- 使用对象池管理事件处理器
- 避免每帧重复的射线检测
- 智能的事件过滤机制

## 调试

使用PointerEventDataAccessor获取当前事件数据：

```csharp
var eventData = PointerEventDataAccessor.GetCurrentPointerEventData();
if (eventData != null)
{
    Debug.Log($"位置: {eventData.position}");
    Debug.Log($"悬停对象: {eventData.pointerCurrentRaycast.gameObject?.name}");
    Debug.Log($"拖拽状态: {eventData.dragging}");
}
```

## 注意事项

1. 确保场景中有EventSystem
2. 确保VirtualCursor正常工作
3. 处理器会自动注册到管理器，无需手动管理
4. 拖拽状态通过Draggable组件检测

## 扩展

系统设计为可扩展的，可以轻松添加新的处理器类型和事件类型。所有核心逻辑都封装在基础类中，便于维护和扩展。 
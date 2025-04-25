using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTUtility;

public enum DialogueEntryType
{
    Pass, // 各界面通用
    ClickAnywhere, // 各界面通用
    ClickMaskArea, // 各界面通用
}

public class DialogueEntry
{
    public class MaskSetting
    {
        public Vector2 pos;
        public Vector2 size;

        public MaskSetting(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
        }
    }

    // 对话类型
    public DialogueEntryType type;

    public string instructionID;

    // 对话内容
    public string text;

    // 对话者名称
    public string actorName;

    // 头像
    public string portriat;

    // 这条DialogueEntry已经播放过几次
    public int playedTimes;

    // 是否只播放一次
    public bool oneTimeOnly;

    // 是否使用打字效果
    public bool useTyping;
    // 打字速度
    public float typingSpeed;
    // 是否使用打乱模式
    public bool useScramble;
    // 下一个对话
    public string nextEntry;
    // 使用一个gadget而非标准对话框
    public string gadget;
    
    // 执行条件
    public Func<int, DialogueEntry, object[], bool> condition;

    // 过渡条件
    public Func<int, DialogueEntry, object[], bool> waitForCondition;

    // 执行开始
    public Action onExecuting;
    // 执行结束
    public Action onExecuted;
    // 一些变量
    public Dictionary<string, object> variables;
    // 遮罩
    public List<MaskSetting> masks;
    // 延迟
    public float delay;

    // 是否暂停游戏
    public bool pauseGame;
    // 是否透明遮罩
    public bool transparentMask;
    // 反向遮罩
    public List<RectTransform> unmasks;

    // 初始化
    public Action<DialogueEntry> onDialogueEntryExecInit;
    // 执行开始
    public Action<DialogueEntry> onDialogueEntryExecStart;
    // 执行结束
    public Action<DialogueEntry> onDialogueEntryExecEnd;

    // 是否初始化
    public bool inited;
    // 是否开始
    public bool started;
    // 是否结束
    public bool done;

    // 是否存在条件
    public bool HasCondition => waitForCondition != null;

    public DialogueEntry(string instructionID, DialogueEntryType type, string portriat, string actorName, string text)
    {
        this.instructionID = instructionID;
        this.type = type;
        this.text = text;
        this.portriat = portriat;
        this.actorName = actorName;
        this.useTyping = true;
        this.typingSpeed = 1f;
        this.useScramble = false;
    }

    // 获取特定核心槽的数值
    // GameManager.Instance.corePercent[CoreSlotType.MemoryCore]
    // 现存所有的核心
    // GameManager.Instance.eCores

    public static List<DialogueEntry> GenerateDialogueEntries()
    {
        var collections = new List<DialogueEntry>();
        DialogueEntry entry = null;

        entry = new DialogueEntry(
            "测试1", 
            DialogueEntryType.ClickAnywhere, 
            "test1", 
            "Astra", 
            "对话1对话1对话1");
        entry.condition = (eventID, inst, args) => {
            return true;
        };
        entry.oneTimeOnly = true;
        entry.delay = 0.5f;
        entry.nextEntry = "测试2";
        collections.Add(entry);

        entry = new DialogueEntry(
            "测试2", 
            DialogueEntryType.Pass, 
            "test2", 
            "Core", 
            "对话2对话2对话2");
        entry.nextEntry = "测试3";
        collections.Add(entry);

        entry = new DialogueEntry(
            "测试3", 
            DialogueEntryType.ClickAnywhere, 
            "test3", 
            "Astra", 
            "对话3对话3对话3");
        //entry.nextEntry = "测试1";
        entry.delay = 3f;
        entry.gadget = "testGadget";
        collections.Add(entry);

        return collections;
    }
}

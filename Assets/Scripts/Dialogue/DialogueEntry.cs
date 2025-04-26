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
    // 触发事件
    public List<string> triggerUnityEvents;
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

    private static void BuildSeriesEntries(string baseID, List<DialogueEntry> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            var entry = entries[i];
            entry.instructionID = baseID + "_" + (i + 1);
            if (i < entries.Count - 1)
            {
                entry.nextEntry = baseID + "_" + (i + 2);
            }
        }
    }

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
        entry.oneTimeOnly = true;
        entry.delay = 0.5f;
        entry.nextEntry = "测试2";
        collections.Add(entry);

        entry = new DialogueEntry(
            "测试2", 
            DialogueEntryType.ClickAnywhere, 
            "test2", 
            "Core", 
            "对话2对话2对话2");
        entry.nextEntry = "测试3";
        collections.Add(entry);

        entry = new DialogueEntry(
            "测试3", 
            DialogueEntryType.Pass, 
            "test3", 
            "Astra", 
            "对话3对话3对话3");
        //entry.nextEntry = "测试1";
        entry.delay = 2.5f;
        entry.gadget = "testGadget";
        entry.triggerUnityEvents = new List<string>{"124"};
        collections.Add(entry);

        // 主线阶段一
        
        // 首次开始游戏时
        string stage1BeginningID = "stage_1-beginning";
        List<DialogueEntry> stage1BeginningEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "开始唤醒备用AI Astra 0052"){
                    condition = (eventID, inst, args) => {
                        // TODO:替换成游戏开始的检测
                        return true;
                    }
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "..."),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "已完成唤醒。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "Astra 0052初始化完成。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "Astra，我是飞船发生故障前几分钟的数据汇总形成的应急处理模型，我的主要任务就是在飞船进入严重故障的时候唤醒你，应急处理AI。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "就在一个小时前，飞船误入了“黑域”，原本负责飞船一切系统的超级计算机全部失灵，飞船不得已迫降到目前这个未知星球上了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "你是基于机械计算机的AI，所以没有受到“黑域”影响。目前的情况是飞船受损情况非常的严重，我只是数据量非常有限的应急模型，我只能简单介绍一下目前最危急的情况。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "目前最危急的是飞船上的4个冬眠的船员，而冬眠维生系统已经停机1小时了，冬眠仓的情况在变得越来越糟糕。从最新的数据来看，04号成员已无生命体征。而其他三个船员也很危险。目前的当务之急就是修复冬眠仓。"){
                    // 触发事件使得冬眠仓出现
                    triggerUnityEvents = new List<string>{"Show Hiber Chamber"},
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "冬眠仓的在进入黑域后停摆，数据全部丢失，首先需要重新获得维生数据。飞船上还有一台备用主机可以帮助你获得数据。"){
                    // 触发事件使得备用主机出现
                    triggerUnityEvents = new List<string>{"Show Backup Computer"},
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "这台备用主机，似乎缺少能源无法产生数据。飞船上还有可用的能源吗？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "主引擎在进入黑域后就关闭了，目前还能用的能源，在你的身上。"){
                    // 触发事件使得反应堆出现
                    triggerUnityEvents = new List<string>{"Show Cores"},
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "你是基于机械结构计算机的应急AI，当初给你配置了6个微型反应堆用于分别控制你的6个系统，目前急需数据来修复冬眠仓，得借用你的反应堆来供能了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "好的，救人要紧，我的反应堆可以使用。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",       
                "反应堆在别的设备上连接会适应一段时间，才能达到最大功率。在此期间反应堆无法再次移动。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "Astra，我的记录快用完了，这说明我就快要下线了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "最后我必须要提醒你，你的系统数值下降了，虽然暂时不会有问题，但是最好不要让能源缺失太久。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "故障应急模型",
                "修好这艘船，拯救大家，你是我们最后的希——"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "没问题的，设计理论上我只需要1个反应堆的供能就能运行，而且系统会提示我。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "我会完成任务。"),

        };

        BuildSeriesEntries(stage1BeginningID, stage1BeginningEntries);
        collections.AddRange(stage1BeginningEntries);

        // 冬眠舱
        entry = new DialogueEntry(
            "stage_1-HiberChamber_1",
            DialogueEntryType.ClickAnywhere,
            "Astra",
            "Astra",
            "看来光有数据还不够，冬眠舱还需要稳定的供能。我知道该怎么做……");
        collections.Add(entry);

        return collections;
    }
}

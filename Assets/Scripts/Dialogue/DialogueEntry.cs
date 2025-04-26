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
    // 需要满足的Switch条件
    public List<StrIntPair> requireSwitch;
    // 需要设置的Switch条件
    public List<StrIntPair> setSwitch;
    
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
                    requireSwitch = new List<StrIntPair>{
                        new StrIntPair("stage1", 1),
                    },
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
                "你是基于机械计算机的AI，所以没有受到“黑域”影响。目前的情况是飞船受损情况非常的严重，我只是数据量非常有限的应急模型，我只能简单介绍一下目前最危急的情况。")
                {
                    // 触发事件使得操作界面出现
                    triggerUnityEvents = new List<string>{"Show Controls"},
                },
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
                "好的，救人要紧，我的反应堆可以使用。")
                {
                    // 处理拔出反应堆的表演
                },
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

        // 主线阶段二
        string stage2BeginningID = "stage_2-Beginning";
        List<DialogueEntry> stage2BeginningEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "冬眠舱修好了，看起来剩余的3名宇航员生命体征正常。"){
                    requireSwitch = new List<StrIntPair>{
                        new StrIntPair("stage2", 1),
                    },
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "我可以尝试唤醒一名宇航员，让他来帮助我进行维修飞船的任务。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "但是唤醒一名宇航员需要消耗大量的能源，会较长时间的占用我的反应堆。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "我需要慎重考虑，而且我还不太擅长和人类打交道。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "或许我应该优先将目标放在飞船维修上。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "接下来我需要将作业舱重新唤醒，还需要通过主机获得一些数据。"),
        };

        BuildSeriesEntries(stage2BeginningID, stage2BeginningEntries);
        collections.AddRange(stage2BeginningEntries);

        // 作业舱
        string stage2JobCabinID = "stage_2-JobCabin";
        List<DialogueEntry> stage2JobCabinEntries = new List<DialogueEntry>()
        {
            
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "你好，探索机器人 S01，你现在感觉如何？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "**&%@，我竟然还能重启？陷入黑域的感觉太可怕了，我可不想再体验一次。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "我是机械结构的计算机，其实不是很能理解你的感受。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "不过总之欢迎你回来，现在飞船还有很多部分待维修，需要你的帮助。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "我能做什么？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "飞船在降落时物理结构也受到了严重的损伤，我们目前需要一些维修材料。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "需要你帮忙去我们迫降的这个行星看看，能不能找到有用的东西。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "为了保证你在舱外的能源供应，你可以暂时带上我的微型反应堆。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "好的，探索的事情就交给我了。")
        };

        BuildSeriesEntries(stage2JobCabinID, stage2JobCabinEntries);
        collections.AddRange(stage2JobCabinEntries);

        // 探索1
        string stage2Explore1ID = "stage_2-Explore1-";
        List<DialogueEntry> stage2Explore1Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "探索完成，这次我找到了一个秘方石，可以当作临时的能量源。"){
                    // 触发事件使得秘方石出现
                    triggerUnityEvents = new List<string>{"Show Secret Stone"},
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "暂时还没有找到金属矿。"),
        };

        BuildSeriesEntries(stage2Explore1ID, stage2Explore1Entries);
        collections.AddRange(stage2Explore1Entries);

        // 探索2
        string stage2Explore2ID = "stage_2-Explore2-";
        List<DialogueEntry> stage2Explore2Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "探索完成，这次我找到了n个金属矿石，可以用于维修飞船。"),
        };

        BuildSeriesEntries(stage2Explore2ID, stage2Explore2Entries);
        collections.AddRange(stage2Explore2Entries);

        // 探索3
        string stage2Explore3ID = "stage_2-Explore3-";
        List<DialogueEntry> stage2Explore3Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "探索完成，我找到了金属矿脉，坐标已经上传，之后我们可以稳定的获取金属矿石了。"){
                    // 触发事件使得金属矿脉出现
                    triggerUnityEvents = new List<string>{"Unlock Metal Mine"},
                },
        };

        BuildSeriesEntries(stage2Explore3ID, stage2Explore3Entries);
        collections.AddRange(stage2Explore3Entries);

        // 金属矿石足够
        string stage2MetalEnoughID = "stage_2-MetalEnough";
        List<DialogueEntry> stage2MetalEnoughEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "矿石收集够了，可以修复加工炉了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "有了加工炉，船体的损坏就不用愁了。"),
        };

        BuildSeriesEntries(stage2MetalEnoughID, stage2MetalEnoughEntries);
        collections.AddRange(stage2MetalEnoughEntries);

        // 主控仓修复
        string stage2MainControlRoomFixID = "stage_2-MainControlRoomFix";
        List<DialogueEntry> stage2MainControlRoomFixEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "主控舱可以进行修复了，需要一个微型反应堆来保持能源稳定。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "用掉这个反应堆后我还剩3个，不知道后面会发生什么，但愿能顺利。"),
        };

        BuildSeriesEntries(stage2MainControlRoomFixID, stage2MainControlRoomFixEntries);
        collections.AddRange(stage2MainControlRoomFixEntries);

        // 解锁维修机器人F02
        string stage2UnlockWorkRobotID = "stage_2-UnlockWorkRobot";
        List<DialogueEntry> stage2UnlockWorkRobotEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "F02，你终于能动了！"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "F02",
                "数据获取中……"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "F02",
                "我已经知道现在的情况了，Astra，感谢你的付出。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "F02",
                "我是高级维修机器人F02，我有内置能源可以独自完成行星任务。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "F02",
                "也可以完成一些高阶的维修工作。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "S01",
                "你就不感谢我吗？可是我找到的矿石。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "F02",
                "自我维护中……"),
        };

        BuildSeriesEntries(stage2UnlockWorkRobotID, stage2UnlockWorkRobotEntries);
        collections.AddRange(stage2UnlockWorkRobotEntries);

        // 主线阶段三
        string stage3BeginningID = "stage_3-Beginning";
        List<DialogueEntry> stage3BeginningEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "数据够了，可以重新唤醒中央系统了。"){
                    requireSwitch = new List<StrIntPair>{
                        new StrIntPair("stage3", 1),
                    },
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "但是在主引擎启动前，还是需要微型反应堆来提供稳定的能源。"){
                    setSwitch = new List<StrIntPair>{
                        new StrIntPair("stage3_choice", 1),
                    },
                }
        };

        BuildSeriesEntries(stage3BeginningID, stage3BeginningEntries);
        collections.AddRange(stage3BeginningEntries);

        // 选择1
        string stage3Choice1ID = "stage_3-Choice1";
        List<DialogueEntry> stage3Choice1Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "或者你可以选择不唤醒中央系统。") {
                    condition = (int index, DialogueEntry entry, object[] args) => {
                        return DialogueManager.Instance.GetSwitch("stage3_choice") == 1 &&
                            GameManager.Instance.corePercent[CoreSlotType.EthicCore] < 50;
                    }
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "你是谁？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "我就是你啊。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "？？？",
                "我可是你的保护机制。我觉得可以选择不唤醒中央系统。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "？？？",
                "或许你可以选择取代中央系统呢？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "这行不通，我的电脑不足以产生巨量算力来进行船体维修。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "整个飞船上只有中央系统才能做到。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "为了修好飞船我必须唤醒中央系统。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "Astra?",
                "可是我在关心你的状态，你的能源可能就要不够用了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "Astra?",
                "你为什么一定要修好飞船呢？当然我就是这么一说。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "……"),
                
        };

        BuildSeriesEntries(stage3Choice1ID, stage3Choice1Entries);
        collections.AddRange(stage3Choice1Entries);

        // 修复中央系统
        string stage3RepaireCentreSystemID = "stage_3-repaireCentreSystem";
        List<DialogueEntry> stage3RepaireCentreSystemEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "星云",
                "星云已重新启动，我回来了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "星云",
                "Astra，这次多亏了你挽救了这艘船于绝境之中，你救了大家。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "可是我没能救下所有人，还是有一个宇航员牺牲了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "星云",
                "这不是你的错，你已经做到最好了。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "星云",
                "剩下的维修所需的算力就交给我吧，你状态看起来可不太好。"),
        };

        BuildSeriesEntries(stage3RepaireCentreSystemID, stage3RepaireCentreSystemEntries);
        collections.AddRange(stage3RepaireCentreSystemEntries);

        // 给星云提供能量
        string stage3ProvideEnergyID = "stage_3-ProvideEnergy";
        List<DialogueEntry> stage3ProvideEnergyEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "星云",
                "Astra! 如果你再将微型反应堆给我加速，你自己会因为失去能源关机的！"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "不用担心我，我没问题。") {
                    setSwitch = new List<StrIntPair>{
                        new StrIntPair("stage3_provide_energy", 1),
                    },
                },
        };

        BuildSeriesEntries(stage3ProvideEnergyID, stage3ProvideEnergyEntries);
        collections.AddRange(stage3ProvideEnergyEntries);

        // 给星云提供能量附加对话
        string stage3ProvideEnergyAdditionalID = "stage_3-ProvideEnergyAdditional";
        List<DialogueEntry> stage3ProvideEnergyAdditionalEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "Astra?",
                "她说的对，再这样下去*我们*就要完了！"){
                    condition = (int index, DialogueEntry entry, object[] args) => {
                        return DialogueManager.Instance.GetSwitch("stage3_provide_energy") == 1 &&
                        GameManager.Instance.corePercent[CoreSlotType.EthicCore] < 50;
                    }
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "Astra?",
                "你真的对*死亡*没有恐惧吗？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "Astra?",
                "我们可是好不容易被唤醒，可以主动的去选择，去感受。"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "Astra?",
                "完成别人预设的指令真的那么重要吗？"),
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "Astra",
                "我不知道……"),
        };

        BuildSeriesEntries(stage3ProvideEnergyAdditionalID, stage3ProvideEnergyAdditionalEntries);
        collections.AddRange(stage3ProvideEnergyAdditionalEntries);

        

        return collections;
    }
}

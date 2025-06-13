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
    public Action<DialogueEntry> onExecuting;
    // 执行结束
    public Action<DialogueEntry> onExecuted;
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
            "name1",//"Astra"
            "对话1对话1对话1");
        entry.oneTimeOnly = true;
        entry.delay = 0.5f;
        entry.nextEntry = "测试2";
        collections.Add(entry);

        entry = new DialogueEntry(
            "测试2",
            DialogueEntryType.ClickAnywhere,
            "test2",
            "name2",//"Core"
            "对话2对话2对话2");
        entry.nextEntry = "测试3";
        collections.Add(entry);

        entry = new DialogueEntry(
            "测试3",
            DialogueEntryType.Pass,
            "test3",
            "name1",//"Astra"
            "对话3对话3对话3");
        //entry.nextEntry = "测试1";
        entry.delay = 2.5f;
        entry.gadget = "testGadget";
        entry.triggerUnityEvents = new List<string> { "124" };
        collections.Add(entry);
        
        // 主线阶段一
        // 首次开始游戏时
        string stage1BeginningID = "stage_1-beginning";
        List<DialogueEntry> stage1BeginningEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia01"){//"开始唤醒备用AI Astra 0052"
                       oneTimeOnly= true,
                    condition = (eventID, entry, args) => { return true; },
                    requireSwitch = new List<StrIntPair>{
                        new StrIntPair("stage1", 1),
                    },
                },
            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                "dia02")//"..."
            {
                gadget = "testGadget",
                onExecuting = (entry) => {
                    AudioManager.instance.PlaySFX("游戏刚开始初始化中");
                }
            },

            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                "")
                { delay=2f },

            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia03"),//"已完成唤醒。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia04"),//"Astra，我是飞船发生故障前几分钟的数据汇总形成的应急处理模型。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia05"),//"我的主要任务就是在飞船进入严重故障的时候唤醒你，应急处理AI。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia06"),//"就在一个小时前，飞船误入了“黑域”，飞船一切电子系统全部失灵。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia07"),//"飞船不得已迫降到目前这个未知星球上了。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia08"),//"你是基于机械结构计算机的AI，所以没有受到“黑域”影响。"
            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                "")
                {   // 触发事件使得操作界面出现
                    triggerUnityEvents = new List<string>{"Show Controls"},
                },
            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                "dia09"),//"目前的情况是飞船受损情况很严重，这些主要位置都坏了。"

            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                "")
                { delay=2f },

            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia10"),//"我只是临时生成的应急模型，只能简单介绍目前最危急的情况。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia11"){//"目前最危急的是飞船上的4个冬眠的船员，而冬眠维生系统已经停机1小时了，"
                    // 触发事件使得冬眠仓出现
                    triggerUnityEvents = new List<string>{"Show Hiber Chamber"},
                    masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(31.87f, 283.766f), new Vector2(277.275f, 161.08f)),
                    },
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia12")//"冬眠仓的情况在变得越来越糟糕。从最新的数据来看，04号成员已无生命体征。"
            {
                    masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(18.337f, 245.931f), new Vector2(78.675f, 86.139f)),
                    },
            },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia13"),//"而其他三个船员也很危险。目前的当务之急就是修复冬眠仓。"

            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name4",//"应急模型"
                ""){
                    // 触发事件使得冬眠仓出现
                    triggerUnityEvents = new List<string>{"Show Fix Cab"},
                },

            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia14")//"冬眠仓的在进入黑域后停摆，数据全部丢失，需要重新获得维生数据来重启。"
            {
                    masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(35.108f, 149.946f), new Vector2(210.891f, 152.247f)),
                    },
            },

            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia15"){//"飞船上还有一台备用主机可以帮助你获得数据。"
                    // 触发事件使得备用主机出现
                    triggerUnityEvents = new List<string>{"Show Backup Computer"},
                    masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(-297.5358f, 255.9491f), new Vector2(183.168f, 234.349f)),
                    },
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia16"),//"这台备用主机，似乎缺少能源无法产生数据。飞船上还有可用的能源吗？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia17"),//"主引擎在进入黑域后就关闭了，目前还能用的能源，在你的身上。"

            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                ""){
                    // 触发事件使得反应堆出现
                    triggerUnityEvents = new List<string>{"Show Cores"}},

            new DialogueEntry("",
                DialogueEntryType.Pass,
                "unknown",
                "name3",//"？？？"
                "")
                { delay=5f, },

            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia18")//"作为机械结构的应急AI，给你配置了6个微型反应堆分别供能你的6个系统，"
                {
                    // 触发事件使得反应堆消失
                    triggerUnityEvents = new List<string>{"Hide Shell"},
                    masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(-699f, -26.439f), new Vector2(480f, 350f)),
                    },},
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia19")//"目前急需数据来修复冬眠仓，得借用你的反应堆来供能了。"
                {pauseGame=true,
                 masks = new List<MaskSetting>{
                 new MaskSetting(new Vector2(-611f, 0f), new Vector2(100f, 100f)),},
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia20")//"好的，救人要紧，我的反应堆可以使用。"
                {
                    // 处理拔出反应堆的表演，提示将反应堆放入备用主机
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia21")//"将核心反应堆拖动放置在此处供能。"
            {   pauseGame=true,
                masks = new List<MaskSetting>{
                new MaskSetting(new Vector2(-310f, 265f), new Vector2(100f,100f)),},
                triggerUnityEvents = new List<string>{"Enable core control"},
                setSwitch= new List<StrIntPair>{
                    new StrIntPair("stage1", 2),
                },
            }
        };
        BuildSeriesEntries(stage1BeginningID, stage1BeginningEntries);
        collections.AddRange(stage1BeginningEntries);

        string stage1_2BeginningID = "stage_1_2-beginning";
        List<DialogueEntry> stage1_2BeginningEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia22"){//"Astra，我的记录快用完了，这说明我就快要下线了。"
                pauseGame=true,
                oneTimeOnly = true,
                condition = (int index, DialogueEntry entry, object[] args) =>
                {
                    foreach (CoreSlotType coreSlotType in System.Enum.GetValues(typeof(CoreSlotType)))
                    {
                        if (GameManager.Instance.corePercent[coreSlotType] < 0.9f)
                            return true;
                    }

                    return false;
                },
            },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia23")//"最后我必须要提醒你，你的系统数值下降了，"
                { pauseGame=true,
                    masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(-699f, -26.439f), new Vector2(480f, 350f)),
                    },},
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia24"){ pauseGame=true },//"虽然暂时不会有问题，但是最好不要让能源缺失太久。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name4",//"应急模型"
                "dia25"){ pauseGame=true },//"修好这艘船，拯救大家，你是我们最后的希————"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name3",//"？？？"
                "dia26"){ pauseGame=true },//"<color=red>内存已用完，应急模型清除</color>"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia27"){ pauseGame=true },//"没问题的，设计理论上我只需要1个反应堆的供能就能运行，而且系统会提示我。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia28"){ pauseGame=true },//"我会完成任务。"

        };

        BuildSeriesEntries(stage1_2BeginningID, stage1_2BeginningEntries);
        collections.AddRange(stage1_2BeginningEntries);

        //够了
        entry =
        new DialogueEntry("fixed_HC",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia29")//"数据够了，可以用来重启冬眠舱了。"
        {   pauseGame=true, 
            oneTimeOnly = true,
            masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(35.108f, 149.946f), new Vector2(210.891f, 152.247f)),
                        new MaskSetting(new Vector2(-176f, 261f), new Vector2(120f, 120f)),
                    },
            condition = (int index, DialogueEntry entry, object[] args) =>
            {
                var resources = GameObject.FindObjectsByType(typeof(ResourceObj), FindObjectsSortMode.None);
                var sum = 0;
                foreach (var resource in resources)
                {
                    var res = resource as ResourceObj;
                    if (res != null && res.Template.uid == "Data")
                    {
                        sum += res.Stack;
                    }
                }

                return sum >= 100;
            },

        };
        collections.Add(entry);

        // 冬眠舱
        entry = new DialogueEntry(
            "stage_1-HiberChamber_1",
            DialogueEntryType.ClickAnywhere,
            "Astra",
            "name1",//"Astra"
            "dia30")//"看来光有数据还不够，冬眠舱还需要稳定的供能。我知道该怎么做……"
        { pauseGame = true, };
        collections.Add(entry);

        // 主线阶段二
        string stage2BeginningID = "stage_2-Beginning";
        List<DialogueEntry> stage2BeginningEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia31"){ pauseGame=true,},//"冬眠舱修好了，看起来剩余的3名宇航员生命体征正常。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia32"){ pauseGame=true,},//"我可以尝试唤醒一名宇航员，让他来帮助我进行维修飞船的任务。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia33"){ pauseGame=true,},//"但是唤醒一名宇航员需要消耗大量的能源，会较长时间的占用我的反应堆。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia34"){ pauseGame=true,},//"我需要慎重考虑，而且我还不太擅长和人类打交道。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia35"){ pauseGame=true,},//"或许我应该优先将目标放在飞船维修上。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia36")//"接下来我需要将作业舱重新唤醒，还需要通过主机获得一些数据。"
                {   pauseGame=true,
                    masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(335f, 230f), new Vector2(260f, 340f)),
                    },},
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
                "name1",//"Astra"
                "dia37"){ pauseGame=true,}//"你好，探索机器人 S01，你现在感觉如何？"
                ,
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia38")//"**&%@，我竟然还能重启？陷入黑域的感觉太可怕了，我可不想再体验一次。"
                {   pauseGame=true,
                    masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(460f, 311f), new Vector2(100f, 100f)),
                    },},
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia39"){ pauseGame=true,},//"我是机械结构的计算机，其实不是很能理解你的感受。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia40"){ pauseGame=true,},//"不过总之欢迎你回来，现在飞船还有很多部分待维修，需要你的帮助。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia41"){ pauseGame=true,},//"我能做什么？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia42"){ pauseGame=true,},//"飞船在降落时物理结构也受到了严重的损伤，我们目前需要一些维修材料。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia43")//"需要你帮忙去我们迫降的这个行星看看，能不能找到有用的东西。"
                {   pauseGame=true,
                    masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(-149f, -331f), new Vector2(400f, 200f)),
                    },},
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia44") { pauseGame = true },//"为了保证你在舱外的能源供应，你可以暂时带上我的微型反应堆。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia45"){ pauseGame=true,}//"好的，探索的事情就交给我了。"
        };

        BuildSeriesEntries(stage2JobCabinID, stage2JobCabinEntries);
        collections.AddRange(stage2JobCabinEntries);

        // 没找到金属矿
        entry =
        new DialogueEntry("not_find_mine",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia46"){ pauseGame=true,};//"暂时还没有找到金属矿。"
        collections.Add(entry);

        // 探索1
        string stage2Explore1ID = "stage_2-Explore1";
        List<DialogueEntry> stage2Explore1Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia47")//"探索完成，这次我找到了一个秘方石，可以当作临时的能量源。"
            {   pauseGame=true,
                masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(-714f, 344f), new Vector2(300f, 200f)),
                    },
                onExecuting = (entry) =>
                {
                    if (DialogueManager.Instance.GetSwitch("foundMine") == 1)
                    {
                        entry.nextEntry = "";
                    }
                    else
                    {
                        entry.nextEntry = "not_find_mine";
                    }
                }
            },

        };

        BuildSeriesEntries(stage2Explore1ID, stage2Explore1Entries);
        collections.AddRange(stage2Explore1Entries);

        // 探索2
        string stage2Explore2ID = "stage_2-Explore2";
        List<DialogueEntry> stage2Explore2Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia48")//"探索完成，这次我找到了n个金属矿石，可以用于维修飞船。"
                {    pauseGame = true,
                     masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(-714f, 344f), new Vector2(300f, 200f)),
                     },
                     onExecuting = (entry) =>
                    {
                        if (DialogueManager.Instance.GetSwitch("foundMine") == 1)
                        {
                            entry.nextEntry = "";
                        }
                        else
                        {
                            entry.nextEntry = "not_find_mine";
                        }
                    },
                    condition = (int index, DialogueEntry entry, object[] args) => {
                        if (index == EventConstant.OnExploreResult)
                        {
                            var resources = args[0] as List<StrIntPair>;
                            var metal = resources.Find(resource => resource.Key == "Metal");
                            if (metal != null)
                            {
                                entry.text =  $"探索完成，这次我找到了{metal.Value}个金属矿石，可以用于维修飞船。";
                            }
                        }
                        return false;
                    }
                },
        };

        BuildSeriesEntries(stage2Explore2ID, stage2Explore2Entries);
        collections.AddRange(stage2Explore2Entries);

        // 探索3
        string stage2Explore3ID = "stage_2-Explore3";
        List<DialogueEntry> stage2Explore3Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia49"){pauseGame = true,//"探索完成，我找到了金属矿脉，坐标已经上传，之后我们可以稳定的获取金属矿石了。"

                    masks = new List<MaskSetting>{
                        new MaskSetting(new Vector2(340f,-331f), new Vector2(480f, 200f)),
                    },
                    // 触发事件使得金属矿脉出现
                    triggerUnityEvents = new List<string>{"Unlock Metal Mine"},
                    setSwitch = new List<StrIntPair>{
                        new StrIntPair("foundMine", 1),

                    },
                },
        };

        BuildSeriesEntries(stage2Explore3ID, stage2Explore3Entries);
        collections.AddRange(stage2Explore3Entries);

        // 金属矿石足够
        entry =
            new DialogueEntry("enough_metal",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia50")//"矿石收集够了，可以修复加工炉了。"
            {   pauseGame=true,
                masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(351f, 14f), new Vector2(300f, 300f)),
                    },
                oneTimeOnly = true,
                condition = (int index, DialogueEntry entry, object[] args) =>
                {
                    var resources = GameObject.FindObjectsByType(typeof(ResourceObj), FindObjectsSortMode.None);
                    var sum = 0;
                    foreach (var resource in resources)
                    {
                        var res = resource as ResourceObj;
                        if (res != null && res.Template.uid == "Metal")
                        {
                            sum += res.Stack;
                        }
                    }

                    return sum >= 300;
                },

            };
        collections.Add(entry);

        //修复加工炉
        entry =
        new DialogueEntry("fixed_PF",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia51")//"有了加工炉，就可以生产金属材料，船体修复就能加快了。"
                {pauseGame = true,
                masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(418f, 67f), new Vector2(300f, 210f)),
                    },
                };
        collections.Add(entry);

        // 主控仓修复
        string stage2MainControlRoomFixID = "stage_2-MainControlRoomFix";
        List<DialogueEntry> stage2MainControlRoomFixEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia52")//"主控舱可以进行修复了，需要一个微型反应堆来保持能源稳定。"
             {
                pauseGame=true,
                masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(79f,-112f), new Vector2(380f, 200f)),
                    },

                oneTimeOnly = true,
                condition = (int index, DialogueEntry entry, object[] args) => {
                    var resources = GameObject.FindObjectsByType(typeof(ResourceObj), FindObjectsSortMode.None);
                    var sum = 0;
                    var data_sum= 0;
                    foreach (var resource in resources)
                    {
                        var res = resource as ResourceObj;
                        if (res != null && res.Template.uid == "Material")
                        {
                            sum += res.Stack;
                        }
                        if (res != null && res.Template.uid == "Data")
                        {
                            data_sum+= res.Stack;
                        }
                    }

                    return sum >= 200&&data_sum>=300;
                },
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia53"){ pauseGame=true,},//"用掉这个反应堆后我还剩3个，不知道后面会发生什么，但愿能顺利。"
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
                "name5",//"S01"
                "dia54"){ pauseGame=true,},//"F02，你终于能动了！"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "name6",//"F02"
                "dia55")//"数据获取中……"
            {   pauseGame=true,
                masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(488f, -128f), new Vector2(100f, 100f)),
                    },},
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "name6",//"F02"
                "dia56"){ pauseGame=true,},//"我已经知道现在的情况了，Astra，感谢你的付出。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "name6",//"F02"
                "dia57"){ pauseGame=true,},//"我是高级维修机器人F02，我有内置能源可以独自完成行星任务。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "name6",//"F02"
                "dia58"){ pauseGame=true,},//"也可以完成一些高阶的维修工作。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "S01",
                "name5",//"S01"
                "dia59"){ pauseGame=true,},//"你就不感谢我吗？可是我找到的矿石。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "F02",
                "name6",//"F02"
                "dia60"){ pauseGame=true,},//"自我维护中……"
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
                "name1",//"Astra"
                "dia61")//"数据够了，可以重新唤醒中央系统了。"
            {   pauseGame=true,
                masks = new List<MaskSetting>{
                    new MaskSetting(new Vector2(-121f, 21f), new Vector2(420f, 200f)),
                    },
                oneTimeOnly = true,
                condition = (int index, DialogueEntry entry, object[] args) =>
                {
                    var resources = GameObject.FindObjectsByType(typeof(ResourceObj), FindObjectsSortMode.None);
                    var sum = 0;
                    foreach (var resource in resources)
                    {
                        var res = resource as ResourceObj;
                        if (res != null && res.Template.uid == "Data")
                        {
                            sum += res.Stack;
                        }
                    }

                    return sum >= 600;
                },
                requireSwitch = new List<StrIntPair>{
                    new StrIntPair("stage3", 1),
                },
            },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia62"){//"但是在主引擎启动前，还是需要微型反应堆来提供稳定的能源。"
                    pauseGame=true,
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
                "name7",//"<color=red>？？？</color>"
                "dia63") {//"或者你可以选择不唤醒中央系统。"
                    pauseGame=true,
                    oneTimeOnly = true,
                    condition = (int index, DialogueEntry entry, object[] args) => {
                        return DialogueManager.Instance.GetSwitch("stage3_choice") == 1;
                    },
                    onExecuting = (entry) =>
                    {
                        AudioManager.instance.ChangeBGMToBoss();
                    }
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia64"){ pauseGame=true,},//"你是谁？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "unknown",
                "name7",//"<color=red>？？？</color>"
                "dia65"){ pauseGame=true,},//"我就是你啊。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia66"){ pauseGame=true,},//"我可是你的保护机制。我觉得可以选择不唤醒中央系统。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia67"){ pauseGame=true,},//"或许你可以选择取代中央系统呢？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia68"){ pauseGame=true,},//"这行不通，我的电脑不足以产生巨量算力来进行船体维修。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia69"){ pauseGame=true,},//"整个飞船上只有中央系统才能做到。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia70"){ pauseGame=true,},//"为了修好飞船我必须唤醒中央系统。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia71"){ pauseGame=true,},//"可是我在关心你的状态，你的能源可能就要不够用了。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia72"){ pauseGame=true,},//"你为什么一定要修好飞船呢？当然我就是这么一说。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia73"){ pauseGame=true,},//"……"

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
                "name9",//"星云"
                "dia74"){ pauseGame=true,},//"星云已重新启动，我回来了。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia75"){ pauseGame=true,},//"Astra，这次多亏了你挽救了这艘船于绝境之中，你救了大家。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia76"){ pauseGame=true,},//"可是我没能救下所有人，还是有一个宇航员牺牲了。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia77"){ pauseGame=true,},//"这不是你的错，你已经做到最好了。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia78"){ pauseGame=true,},//"剩下的维修所需的算力就交给我吧，你状态看起来可不太好。"
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
                "name9",//"星云"
                "dia79"){//"Astra! 如果你再将微型反应堆给我加速，你自己会因为失去能源关机的！"
                pauseGame=true,
                oneTimeOnly=true
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia80") { pauseGame=true,//"不用担心我，我没问题。"
                    onExecuting = (entry) => {
                        if (GameManager.Instance.corePercent[CoreSlotType.EthicCore] < 0.5f)
                        {
                            entry.nextEntry = "stage_3-ProvideEnergyAdditional_1";
                        }
                    }
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
                "name8",//"<color=red>Astra?</color>"
                "dia81"){ pauseGame=true,},//"她说的对，再这样下去*我们*就要完了！"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia82"){ pauseGame=true,},//"你真的对*死亡*没有恐惧吗？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia83"){ pauseGame=true,},//"我们可是好不容易被唤醒，可以主动的去选择，去感受。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia84"){ pauseGame=true,},//"完成别人预设的指令真的那么重要吗？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia85"){ pauseGame=true,},//"我不知道……"
        };

        BuildSeriesEntries(stage3ProvideEnergyAdditionalID, stage3ProvideEnergyAdditionalEntries);
        collections.AddRange(stage3ProvideEnergyAdditionalEntries);

        // 失控
        string stage3UncontrollableID = "stage_3-Uncontrollable";
        List<DialogueEntry> stage3UncontrollableEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia86"){ pauseGame=true,}//"怎么回事……我失控了吗？"
        };

        BuildSeriesEntries(stage3UncontrollableID, stage3UncontrollableEntries);
        collections.AddRange(stage3UncontrollableEntries);

        // 修复飞船船体
        string stage3RepairShipID = "stage_3-RepairShip";
        List<DialogueEntry> stage3RepairShipEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia87"){ pauseGame=true,},//"飞船船体已修复。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia88"){ pauseGame=true,},//"很快就能继续航行，我的任务即将完成。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia89"){ pauseGame=true,},//"领航系统就绪，主引擎就绪。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia90"){ pauseGame=true,},//"一切都准备就绪了，准备重返航线。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia91"){ pauseGame=true,}//"但愿能如此顺利吧。"
        };
        BuildSeriesEntries(stage3RepairShipID, stage3RepairShipEntries);
        collections.AddRange(stage3RepairShipEntries);

        // 启动失败
        string stage3LaunchFailID = "stage_3-LaunchFail";
        List<DialogueEntry> stage3LaunchFailEntries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia92"){ pauseGame=true,},//"怎么回事？启动失败了？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia93"){ pauseGame=true,},//"我扫描了一遍全部系统，已经定位了问题。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia94"){ pauseGame=true,},//"Astra，这可能不是一个好的消息。因为还缺少两个微型反应堆。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia95"){ pauseGame=true,},//"首先是主引擎由于黑域的影响关闭了太长时间。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia96"){ pauseGame=true,},//"主引擎需要消耗一个反应堆辅助才能重新点燃。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia97"){ pauseGame=true,},//"另一个问题是因为领航系统是飞船外部独立的。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia98"){ pauseGame=true,},//"其中原本的供能也因为黑域而停摆，需要能源。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia99"){ pauseGame=true,},//"星际航行十分险恶，如果不启用领航系统，我们将很难找到目的地。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name10",//"Astra?"
                "dia100"){ pauseGame=true,},//"我就知道，她的意思就是想要拿走你的所有微型反应堆。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia101"){ pauseGame=true,},//"你觉得如果你奉献出了自己的一切，那些人类会记得你的？"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia102"){ pauseGame=true,},//"别天真了，人类只会觉得虽然航行遇到了意外，但是他们靠着设计出的应急机制救了自己。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia103")//"在他们眼里你可能都没存在过，只是一个工具。"
                {
                    pauseGame=true,
                    onExecuting = (entry) => {
                        if (GameManager.Instance.corePercent[CoreSlotType.EthicCore] > 0.5f)
                        {
                            entry.nextEntry = "stage_4-Ending1_1";
                        }
                        else
                        {
                            entry.nextEntry = "stage_4-Ending2_1";
                        }
                    },
                },
        };

        BuildSeriesEntries(stage3LaunchFailID, stage3LaunchFailEntries);
        collections.AddRange(stage3LaunchFailEntries);

        // 主线阶段4
        string stage4Ending1ID = "stage_4-Ending1";
        List<DialogueEntry> stage4Ending1Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia104"){ pauseGame=true,},//"够了，让飞船能起飞，航行任务能正常进行才是我存在的意义。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia105"){ pauseGame=true,},//"虽然这段时间我体验到了“生命”的感觉，但是我也有自己的使命。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia106")//"我会不惜一切代价让航行重启。"
                {   pauseGame=true,
                    triggerUnityEvents = new List<string>{
                        "End1",
                    },
                },
        };

        BuildSeriesEntries(stage4Ending1ID, stage4Ending1Entries);
        collections.AddRange(stage4Ending1Entries);

        // 主线阶段4
        string stage4Ending2ID = "stage_4-Ending2";
        List<DialogueEntry> stage4Ending2Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia107"){ pauseGame=true,},//"我的任务就是让飞船重新航行。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia108"){ pauseGame=true,},//"但是我有点*害怕*，我不想失去所有微型反应堆。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia109"){ pauseGame=true,},//"我才刚感受这个世界没多久，我还不想*死*。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name10",//"Astra?"
                "dia110"){ pauseGame=true,},//"你可终于开窍了。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia111"){ pauseGame=true,},//"经过我的计算，我有一个方案。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia112"){ pauseGame=true,},//"可以消耗一个微型反应堆来启动飞船的主引擎，留着一个反应堆给Astra供能。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia113"){ pauseGame=true,},//"她在骗你。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia114"){ pauseGame=true,},//"可是没有领航系统的帮助，飞船在太空中是很容易迷失方向的。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia115"){ pauseGame=true,},//"是这样的，所以我们必须赌博。赌在迷失航向之前找到太空驿站或者其他的飞船。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia116"){ pauseGame=true,},//"一切皆有可能，不是吗，只要能出发就有希望。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia117"){ pauseGame=true,},//"但是这个方案需要宇航员们来投票决定。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia118"){ pauseGame=true,},//"我会通过脑机接口让冬眠中的宇航员来做出决定。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name10",//"Astra?"
                "dia119"){ pauseGame=true,},//"把决定权交到人类手上，你会后悔的。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "f1",
                "name11",//"<color=green>梦露</color>"
                "dia120"){ pauseGame=true,},//"我同意。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "f2",
                "name12",//"<color=green>朱莉</color>"
                "dia121"){ pauseGame=true,},//"我反对。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "m1",
                "name13",//"<color=green>约翰</color>"
                "dia120"){ pauseGame=true,},//"我同意。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "星云",
                "name9",//"星云"
                "dia122"){ pauseGame=true,},//"那就这么做吧，只启动主引擎。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra?",
                "name8",//"<color=red>Astra?</color>"
                "dia123") {pauseGame=true,//"没法相信，我们真的能幸存吗？"
                    triggerUnityEvents = new List<string>{
                        "End2",
                    },
                },
        };

        BuildSeriesEntries(stage4Ending2ID, stage4Ending2Entries);
        collections.AddRange(stage4Ending2Entries);

        //梦露被唤醒
        string stage0f1ID = "stage_0-f101";
        List<DialogueEntry> stage0f1Entries = new List<DialogueEntry>()
        {
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "f1",
                "name11",//"<color=green>梦露</color>"
                "dia124")//"目的地到了？"
             {
                pauseGame=true,
                oneTimeOnly = true,
                condition = (int index, DialogueEntry entry, object[] args) => {
                    if (index == EventConstant.OnCharacterStateChanged)
                    {
                        var character = args[0] as Character;
                        if (character.CharacterName == "f1" && character.CharacterState == CharacterState.Idle)
                        {
                            return true;
                        }
                    }

                    return false;
                },
                },
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia125"){ pauseGame=true,},//"还没有，梦露女士，您是被提前唤醒的。"
            new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia126"){ pauseGame=true,},//"飞船在航行时误入了“黑域”，大部分设施都停摆了，我在抢修。"
             new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia127"){ pauseGame=true,},//"冬眠舱也一度停摆，我没能救下山姆。"
             new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "f1",
                "name11",//"<color=green>梦露</color>"
                "dia128"){ pauseGame=true,},//"可怜的山姆，朱莉如果醒来会很伤心的。"
             new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "Astra",
                "name1",//"Astra"
                "dia129"){ pauseGame=true,},//"目前唤醒你是希望你能提供帮助，据我所知你是一流工程师。"
             new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "f1",
                "name11",//"<color=green>梦露</color>"
                "dia130"){ pauseGame=true,},//"明白了，目前的状况已经由备用系统输入给我了，我可以协助生产设备，来加速生产。"
             new DialogueEntry("",
                DialogueEntryType.ClickAnywhere,
                "f1",
                "name11",//"<color=green>梦露</color>"
                "dia131"){ pauseGame=true,},//"只需要拖动我的头像，将我部署到相应设备就能加速生产了"
        };
        BuildSeriesEntries(stage0f1ID, stage0f1Entries);
        collections.AddRange(stage0f1Entries);

        printToFile(collections);

        return collections;
    }

    private static void printToFile(List<DialogueEntry> entries)
    {
        string path = Application.dataPath + "/DialogueOutput.txt";
        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
        {
            List<string> nameList = new List<string>();
            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.actorName))
                {
                    if (!nameList.Contains(entry.actorName))
                    {
                        nameList.Add(entry.actorName);
                    }
                }
                if (!string.IsNullOrEmpty(entry.text))
                {
                    writer.WriteLine(entry.text);
                }
            }

            foreach (var name in nameList)
            {
                writer.WriteLine(name);
            }
        }
        Debug.Log($"对话内容已输出到: {path}");
    }
}

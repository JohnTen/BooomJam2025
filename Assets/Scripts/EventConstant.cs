
using UnityEngine.AI;

public static class EventConstant
{
    private const int mask = 100000000;
    private const int offset = 100000;

    public const int ResourceRequirementMet = 2 * offset + 1;
    public const int ResourceRequirementFinished = 2 * offset + 2;
    public const int OnCharacterStateChanged = 2 * offset + 3;
    public const int OnCharacterSlotChanged = 2 * offset + 4;
    public const int ResourceGeneratorStarted = 2 * offset + 5;
    public const int ResourceGeneratorStopped = 2 * offset + 6;
    public const int AustronautAwaked = 2 * offset + 7;
    public const int OnExploreResult = 2 * offset + 8;
    public const int CoreStageChanged = 2 * offset + 9;

    public const int AsyncSceneActivating = 3 * offset + 1;
    public const int AsyncSceneActivated = 3 * offset + 2;

    public const int OnDragStart = 4 * offset + 1;
    public const int OnDragEnd = 4 * offset + 2;
}

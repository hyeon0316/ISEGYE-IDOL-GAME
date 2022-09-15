public static class Global
{
    private const int _usingItemCount = 6;          // 배틀에 사용되는 아이템 최대 갯수
    private const int _itemQueueRepeat = 5;         // 아이템 순서 반복 횟수
    public const int ItemQueueLength = _usingItemCount * _itemQueueRepeat * 2;
    public const int MaxRoomPlayer = 8;
    
    //게임 첫 시작시 기본적으로 주워지는 아이템의 코드
    public const EItemCode DefaultItem1 = EItemCode.Item1;
    public const EItemCode DefaultItem2 = EItemCode.Item6;

    public const int SlotMaxCount = 10;

    public const int NextBattle = 12;

    public const int EmptySlotIndex = 255;
    
    public const int MaxUserNameSizeByByte = 22;
}

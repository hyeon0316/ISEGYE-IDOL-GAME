public static class Global
{
    private const int _usingItemCount = 6;          // 배틀에 사용되는 아이템 최대 갯수
    private const int _itemQueueRepeat = 5;         // 아이템 순서 반복 횟수
    public const int ItemQueueLength = _usingItemCount * _itemQueueRepeat * 2;
    public const int MaxRoomPlayer = 8;
}

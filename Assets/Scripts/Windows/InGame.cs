using UnityEngine;

/// <summary>
/// 아이템 드래그 관련 이벤트를 Ready창과 Battle창에 따라 나누기 위함
/// </summary>
public enum GameType
{
    Ready,
    Battle,
}

public class InGame : MonoBehaviour
{
    public GameObject BattleWindow;
    public GameObject ReadyWindow;

    public static GameType CurGameType = GameType.Ready;
    
    private void Start()
    {
        CloseBattle();
    }

    public void OpenBattle()
    {
        BattleWindow.SetActive(true);
        ReadyWindow.SetActive(false);
    }
    
    public void CloseBattle()
    {
        CurGameType = GameType.Ready;
        BattleWindow.SetActive(false);
        ReadyWindow.SetActive(true);
    }
}

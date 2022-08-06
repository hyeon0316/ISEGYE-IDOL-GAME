using UnityEngine;

public enum CursorType
{
    Nomal,
    Upgrade,
}

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] private Texture2D _nomalCursor;
    [SerializeField] private Texture2D _upgradeCursor;

    public CursorType CurCursorType;
    // Start is called before the first frame update
    void Start()
    {
        SetNomalCursor();
    }

    public void SetNomalCursor()
    {
        Cursor.SetCursor(_nomalCursor,Vector2.zero, CursorMode.ForceSoftware);
        CurCursorType = CursorType.Nomal;
    }
    
    public void SetUpgradeCursor()
    {
        Cursor.SetCursor(_upgradeCursor,Vector2.zero, CursorMode.ForceSoftware);
        CurCursorType = CursorType.Upgrade;
    }

    
}

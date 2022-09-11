using UnityEngine;

public enum ECursorType
{
    Nomal,
    Upgrade,
}

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] private Texture2D _nomalCursor;
    [SerializeField] private Texture2D _upgradeCursor;

    public ECursorType CurCursorType;
    // Start is called before the first frame update
    void Start()
    {
        SetNomalCursor();
    }

    public void SetNomalCursor()
    {
        Cursor.SetCursor(_nomalCursor,Vector2.zero, CursorMode.ForceSoftware);
        CurCursorType = ECursorType.Nomal;
    }
    
    public void SetUpgradeCursor()
    {
        Cursor.SetCursor(_upgradeCursor,Vector2.zero, CursorMode.ForceSoftware);
        CurCursorType = ECursorType.Upgrade;
    }

    
}

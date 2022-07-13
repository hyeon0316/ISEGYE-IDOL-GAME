using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Empty,
    Attack,
    Defense,
    Ability
}

public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemType _curItemType;

    public ItemType CurItemType
    {
        get { return _curItemType; }
        set { _curItemType = value; }
    }

    protected string Name;
    [TextArea]
    protected string Description;
    public uint Upgrade = 0;

    public uint Code { get; protected set; }
    
    public SetItem SetItem { get; set; }

    private Image _image;
    protected Sprite Sprite;
    
    public virtual void Awake()
    {
        _image = GetComponent<Image>();
    }

    public abstract void SetData(uint itemCode);
        
    /// <summary>
    /// 아이템 효과 사용
    /// </summary>
    public abstract void Active();

    public virtual void ShowUsingEffect()
    {
        //todo: 아이템 사용 시 나오는 이펙트 효과 
    }
        
    public virtual void Show()
    {
        //todo: 아이템 첫 등장 시 나오는 이펙트 효과
    }

    public virtual void Hide()
    {
        //todo: 아이템 사용이후 사라질때 이펙트 효과
    }
    
    

}

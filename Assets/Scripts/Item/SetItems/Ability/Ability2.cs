using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 햄버거
/// </summary>
public class Ability2 : Item
{
    public Sprite BurgerSprite;
    public Sprite BurgerSprite2;

    private int _spriteIndex = 0;

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(ChangeSprite());
    }
    public override void Active(BattlePlayer player, BattlePlayer opponent)
    {
        Debug.Log(this.GetType().Name);
        opponent.UpdateAvatarHp(-10);//임시
        
        ChangeColor(Color.black);
    }

    private IEnumerator ChangeSprite()
    {
        switch (_spriteIndex)
        {
            case 0:
                Image.sprite = Sprite;
                break;
            case 1:
                Image.sprite = BurgerSprite;
                break;
            case 2:
                Image.sprite = BurgerSprite2;
                break;
        }
        yield return new WaitForSeconds(1f);
        _spriteIndex++;
        if (_spriteIndex >= 3)
            _spriteIndex = 0;
        
        StartCoroutine(ChangeSprite());
    }

  
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : MonoBehaviour
{
    public int MaxAvaterHp;
    public int AvatarHp;//전투용 Hp
    public int AvaterDefense;
    public ItemSlot[] ItemSlots;
    public Image AvatarImage;
    public TextMeshProUGUI PlayerNickName;
    public TextMeshProUGUI AvatarHpText;
    public TextMeshProUGUI DefenseText;
    
    public Player Player { get; private set; }
    private byte[] _itemOrder;
    private int _index = 0;
    private bool _isMyturn = false;

    public BattlePlayer Opponent { get; set; }

    private int _remainValue; //특정 데미지를 받아 방어도가 전부 깎이고 체력에 데미지를 줄때의 값을 저장하기 위한 변수

    private bool _isCC; //출혈 등 상태이상에 걸렸을때에 대한 bool값
    private int _nestValue; //상태이상 데미지에 대한 중첩값이 담긴 변수
    
    
    public void SetBattlePlayer(Player player, byte[] itemOrder, BattlePlayer oppoent)
    {
        Player = player;
        _itemOrder = itemOrder;
        Opponent = oppoent;
        
        Player.SetItem(ItemSlots);
        _isMyturn = false;
        MaxAvaterHp = 100;
        AvatarHp = MaxAvaterHp;
        AvaterDefense = Player.Defense;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
        DefenseText.text = $"방어력: {AvaterDefense}";
        AvatarImage.sprite = Player.Sprite;
        PlayerNickName.text = $"{Player.NickName}";
        _index = 0;
    }

    public void UpdateAvatarHp(int amount)
    {
        if (amount < 0) //데미지
        {
            UpdateDefense(amount);
            AvatarHp += _remainValue;
            
            if (AvatarHp < 0) 
                AvatarHp = 0;
            else if(AvatarHp > MaxAvaterHp)
                AvatarHp = MaxAvaterHp;
        }
        else //회복
        {
            AvatarHp += amount;
            if (AvatarHp > MaxAvaterHp)
                AvatarHp = MaxAvaterHp;
        }

        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
    }

    public void UpdateDefense(int amount)
    {
        _remainValue = 0;
        AvaterDefense += amount;
        if (AvaterDefense < 0) 
        {
            _remainValue = AvaterDefense;
            AvaterDefense = 0;
        }

        DefenseText.text = $"방어력: {AvaterDefense}";
    }

    public void SetFirstTurn()
    {
        _isMyturn = true;
    }

    public void TakeCC(int amount)
    {
        _isCC = true;
        _nestValue += amount;
    }

    public void ClearCC()
    {
        _isCC = false;
        _nestValue = 0;
    }
    

    public void ActiveItem()
    {
        if (_isMyturn)
        {
            if (_isCC)//아이템 발동 전 자신에게 상태이상이 걸렸을 경우
            {
                Debug.Log("상태이상");
                ActiveCCEfect();
                UpdateAvatarHp(_nestValue);
            }
            
            int itemSlot;
            bool active = false;

            itemSlot = _itemOrder[_index++];
            active = Convert.ToBoolean(_itemOrder[_index++]); //나중에 확률아이템에 대한 bool값
            if (_index == _itemOrder.Length)
                _index = 0;

            if (active) 
            {
                ItemSlots[itemSlot].ActiveItem(this, Opponent);
            }
            else
            {
                Debug.Log("슬롯 잠김 OR 아이템 확률 실패");
                //todo: 아이템 잠겼을때 OR 아이템 확률 실패 했을 때 구분하여 이펙트 발생
            }
        }

        _isMyturn = !_isMyturn;
    }

    /// <summary>
    /// 프로토타입 용 출혈데미지 효과
    /// </summary>
    private void ActiveCCEfect()
    {
        StartCoroutine(ActiveCCEfectCo());
    }

    private IEnumerator ActiveCCEfectCo()
    {
        AvatarImage.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        AvatarImage.color = Color.white;
    }
    
}

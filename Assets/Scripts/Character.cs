using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ECharacterType : Byte
{
    Woowakgood,
    Ine,
    Jingburger,
    Lilpa,
    Jururu,
    Gosegu,
    Viichan,
    Empty
}

public class Character : MonoBehaviour
{

    [SerializeField]
    private ECharacterType _curCharType;

    public ECharacterType CurCharType => _curCharType;
    
    private Image _image;

    public Image Image => _image;
    
    private string _name;

    public string Name => _name;
    
    [TextArea]
    public string Description;


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        SetCharacter();
    }


    private void SetCharacter()
    {
        switch (_curCharType)
        {
            case ECharacterType.Woowakgood:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Woowakgood}/Woowakgood");
                _name = "우왁굳";
                break;
            case ECharacterType.Ine:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Ine}/Ine");
                _name = "아이네";
                break;
            case ECharacterType.Jingburger:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Jingburger}/Jingburger");
                _name = "징버거";
                break;
            case ECharacterType.Lilpa:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Lilpa}/Lilpa");
                _name = "릴파";
                break;
            case ECharacterType.Jururu:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Jururu}/Jururu");
                _name = "주르르";
                break;
            case ECharacterType.Gosegu:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Gosegu}/Gosegu");
                _name = "고세구";
                break;
            case ECharacterType.Viichan:
                _image.sprite = Resources.Load<Sprite>($"Characters/{ECharacterType.Viichan}/Viichan");
                _name = "비챤";
                break;
            
        }
    }
    
    public void ShowInfo()
    {
        WindowManager.Instance.Windows[(int) EWindowType.Select].GetComponent<Select>().ShowInfo(_name, Description, _image.sprite);
    }
    
    
}

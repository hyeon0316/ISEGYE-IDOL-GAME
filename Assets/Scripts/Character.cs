using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public enum CharacterType
    {
        Woowakgood,
        Ine,
        Jingburger,
        Lilpa,
        Jururu,
        Gosegu,
        Viichan
    }

    [SerializeField]
    private CharacterType _curCharType;
    
    private Image _image;
    private string _name;
    
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
            case CharacterType.Woowakgood:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Woowakgood}/Woowakgood");
                _name = "우왁굳";
                break;
            case CharacterType.Ine:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Ine}/Ine");
                _name = "아이네";
                break;
            case CharacterType.Jingburger:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Jingburger}/Jingburger");
                _name = "징버거";
                break;
            case CharacterType.Lilpa:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Lilpa}/Lilpa");
                _name = "릴파";
                break;
            case CharacterType.Jururu:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Jururu}/Jururu");
                _name = "주르르";
                break;
            case CharacterType.Gosegu:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Gosegu}/Gosegu");
                _name = "고세구";
                break;
            case CharacterType.Viichan:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Viichan}/Viichan");
                _name = "비챤";
                break;
            
        }
    }
    
    public void ShowInfo()
    {
        WindowManager.Instance.Windows[(int) WindowType.Select].GetComponent<Select>().ShowInfo(_name, Description, _image.sprite);
    }
    
    
}

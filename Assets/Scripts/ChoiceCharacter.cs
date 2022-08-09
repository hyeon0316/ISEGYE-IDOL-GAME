using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceCharacter : MonoBehaviour
{
    public int NetworkID;
    
    private Image _image;

    public Image Image => _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeCharacterImage(int type)
    {
        switch (type)
        {
            case 0:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Woowakgood}/Woowakgood");
                break;
            case 1:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Ine}/Ine");
                break;
            case 2:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Jingburger}/Jingburger");
                break;
            case 3:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Lilpa}/Lilpa");
                break;
            case 4:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Jururu}/Jururu");
                break;
            case 5:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Gosegu}/Gosegu");
                break;
            case 6:
                _image.sprite = Resources.Load<Sprite>($"Characters/{CharacterType.Viichan}/Viichan");
                break;
            
        }         
    }

}

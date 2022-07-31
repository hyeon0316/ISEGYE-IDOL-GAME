using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RandomButton : MonoBehaviour
{
    private Image _image;
    
    
    public void SetData(Sprite sprite)
    {
        _image = GetComponent<Image>();
        _image.sprite = sprite;
    }
}

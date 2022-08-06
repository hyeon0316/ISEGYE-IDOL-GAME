using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense2 : Item
{
   
    public override void SetData()
    {
        throw new System.NotImplementedException();
    }

    public override void Active()
    {
        Debug.Log(this.GetType().Name);
    }
}

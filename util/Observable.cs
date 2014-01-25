using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public abstract class Observable : MonoBehaviour {

    public event Action<object> Update;
	
    protected void RaiseUpdate(object obj)
    {
        if (Update != null)
            Update(obj);
    }
		
//    protected void RaiseUpdate(string propertyName)
//    {
//        if (Update != null)
//            Update(propertyName);
//    }
}

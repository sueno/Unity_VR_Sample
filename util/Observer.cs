using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Observer {

    Dictionary<Type, Action<object>> updateExpressions = new Dictionary<Type, Action<object>>();
    Dictionary<Type, Observable> dataSource = new Dictionary<Type, Observable>();
 
    public Observable DataSource
    {
        set {
            dataSource[value.GetType()] = value;
            value.Update += Update;
        }
    }
 
    protected void AddUpdateAction(Type type, Action<object> updateAction)
    {
        updateExpressions[type] = updateAction;
    }

	void Update(object obj)
    {
//		Debug.Log("obs obj : "+obj.GetType());
        Action<object> updateAction;
        if (updateExpressions.TryGetValue(obj.GetType(), out updateAction))
            updateAction(obj);
    }
//    void Update(string propertyName)
//    {
//		Debug.Log("obs obj : "+this.GetType());
//        Action<object> updateAction;
//        if (updateExpressions.TryGetValue(propertyName, out updateAction))
//            updateAction(dataSource.Eval(propertyName));
//    }
}

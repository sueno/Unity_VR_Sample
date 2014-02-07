using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

namespace Unity_VR.MainPlayer.Motion
{

    public static class ObjectExtensions
    {

        // オブジェクトの指定された名前のプロパティの値を取得
        public static object Eval(this object item, string propertyName)
        {
            var propertyInfo = item.GetType().GetProperty(propertyName);
            Debug.Log("propety  : " + propertyInfo + "    item : " + item);
            return propertyInfo == null ? null : propertyInfo.GetValue(item, null);
        }
    }
}

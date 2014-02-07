using UnityEngine;
using System.Collections;

namespace Unity_VR.Item
{

    public interface IItem
    {
        void hold();
        void release();

        void use();

        GameObject getGameObject();
    }

    public interface IUseAbleItem : IItem
    {
    }

    public interface IEatAbleItem : IItem
    {
    }

    public interface IEquipAbleItem : IItem
    {
    }
}
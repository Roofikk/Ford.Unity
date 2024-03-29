﻿using UnityEngine;

public class GroupBoneObjects : GroupBones
{
    [SerializeField] public bool _addChildrenBones = true;

    public override void Initiate()
    {
        base.Initiate();

        int countChildren = transform.GetComponentsInChildren<BoneObject>().Length;
        if (Bones.Count != countChildren)
        {
            if (_addChildrenBones)
            {
                Debug.LogWarning("Количество объектов в списке не соответствует количеству дочерних элементов. Дочерние элементы были добавлены в список");
                foreach (Transform tr in transform)
                {
                    if (tr.TryGetComponent(out BoneObject boneObj) && !_bones.Contains(boneObj))
                    {
                        _bones.Add(boneObj);
                    }
                }
            }
        }

        foreach (var bone in Bones)
        {
            bone.Initiate(this);
        }

        GroupBonesUI groupBonesUi = UiManager.Instance.CreateGroupBones();
        groupBonesUi.Initiate(this);
    }
}
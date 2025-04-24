using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimPlayer : UnitAnimPlayer
{
    const int PartNum = (int)UnitAnimPart.Max;
    public SpriteRenderer[] PartRenders = new SpriteRenderer[PartNum];
    public int[] PartTypeIDs = new int[PartNum];
    public Sex sex;
    string getPartAnimName(UnitAnimPart part,int typeId,UnitDirType dirType,UnitAnimType animType)
    {
        switch (part)
        {
            case UnitAnimPart.Body:
                return GetAnimName($"body{typeId:D3}",dirType,animType);
            case UnitAnimPart.Hair:
                return GetAnimName($"hair{typeId:D3}",dirType,animType);
            case UnitAnimPart.Weapon:
                return GetAnimName($"weapon{typeId:D3}",dirType,animType);
            default:
                break;
        }
        return "";
    }

    string getPartAnimDataPath(UnitAnimPart part, int typeId)
    {
        switch (part)
        {
            case UnitAnimPart.Body:
                return $"body/body{typeId:D3}_{(int)sex}";
            case UnitAnimPart.Hair:
                return $"hair/hair{typeId:D3}_{(int)sex}";
            case UnitAnimPart.Weapon:
                return $"weapon/weapon{typeId:D3}_{(int)sex}";
            default:
                break;
        }
        return "";

    }

    // Start is called before the first frame update
    void Start()
    {
        this.parts = new UnitPartInfo[PartNum];
        for (int i = 0; i < PartNum; i++)
        {
            parts[i] = new UnitPartInfo();
            parts[i].TypeID = PartTypeIDs[i];
            parts[i].Render = PartRenders[i];
        }
        ReloadRes();
        Play(this.animType,this.dirType);
    }

    string GetAnimName(string typeName,UnitDirType dirType,UnitAnimType animType) 
    {
        return $"{typeName}_{(int)sex}_{(int)dirType}{AnimNames[(int)animType]}";
    }

    void ReloadRes()
    {
        counter = 0;
        frameIndex = 0;
        // 加载各部位动画数据
        for (int i = 0; i < parts.Length; i++)
        {
            var typeId = parts[i].TypeID;
            var path = getPartAnimDataPath((UnitAnimPart)i, typeId);
            parts[i].AnimData = Resources.Load<FrameAnimData>(path);
            if (parts[i].AnimData == null)
            {
                Debug.LogError("Resources.Load fail:" + path);
            }
        }
    }


    public override void Play(UnitAnimType t,UnitDirType dirType)
    {
        base.Play(t, dirType);
        for (int i = 0; i < parts.Length; i++)
        {
            var typeId = parts[i].TypeID;
            var animName = getPartAnimName((UnitAnimPart)i, typeId,dirType,t);
            parts[i].Anim = parts[i].AnimData.AnimRes.Infos.Find(x => x.AnimName == animName);
            if (parts[i].Anim == null)
            {
                Debug.LogError("找不到动画：" + animName);
                return;
            }
        }
    }

}


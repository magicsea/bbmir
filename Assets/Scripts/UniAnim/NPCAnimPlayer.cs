using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NPCAnimPlayer : UnitAnimPlayer
{
    public static UnitAnimType[] NPCAnimTypes = { 
        UnitAnimType.Stand,
    };
    //翻转方向，动画是左右对称的
    public SpriteRenderer Render;
    public int TypeID;
    string getPartAnimName(int typeId)
    {
        return $"npc{typeId:D4}";
    }

    string getPartAnimDataPath(int typeId)
    {
        return $"npc/npc{typeId:D4}";
    }

    void Start()
    {
        parts = new UnitPartInfo[1];
        parts[(int)UnitAnimPart.Body] = new UnitPartInfo
        {
            TypeID = TypeID,
            Render = Render
        };
        ReloadRes();
        Play(this.animType,this.dirType);
    }
    
    void ReloadRes()
    {
        counter = 0;
        frameIndex = 0;

        var typeId = parts[(int)UnitAnimPart.Body].TypeID;
         // 加载各部位动画数据
      

        var path = getPartAnimDataPath(typeId);
        parts[(int)UnitAnimPart.Body].AnimData = Resources.Load<FrameAnimData>(path);
        if (parts[(int)UnitAnimPart.Body].AnimData == null)
        {
            Debug.LogError("Resources.Load fail:" + path);
        }
    }

    public override void Play(UnitAnimType t,UnitDirType d)
    {
        base.Play(t,d);
        var i = (int)UnitAnimPart.Body;
        var typeId = parts[i].TypeID;
        var animName = getPartAnimName(typeId);
        parts[i].Anim = parts[i].AnimData.AnimRes.Infos.Find(x => x.AnimName == animName);
        if (parts[i].Anim == null)
        {
            Debug.LogError("找不到动画：" + animName);
            return;
        }
    }
}

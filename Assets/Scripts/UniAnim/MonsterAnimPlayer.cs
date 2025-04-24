using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MonsterAnimPlayer : UnitAnimPlayer
{
    public static UnitAnimType[] MonsterAnimTypes = { 
        UnitAnimType.Stand,
        UnitAnimType.Walk,
        UnitAnimType.Attack,
        UnitAnimType.Dead,
        UnitAnimType.Hit,
    };
    //翻转方向，动画是左右对称的
    public static Dictionary<UnitDirType,UnitDirType> FlipDirs = new Dictionary<UnitDirType, UnitDirType>
    {
        { UnitDirType.Left, UnitDirType.Right },
        { UnitDirType.UpLeft, UnitDirType.UpRight },
        { UnitDirType.DownLeft, UnitDirType.DownRight }
    };
    public SpriteRenderer Render;
    public int TypeID;
    string getPartAnimName(int typeId,int dir,int animType)
    {
        return $"monster{typeId:D3}_{(int)dir}{AnimNames[(int)animType]}";
    }

    string getPartAnimDataPath(int typeId,UnitAnimType animType,int dir)
    {
        return $"monster/{typeId:D3}/monster{typeId:D3}_{dir}{AnimNames[(int)animType]}";
    }

    void Start()
    {
        parts = new UnitPartInfo[1];
        parts[(int)UnitAnimPart.Body] = new UnitPartInfo();
        parts[(int)UnitAnimPart.Body].TypeID = TypeID;
        parts[(int)UnitAnimPart.Body].Render = Render;
        ReloadRes();
        Play(this.animType,this.dirType);
    }
    
    void ReloadRes()
    {
        counter = 0;
        frameIndex = 0;
        
        var typeId = parts[(int)UnitAnimPart.Body].TypeID;
        var animDataMulti = new Dictionary<int, FrameAnimData>();
        
        try 
        {
            for (int d = 0; d <= (int)UnitDirType.Down; d++)
            {
                foreach (var animType in MonsterAnimTypes)
                {
                    var path = getPartAnimDataPath(typeId, animType, d);
                    var animData = Resources.Load<FrameAnimData>(path);
                    if (animData == null)
                    {
                        Debug.LogError($"Resources.Load fail: {path}");
                        continue;
                    }
                    
                    var key = d * 100 + (int)animType;
                    if (!animDataMulti.ContainsKey(key))
                    {
                        animDataMulti.Add(key, animData);
                    }
                }
            }
            parts[(int)UnitAnimPart.Body].AnimDataMulti = animDataMulti;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"ReloadRes failed: {e.Message}");
        }
    }

    public override void Play(UnitAnimType t,UnitDirType d)
    {
        base.Play(t,d);

        var part = UnitAnimPart.Body;
        //是否翻转
        if (FlipDirs.ContainsKey(d))
        {
            d = FlipDirs[d];
            parts[(int)part].Render.gameObject.transform.localScale = new Vector3(-1,1,1);
        } else {
            parts[(int)part].Render.gameObject.transform.localScale = new Vector3(1,1,1);
        }

        var key = (int)d * 100 + (int)animType;

        var typeId = parts[(int)part].TypeID;
        var animName = getPartAnimName(typeId,(int)d,(int)animType);
        parts[(int)part].Anim = parts[(int)part].AnimDataMulti[key].AnimRes.Infos.Find(x => x.AnimName == animName);
        if (parts[(int)part].Anim == null)
        {
            Debug.LogError("找不到动画：" + animName);
            return;
        }
        
    }


}

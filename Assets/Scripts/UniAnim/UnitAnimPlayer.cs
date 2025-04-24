using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//body004_1_1w name{%2d}_{sex}_{dir}{anim}
public enum UnitDirType
{
    Up = 0,
    UpRight = 1,
    Right = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    Left = 6,
    UpLeft = 7,
    Max
}
public enum Sex
{
    Man = 0,
    Women =1
}

public enum UnitAnimType
{
    Attack = 0,
    Cast = 1,
    Dead = 2,
    Hit = 3,
    Run = 4,
    Stand = 5,
    StandFight = 6,
    Walk = 7
}

public enum UnitAnimPart
{
    Body = 0,
    Hair = 1,
    Weapon = 2,
    Max
}


public class UnitPartInfo
{
    public SpriteRenderer Render;
    public int TypeID;
    public FrameAnimData AnimData;
    public Dictionary<int, FrameAnimData> AnimDataMulti;//key=dir*100+animType
    public FrameAnimInfo Anim;
}
public class UnitAnimPlayer : MonoBehaviour
{
    public static string[] AnimNames = { "a", "c", "d", "h", "r", "s", "s1", "w" };

    public UnitPartInfo[] parts;

    public UnitDirType dirType;
    public UnitAnimType animType;
    public float speed = 1; //播放速度，默认为1

    public bool isPlaying;
    protected int counter;
    protected int frameIndex;

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }
        var anim = parts[(int)UnitAnimPart.Body].Anim;//body动画数据作为主数据
        if (anim == null)
        {
            return;
        }
        counter++;
        // 根据帧率和speed计算是否切换到下一帧
        if (counter >= anim.FrameRate / speed)  // 添加speed影响
        {
            counter = 0;
            frameIndex++;
            // 循环播放处理
            if (frameIndex >= anim.Frames.Length)
            {
                frameIndex = 0;
            }
            // 更新当前显示的精灵帧
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].Render.sprite = parts[i].Anim.Frames[frameIndex].sprite;
            }
        }
    }

    public virtual void Play(UnitAnimType t,UnitDirType dirType)
    {
        this.animType = t;
        this.dirType = dirType;

        isPlaying = true;
        counter = 0;
        frameIndex = 0;
    }

    public virtual void Pause()
    {
        isPlaying = false;

    }
}


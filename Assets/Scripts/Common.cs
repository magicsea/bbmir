using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Common
{

     const float PixcelSize = 0.01f;
     public static Vector3 GamePosToWorldPos(float x, float y) 
     {
        return new Vector3(x*PixcelSize, y*PixcelSize, 0);
     }
     public static Vector2 WorldPosToGamePos(Vector3 pos)
     {
        return new Vector2(pos.x/PixcelSize, pos.y/PixcelSize);
     }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键点击
        {
            // 获取鼠标位置并转换为世界坐标
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane; // 设置合适的Z值
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            // 移动玩家到目标位置(保持原有Z轴不变)
            player.transform.position = new Vector3(
                worldPos.x, 
                worldPos.y, 
                player.transform.position.z
            );
        }
    }
}

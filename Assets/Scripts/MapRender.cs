using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRender : MonoBehaviour
{
    public Vector2Int RenderCellNum = new Vector2Int(5,4); // 渲染的格子数量
    public Vector2Int RenderCellIndexFrom = new Vector2Int(0, 0);//渲染起始格子
    public Vector2Int RenderCellIndexTo = new Vector2Int(4, 3); //渲染结束格子

    public Vector2Int CellNum = new Vector2Int(6, 4);//轴方向上的格子数
    public Vector2 CellSize = new Vector2(256,256);//格子大小
    public string MapPath = "map/0105small";//地图路径

    //地图内拆分图的路径
    public string MapCellPath 
    {
        get{
            return MapPath + "/image";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;
        
        // 计算相机中心偏移(假设相机在(0,0)位置)
        float cameraHalfHeight = mainCamera.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;
        
        for (int x = 0; x < CellNum.x; x++)
        {
            for (int y = 0; y < CellNum.y; y++)
            {
                string spriteName = $"{y}_{x}";
                Sprite sprite = Resources.Load<Sprite>($"{MapCellPath}/{spriteName}");
                
                if (sprite != null)
                {
                    GameObject cellObj = new GameObject(spriteName);
                    cellObj.transform.SetParent(transform);
                    
                    // 计算相对于相机中心的坐标
                    Vector3 pos =  Common.GamePosToWorldPos(
                        x * CellSize.x,  // 水平居中
                        -y * CellSize.y  // 垂直居中
                        );
                    
                    // 直接设置世界坐标
                    cellObj.transform.localPosition = pos;
                    
                    SpriteRenderer renderer = cellObj.AddComponent<SpriteRenderer>();
                    renderer.sprite = sprite;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

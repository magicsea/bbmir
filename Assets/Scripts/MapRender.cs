using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MapRender : MonoBehaviour
{
    public Vector2Int RenderCellNum = new Vector2Int(5,4); // 渲染的格子数量
    //public Vector2Int RenderCellIndexFrom = new Vector2Int(0, 0);//渲染起始格子
    // 移除RenderCellIndexTo定义

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
    private List<GameObject> cellObjects = new List<GameObject>();

    void Start()
    {
        // 预先创建RenderCellNum个空对象
        for (int i = 0; i < RenderCellNum.x * RenderCellNum.y; i++)
        {
            GameObject cellObj = new GameObject($"Cell_{i}");
            cellObj.transform.SetParent(transform);
            cellObj.AddComponent<SpriteRenderer>();
            cellObjects.Add(cellObj);
        }

        // 计算结束位置并加载区域
        // Vector2Int to = new Vector2Int(
        //     RenderCellIndexFrom.x + RenderCellNum.x - 1,
        //     RenderCellIndexFrom.y + RenderCellNum.y - 1
        // );
        // LoadMapRegion(RenderCellIndexFrom, to);


        //初始化地图
        if (Camera.main == null) return;

        // 计算相机当前所在的格子位置
        Vector3 cameraPos = Camera.main.transform.position;
        var gcell = Common.GamePosToWorldPos(CellSize.x, CellSize.y);
        Vector2Int currentCameraCellPos = new Vector2Int(
            Mathf.FloorToInt(cameraPos.x / gcell.x),
            Mathf.FloorToInt(-cameraPos.y / gcell.y)
        );

        // 如果相机移动超过一个格子，重新加载区域
        updateMapRegion(currentCameraCellPos);
    }

    void LoadMapRegion(Vector2Int from, Vector2Int to)
    {
        int index = 0;
        for (int x = from.x; x <= to.x && index < cellObjects.Count; x++)
        {
            for (int y = from.y; y <= to.y && index < cellObjects.Count; y++)
            {
                string spriteName = $"{y}_{x}";
                Sprite sprite = Resources.Load<Sprite>($"{MapCellPath}/{spriteName}");
                
                if (sprite != null)
                {
                    GameObject cellObj = cellObjects[index];
                    cellObj.name = spriteName;
                    
                    // 计算世界坐标
                    Vector3 pos = Common.GamePosToWorldPos(
                        (x+0.5f) * CellSize.x,
                        -(y+0.5f) * CellSize.y
                    );
                    
                    cellObj.transform.localPosition = pos;
                    cellObj.GetComponent<SpriteRenderer>().sprite = sprite;
                }
                index++;
            }
        }
    }
    // Update is called once per frame
    private Vector2Int lastCameraCellPos = Vector2Int.zero;


    void updateMapRegion(Vector2Int currentCameraCellPos) 
    {
        Debug.Log("updateMapRegion:"+currentCameraCellPos);
        lastCameraCellPos = currentCameraCellPos;
            
        // 计算新的渲染范围(以相机所在格子为中心)
        Vector2Int from = new Vector2Int(
                Mathf.Max(0, currentCameraCellPos.x - RenderCellNum.x / 2),
                Mathf.Max(0, currentCameraCellPos.y - RenderCellNum.y / 2)
        );
            
        Vector2Int to = new Vector2Int(
                Mathf.Min(CellNum.x - 1, currentCameraCellPos.x + Mathf.CeilToInt(RenderCellNum.x / 2.0f) - 1),
                Mathf.Min(CellNum.y - 1, currentCameraCellPos.y + Mathf.CeilToInt(RenderCellNum.y / 2.0f) - 1)
        );

        LoadMapRegion(from, to);
    }
    void Update()
    {
        if (Camera.main == null) return;
        
        // 计算相机当前所在的格子位置
        Vector3 cameraPos = Camera.main.transform.position;
        var gcell = Common.GamePosToWorldPos(CellSize.x, CellSize.y);
        Vector2Int currentCameraCellPos = new Vector2Int(
            Mathf.FloorToInt(cameraPos.x / gcell.x),
            Mathf.FloorToInt(-cameraPos.y / gcell.y)
        );

        // 如果相机移动超过一个格子，重新加载区域
        if (currentCameraCellPos != lastCameraCellPos)
        {
            updateMapRegion(currentCameraCellPos);
        }
    }
}

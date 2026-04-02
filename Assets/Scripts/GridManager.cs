using UnityEngine;


public class GridManager
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray; // [,] là cú pháp khai báo mảng 2 chiều
    private Vector3 originalPos;

    public GridManager(int width, int height, float cellSize, Vector3 originalPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originalPos = originalPos;
        gridArray = new int[width, height]; //new: cấp phát bộ nhớ, khởi tạo 1 cái gridArray có chiều rộng width và chiều dài height
    }

    public Vector3 GetWorldPosition(int x, int y) //Truyền vào tọa độ của ô để chuyển ra tọa độ trong unity
    {
        return new Vector3(x,y,0) * cellSize + originalPos;
    }

    public void GetXY(Vector3 worldPos, out int x, out int y) // out bản chất là truyền tham chiếu, nhưng ko cần có giá trị ban đầu
    {
        x = Mathf.FloorToInt((worldPos.x - originalPos.x) / cellSize); // Chuyển từ tọa độ unity về lại tọa độ ô khi người chơi ấn vào
        y = Mathf.FloorToInt((worldPos.y - originalPos.y) / cellSize); // hàm Mathf.FloorToInt: chỉ lấy phần nguyên của số thập phân
    }                                                // Chia lại tọa độ trong unity cho cellsize để trả về tọa độ ô

    public void DrawDebugLines() //Vẽ khung lưới ra để dễ hình dung
    {
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }
}

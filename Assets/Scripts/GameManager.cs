using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GridManager grid;

    private void Start()
    {
        int columns = 6; // Chiều rộng: 6 ô (width)
        int rows = 14;    // Chiều cao: 8 ô (height)

        // ==========================================
        // BƯỚC 1: ĐO KÍCH THƯỚC CAMERA TRONG THẾ GIỚI THỰC
        // ==========================================

        // TẠI SAO PHẢI NHÂN 2? 
        // Trong Unity, thông số 'Camera.main.orthographicSize' bản chất chỉ là khoảng cách từ TÂM màn hình lên MÉP TRÊN.
        // Mày phải nhân đôi (x2) lên thì mới ra được tổng chiều CAO thực tế của cái Camera.
        float screenHeight = Camera.main.orthographicSize * 2f;

        // TẠI SAO LẠI NHÂN VỚI ASPECT? 
        // Camera.main.aspect là tỷ lệ khung hình (Chiều Rộng chia Chiều Cao. Ví dụ màn hình 16:9 thì aspect = 1.77).
        // Toán học lớp 3: Rộng = Cao * Tỷ lệ. Áp vào đây để tìm ra chiều RỘNG thực tế của Camera.
        float screenWidth = screenHeight * Camera.main.aspect;


        // ==========================================
        // BƯỚC 2: TÍNH KÍCH THƯỚC Ô (CELL SIZE) ĐỂ VỪA KHÍT
        // ==========================================

        // Tạo một khoảng lùi (Padding). Nếu để 1.0f thì lưới dính sát rạt mép điện thoại nhìn rất tù.
        // Để 0.9f nghĩa là lưới chỉ chiếm 90% không gian, chừa lại 10% làm viền xung quanh nhìn cho thoáng.
        float padding = 0.9f;

        // Tính xem: Nếu muốn nhét vừa khít chiều RỘNG màn hình, thì 1 ô được phép to tối đa bao nhiêu?
        float maxCellWidth = (screenWidth * padding) / columns;

        // Tính xem: Nếu muốn nhét vừa khít chiều CAO màn hình, thì 1 ô được phép to tối đa bao nhiêu?
        float maxCellHeight = (screenHeight * padding) / rows;

        // TẠI SAO LẠI DÙNG Mathf.Min Ở ĐÂY?
        // Đây là điểm ăn tiền! Điện thoại có loại màn hình siêu dài, có loại màn hình hẹp ngang.
        // Nếu mày lấy cell size to theo chiều dài, mà điện thoại lại bị hẹp ngang, cái lưới sẽ chọc thủng 2 bên mép màn hình.
        // Hàm Mathf.Min sẽ so sánh và CHỌN LẤY SỐ NHỎ HƠN. 
        // Việc chọn kích thước nhỏ nhất đảm bảo 100% cái lưới của mày sẽ chui lọt vào Camera mà không bị tràn viền ở bất cứ trục nào.
        float cellSize = Mathf.Min(maxCellWidth, maxCellHeight);


        // ==========================================
        // BƯỚC 3: CĂN GIỮA (Vẫn giữ nguyên bản chất toán học nãy tao chỉ)
        // ==========================================

        // Tính tổng kích thước thực tế của lưới sau khi đã có được cellSize tự động
        float totalGridWidth = columns * cellSize;
        float totalGridHeight = rows * cellSize;

        // Kéo cái tọa độ Gốc lùi về góc dưới cùng bên trái, lùi đúng bằng 1 nửa kích thước lưới
        Vector3 centerOrigin = new Vector3(-totalGridWidth / 2f, -totalGridHeight / 2f, 0);

        // Bơm toàn bộ thông số đã tính toán hoàn toàn tự động này vào "Bộ não"
        grid = new GridManager(columns, rows, cellSize, centerOrigin);

        grid.DrawDebugLines();
    }

    private void Update()
    {
        // (Giữ nguyên code test chuột cũ nhé)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            int x, y;
            grid.GetXY(mousePos, out x, out y);
            Debug.Log("Mày vừa chọc vào ô: " + x + ", " + y);
        }
    }
}
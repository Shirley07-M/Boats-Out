using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Bắt buộc phải có để dùng lệnh sắp xếp OrderBy

public class QueueManager : MonoBehaviour
{
    [Header("Cài đặt quét khách")]
    public float maxDistance = 10f; // Khoảng cách tối đa thuyền có thể "nhìn" thấy
    public List<Passenger> waitingPassengers = new List<Passenger>(); // Danh sách tất cả khách trên sân
    public LayerMask combinedLayer; // Phải tích chọn cả Layer 'Passenger' và 'Obstacle' (Tường)

    /// <summary>
    /// Hàm này được gọi khi thuyền của Dev A cập bến
    /// </summary>
    /// <param name="boatPosition">Vị trí của thuyền</param> hãy gửi vị trí trên mặt nước một chút (ví dụ transform.position + Vector3.up)
    /// <param name="boatColor">Màu của thuyền</param>
    /// <param name="boatCapacity">Số chỗ trống trên thuyền</param>
    public void OnBoatArrived(Vector3 boatPosition, Color boatColor, int boatCapacity)
    {
        // BƯỚC 1: Lọc ra những người cùng màu và KHÔNG bị vật cản che khuất
        List<Passenger> visiblePassengers = new List<Passenger>();

        foreach (Passenger p in waitingPassengers)
        {
            // Chỉ xét những người cùng màu với thuyền
            if (p != null && p.myColor == boatColor)
            {
                // Tính hướng và khoảng cách từ thuyền đến người đó
                Vector2 direction = p.transform.position - boatPosition;
                float distance = direction.magnitude;

                if (distance > maxDistance)
                {
                    // Bắn tia Laser (Raycast) để kiểm tra tầm nhìn
                    // Tia này sẽ đập vào bất cứ thứ gì thuộc 'combinedLayer' (Người hoặc Tường)
                    RaycastHit2D hit = Physics2D.Raycast(boatPosition, direction.normalized, distance, combinedLayer);

                    // KIỂM TRA: Nếu tia laser chạm trúng chính người đó mà không bị ai chặn giữa đường
                    if (hit.collider != null && hit.collider.gameObject == p.gameObject)
                    {
                        visiblePassengers.Add(p); // Người này "nhìn thấy" thuyền và đủ điều kiện đi

                    }
                }
            }
        }

        // BƯỚC 2: Sắp xếp những người nhìn thấy thuyền theo thứ tự Gần -> Xa
        // OrderBy giúp người có Distance nhỏ nhất đứng đầu danh sách
        var sortedList = visiblePassengers
      .OrderBy(p => Vector3.Distance(p.transform.position, boatPosition))
      .ToList();

        // BƯỚC 3: Cho khách lên thuyền theo thứ tự đã sắp xếp
        int spotsTaken = 0;

        // Vì ta vừa duyệt List vừa xóa phần tử, nên tạo một danh sách phụ để lưu những người sẽ đi
        List<Passenger> passengersToRemove = new List<Passenger>();

        foreach (Passenger p in sortedList)
        {
            if (spotsTaken <= boatCapacity)
            {
                // Gọi hàm di chuyển bên Script Passenger (Dev B tự viết hoặc bảo Dev A viết)
                p.MoveToBoat(boatPosition);

                passengersToRemove.Add(p);
                spotsTaken++;
            }
            else
            {
                break; // Thuyền đầy rồi, không nhận thêm người ở xa nữa
            }
        }

        // BƯỚC 4: Cập nhật lại danh sách tổng (xóa những người đã lên thuyền)
        foreach (Passenger p in passengersToRemove)
        {
            waitingPassengers.Remove(p);
        }
    }

    // Vẽ vòng tròn hỗ trợ nhìn trong lúc Dev (Gizmos)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDistance); // Vẽ mốc vị trí quản lý
    }
}
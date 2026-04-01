using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Color myColor;

    void Start()
    {
        // Tự tìm ông Quản lý trên sân và xin vào danh sách đợi
        QueueManager manager = Object.FindAnyObjectByType<QueueManager>();
        if (manager != null)
        {
            manager.waitingPassengers.Add(this);
        }
    }
    public void MoveToBoat(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
}

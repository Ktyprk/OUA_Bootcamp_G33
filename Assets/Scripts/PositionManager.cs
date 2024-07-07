using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public GameObject firstFloorRight;
    public GameObject firstFloorLeft;
    public GameObject secondFloorRight;
    public GameObject secondFloorLeft;

    private void Start()
    {

        string targetPosition = PlayerPrefs.GetString("TargetPosition", "");

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player objesi bulunamad�. L�tfen sahnede 'Player' tagli bir obje oldu�undan emin olun.");
            return;
        }

        Debug.Log("Target Position: " + targetPosition);

        if (targetPosition == "FirstFloor_Right" && firstFloorRight != null)
        {
            player.transform.position = firstFloorRight.transform.position;
        }
        else if (targetPosition == "FirstFloor_Left" && firstFloorLeft != null)
        {
            player.transform.position = firstFloorLeft.transform.position;
        }
        else if (targetPosition == "SecondFloor_Right" && secondFloorRight != null)
        {
            player.transform.position = secondFloorRight.transform.position;
        }
        else if (targetPosition == "SecondFloor_Left" && secondFloorLeft != null)
        {
            player.transform.position = secondFloorLeft.transform.position;
        }
        else
        {
            Debug.LogWarning("Ge�i� noktas� bulunamad� veya ge�ersiz. L�tfen ge�i� noktalar�n�n do�ru atand���ndan emin olun.");
        }

        

    }
}

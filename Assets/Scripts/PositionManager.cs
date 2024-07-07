using System;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public GameObject firstFloorRight;
    public GameObject firstFloorLeft;
    public GameObject secondFloorRight;
    public GameObject secondFloorLeft;
    
    private GameObject player;

    private void Awake()
    { 
        player = GameObject.FindWithTag("Player");
        DontDestroyOnLoad(player);
    }

    private void Start()
    {

        string targetPosition = PlayerPrefs.GetString("TargetPosition", "");
        

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
            Debug.LogWarning("Geçiş noktası bulunamadı veya geçersiz. Lütfen geçiş noktalarının doğru atandığından emin olun.");
        }
        
    }
    
    
    
    
}

using UnityEngine;
using Cinemachine;

public class ImageToCamera : MonoBehaviour
{
    public Camera targetCamera; 
    public GameObject player;

    void FixedUpdate()
    {
        Vector3 playerPosition = player.transform.position;
        transform.position = targetCamera.WorldToScreenPoint(new Vector3(playerPosition.x, playerPosition.y, playerPosition.z));

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPosShiningUI : MonoBehaviour
{
    private static QuestPosShiningUI instance;
    
    private Image image;

    private bool isShowing;
    
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex;
    private Transform waypoint => waypoints[currentWaypointIndex];
    
    [SerializeField] private Transform character; // Reference to the character's Transform

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        Vector2 position = Camera.main.WorldToScreenPoint(waypoint.position);

        position.x = Mathf.Clamp(position.x, 0, Screen.width);
        position.y = Mathf.Clamp(position.y, 0, Screen.height);

        if (Vector3.Dot((waypoint.position - Camera.main.transform.position), Camera.main.transform.forward) < 0)
        {
            position.x = Mathf.Abs(Screen.width - position.x);
            position.y = Mathf.Abs(Screen.height - position.y);

            if ((position.x != 0 && position.x != Screen.width) && (position.y != 0 && position.y != Screen.height))
            {
                if (position.x < Screen.width / 2)
                {
                    position.x = 0;
                }
                else
                {
                    position.x = Screen.width;
                }
            }
        }

        float distance = Vector3.Distance(character.position, waypoint.position);

        if (distance < 5f)
        {
            image.enabled = false;
        }
    }
}
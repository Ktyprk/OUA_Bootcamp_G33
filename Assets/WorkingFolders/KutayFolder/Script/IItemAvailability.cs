using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemAvailability
{
    int usableItemId { get; } 
    int usableItemCount { get; } 
    public void UseItem();
}

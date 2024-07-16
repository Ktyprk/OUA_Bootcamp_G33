using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Pistol : ItemScript
{
    public override void Fire()
    {
        base.Fire();
        Debug.Log("Firing pistol");
    }
}

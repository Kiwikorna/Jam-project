using System;
using UnityEngine;

public class BorderConroller : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CarDrive carDrive;
    
    private void OnCollisionStay2D(Collision2D collision)
    { 
        var colliderContact = collision.GetContact(0);
        var normal = colliderContact.normal;
        carDrive.SetСCollisionNormal(normal);
    }
    private void OnCollisionExit2D()
    { 
        carDrive.SetСCollisionNormal(Vector2.zero);
    }
}

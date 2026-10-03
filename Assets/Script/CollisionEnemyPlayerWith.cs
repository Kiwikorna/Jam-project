using System;
using UnityEngine;

public class CollisionEnemyPlayerWith : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(other.gameObject);
    }
}

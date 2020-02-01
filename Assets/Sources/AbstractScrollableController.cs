using UnityEngine;
using System.Collections;

public abstract class AbstractScrollableController : MonoBehaviour
{
    // Check if background has reached the left side of the screen
    public virtual bool IsOutOfLeftBoundary()
    {
        return transform.position.x + GetWorldSize() < GameManager.instance.leftCorner.x;
    }

    public abstract float GetWorldSize();
}

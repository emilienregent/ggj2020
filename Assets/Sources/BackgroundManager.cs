using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public List<BackgroundController> backgrounds = new List<BackgroundController>();

    // Initialize backgrounds to have proper position
    private void Start()
    {
        for (int i = 0; i < backgrounds.Count; ++i)
        {
            backgrounds[i].transform.position = new Vector2((i - 2) * backgrounds[i].GetWorldSize(), 0f);

            backgrounds[i].Initialize(this);
        }
    }

    // Move a background to last position and update the list
    public Vector2 SwapToEnd(BackgroundController background)
    {
        Vector2 position = background.transform.position;
        BackgroundController lastBackground = backgrounds[backgrounds.Count - 1];

        backgrounds.Remove(background);
        backgrounds.Add(background);

        position.x = lastBackground.transform.position.x + background.GetWorldSize();

        return position;
    }
}

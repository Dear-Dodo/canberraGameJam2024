using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashCounter : MonoBehaviour
{
    public GameObject DashIcon;

    public List<GameObject> DashIcons = new List<GameObject>();

    public void Redraw(int charges)
    {
        for (int i = DashIcons.Count - 1; i >= 0; i--)
        {
            Destroy(DashIcons[i]);
        }
        DashIcons.Clear();

        for (int i = 0; i < charges; i++)
        {
            DashIcons.Add(Instantiate(DashIcon, transform));
        }
    }
}

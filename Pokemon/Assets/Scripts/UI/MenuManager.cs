using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static void OpenActionMenu(GameObject actionMenu)
    {
        actionMenu.SetActive(true);
    }
    public static void CloseActionMenu(GameObject actionMenu)
    {
        actionMenu.SetActive(false);
    }
}

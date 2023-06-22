using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystemUtils
{
    public static void ControlCursor(PlayerInput input)
    {
        if (!input.currentControlScheme.Equals(GlobalConstants.ControlSchemeKeyboard))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
            }
        }
        else
        {
            if (!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }
    }
}

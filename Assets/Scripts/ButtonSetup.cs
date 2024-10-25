using UnityEngine;
using UnityEngine.UI;

public class ButtonSetup : MonoBehaviour
{
    public Button[] buttons;

    void Start()
    {
        // assign hover and click sounds to all buttons
        foreach (Button button in buttons)
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.AssignButtonSounds(button);
            }
        }
    }
}

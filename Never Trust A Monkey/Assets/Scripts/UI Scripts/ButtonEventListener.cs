using UnityEngine;

public class ButtonEventListener : MonoBehaviour
{
    public UIAudioPlayer player;

    private void OnMouseEnter()
    {
        Debug.Log("Hover!");
        player.playHover();
    }

    private void OnMouseDown()
    {
        player.playClick();
        Debug.Log("Click!");
    }

}

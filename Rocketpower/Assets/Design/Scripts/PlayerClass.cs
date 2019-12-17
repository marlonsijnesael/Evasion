using UnityEngine;
using XInputDotNetPure;

public class PlayerClass : MonoBehaviour
{
    public PlayerIndex playerIndex;
    public VirtualController virtualController;

    private void Awake()
    {
        virtualController.playerIndex = playerIndex;
    }

    private void Update()
    {

    }
}

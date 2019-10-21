using UnityEngine;

public class AnimContrller : MonoBehaviour
{
    [Header("Animator variable names")]
    [SerializeField] private string animIdle = "isIdle";
    [SerializeField] private string animRunning = "isRunnin";
    [SerializeField] private string animWallrunLeft = "wallrun_left";
    [SerializeField] private string animWallrunRight = "wallrun_right";
    [SerializeField] private string animAirtime = "isGrounded";
    [SerializeField] private string animSliding = "sliding";

    public enum animations
    {
        idle = 0, running = 1, wallrun_left = 2, wallrun_right = 3, airtime = 4, jumping = 5, sliding = 6
    }

    public animations animationStates = new animations();


    private string getAnimationName(string name)
    {
        switch (name)
        {
            case "idle":
                return animIdle;
            case "running":
                return animRunning;
            case "wallrun_left":
                return animWallrunLeft;
            case "wallrun_right":
                return animWallrunRight;
            case "airtime":
                return animAirtime;
            case "jumping":
                return animAirtime;
            case "sliding":
                return animSliding;
            default:
                return animIdle;
        }
    }

    public void SetBool(Animator animator, string animation, bool val)
    {
        string name = getAnimationName(animation);
        animator.SetBool(name, val);
    }

    public void SetFloat(Animator animator, string animation, float val)
    {
        string name = getAnimationName(animation);
        animator.SetFloat(name, val);
    }
}

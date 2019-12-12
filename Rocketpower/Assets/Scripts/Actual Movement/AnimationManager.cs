using UnityEngine;
public class AnimationManager : MonoBehaviour
{
    [Header("Animator variable names")]
    [SerializeField] private string animIdle = "isIdle";
    [SerializeField] private string animRunning = "isRunning";
    [SerializeField] private string animWallrunLeft = "wallrun_left";
    [SerializeField] private string animWallrunRight = "wallrun_right";
    [SerializeField] private string animAirtime = "grounded";
    [SerializeField] private string animSliding = "sliding";
    [SerializeField] private string animClimbing = "climbing";

    public enum AnimationStates
    {
        idle = 0, running = 1, wallrun_left = 2, wallrun_right = 3, airtime = 4, jumping = 5, sliding = 6, climbing = 7
    }

    [HideInInspector] public AnimationStates animationStates = new AnimationStates();

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
            case "climbing":
                return animClimbing;
            default:
                return animIdle;
        }
    }

    public void SetBool(Animator animator, string animation, bool val)
    {
        string name = getAnimationName(animation);
        animator.SetBool(name, val);
        //        Debug.Log(name + " " + val);
    }

    public void SetFloat(Animator animator, string animation, float val)
    {
        string name = getAnimationName(animation);
        animator.SetFloat(name, val);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public TestMove MoveSystem;
    public TestPhysx PhysxSystem;

    [SerializeField] private Animator anim;

    private void FixedUpdate()
    {
        MoveSystem.Act();
        PhysxSystem.Act();
    }

    public void SetAnimation(string aVariable, bool val)
    {
        anim.SetBool(aVariable, val);
    }

    public void SetAnimation(string aVariable, float value)
    {
        anim.SetFloat(aVariable, value);
    }

}

using UnityEngine;

public class Idle : PlayerState
{
    public Idle(PlayerController playerController) : base(playerController)
    {
        Enter();
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        Debug.Log("Idle FixedUpdate"+" "+GetStateName());
    }

    public override void Update()
    {
        
    }
}

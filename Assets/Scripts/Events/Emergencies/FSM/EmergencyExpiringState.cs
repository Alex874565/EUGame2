using UnityEngine;

public class EmergencyExpiringState : EmergencyState
{
    public EmergencyExpiringState(EmergencyStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        StateMachine.Emergency.SolvingTimeLeft = Mathf.Min(StateMachine.Emergency.EmergencyData.TimeToSolve, StateMachine.Emergency.SolvingTimeLeft + Time.deltaTime);
        StateMachine.Emergency.ExpirationTimeLeft = Mathf.Max(0, StateMachine.Emergency.ExpirationTimeLeft - Time.deltaTime);
        UpdateUI();
        
        if(StateMachine.Emergency.ExpirationTimeLeft <= 0)
        {
            StateMachine.Emergency.Expire();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }

    public override EmergencyState GetNextState()
    {
        if (Emergency.IsSolving)
        {
            return StateMachine.SolvingState;
        }
        
        return null;
    }
    
    private void UpdateUI()
    {
        Emergency.FillImage.fillAmount = Emergency.ExpirationTimeLeft / Emergency.EmergencyData.TimeUntilExpiry;
        Emergency.TimerText.text = Mathf.Ceil(Emergency.ExpirationTimeLeft).ToString();
    }
}
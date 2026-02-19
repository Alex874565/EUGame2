using UnityEngine;

public class EmergencySolvingState : EmergencyState
{
    public EmergencySolvingState(EmergencyStateMachine stateMachine) : base(stateMachine) { }
    
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
        Emergency.SolvingTimeLeft -= Time.deltaTime;
        Emergency.ExpirationTimeLeft += Mathf.Min(Time.deltaTime, Emergency.EmergencyData.TimeUntilExpiry - Emergency.ExpirationTimeLeft);
        UpdateUI();
    }
    
    public override void Exit()
    {
        base.Exit();
    }
    
    public override EmergencyState GetNextState()
    {
        if (!Emergency.HasAllRequiredUnits())
        {
            return StateMachine.ExpiringState;
        }
        
        return null;
    }
    
    private void UpdateUI()
    {
        Emergency.FillImage.fillAmount = 1 - Emergency.SolvingTimeLeft / Emergency.EmergencyData.TimeToSolve;
        Emergency.TimerText.text = Mathf.Ceil(Emergency.SolvingTimeLeft).ToString();
    }
}
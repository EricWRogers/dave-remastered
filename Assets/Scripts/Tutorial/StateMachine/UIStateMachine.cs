using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPupSystems.StateMachine;
public class UIStateMachine : SimpleStateMachine
{
    public StartState startState;
    public LevelState levelState;
    public OptionState optionState;
    public CreditState creditState;

    //tutorial
    public UITutorialTitle uITutorialTitle;
    public UIDetails uIDetails;
    public LevelDetail levelDetail;
    public BuildingDetail buildingDetail;
    public MovePrompt movePrompt;
    public FireballPrompt fireballPrompt;
    public FireBreathPrompt firebreathPrompt;
    public GrabPrompt grabPrompt;
    public HealthPrompt healthPrompt;
    





    void Start()
    {

        startState.uIStateMachine = this;
        States.Add(startState);
        levelState.uIStateMachine = this;
        States.Add(levelState);
        optionState.uIStateMachine = this;
        States.Add(optionState);
        creditState.uIStateMachine = this;
        States.Add(creditState);

        //tutorial
        uITutorialTitle.uIStateMachine = this;
        States.Add(uITutorialTitle);
        uIDetails.uIStateMachine = this;
        States.Add(uIDetails);
        levelDetail.uIStateMachine = this;
        States.Add(levelDetail);
        buildingDetail.uIStateMachine = this;
        States.Add(buildingDetail);
        movePrompt.uIStateMachine = this;
        States.Add(movePrompt);
        fireballPrompt.uIStateMachine = this;
        States.Add(fireballPrompt);
        firebreathPrompt.uIStateMachine = this;
        States.Add(firebreathPrompt);
        grabPrompt.uIStateMachine = this;
        States.Add(grabPrompt);
        healthPrompt.uIStateMachine = this;
        States.Add(healthPrompt);
        
        

        foreach(SimpleState state in States)
        {
            state.StateMachine = this;
        }

        ChangeState(nameof(startState));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToStart()
    {
        ChangeState(nameof(startState));
    }

    public void ToLevel()
    {
        ChangeState(nameof(levelState));
    }

    public void ToOptionState()
    {
        ChangeState(nameof(optionState));
    }

    public void ToCreditState()
    {
        ChangeState(nameof(creditState));
    }

    //tutorial
    public void ToMain()
    {
        ChangeState(nameof(uITutorialTitle));
    }

    public void ToDetails()
    {
        ChangeState(nameof(uIDetails));
    }

    public void ToLeveldetail()
    {
        ChangeState(nameof(levelDetail));
    }

    public void ToBuildingDetail()
    {
        ChangeState(nameof(buildingDetail));
    }

    public void ToMovePrompt()
    {
        ChangeState(nameof(movePrompt));
    }

    public void ToFireBall()
    {
        ChangeState(nameof(fireballPrompt));
    }

    public void ToFireBreath()
    {
        ChangeState(nameof(firebreathPrompt));
    }

    public void ToGrabPrompt()
    {
        ChangeState(nameof(grabPrompt));
    }

    public void ToHealthPrompt()
    {
        ChangeState(nameof(healthPrompt));
    }
}

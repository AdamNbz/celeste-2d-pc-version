using GameState;
using GameStateEvent;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
[System.Serializable]
public class InputManager : IConfigModule,IFixedUpdateModule
{
    const string endsaveDirectory="InputManager";
    List<NameActionMapping> actionlist=new List<NameActionMapping>();
    bool isInputEnable = true;
    bool capturingKey = false;
    public void AwakeModule()
    {
        LoadModuleConfig();
        var eventBus = GameManagerRefactor.Instance.GetModule<EventBus>();
        eventBus.Subscribe<UnpauseGameEvent>(OnGameResume);
        eventBus.Subscribe<PauseGameEvent>(OnGamePaused);
    }

    public void StartListeningForKey(string actionName)
    {
        if (capturingKey) return;
        capturingKey = true;
        var gameStateHolder = GameManagerRefactor.Instance.GetModule<GameStateHolderModule>();
        if (gameStateHolder.GetGameState().GetType() != typeof(Paused))
        {
            gameStateHolder.ChangeStateRequest(new Paused());
        }
        ListeningToKey(actionName);
    }

    public void ListeningToKey(string actionName)
    {
        InputSystem.onAnyButtonPress.Where(control =>
        !control.path.StartsWith("<Mouse>") &&
        !control.path.StartsWith("<Pointer>")
        )
        .CallOnce(control =>
        {
            if (control.path == "<Keyboard>/escape")
            {
                capturingKey = false;
                return;
            }
            AddKey(actionName, control);
            capturingKey = false;
        });
    }

    public InputAction GetGamePlayAction(string name)
    {
        if (!isInputEnable)
        {
            Debug.Log("GameIsPausingSoIgnoreActionReturn");
            return null;
        }
        return actionlist.Find(x => x.actionName == name)?.inputAction;
    }

    public void AddAction(string name, InputAction action)
    {
        if (action.type !=InputActionType.Button)
        {
            Debug.LogWarning($"Only Button type actions are supported. Action is of type {action.type}.");
            return;
        }
        if (actionlist.Exists(x => x.actionName == name))
        {
            Debug.LogWarning($"Action with name {name} already exists.");
            return;
        }
        actionlist.Add(new NameActionMapping { actionName = name, inputAction = action });
    }

    public void AddKey(string name,InputControl input)
    {

        // Implementation for adding a key binding can be added here.
        var action = actionlist.Find(x => x.actionName == name)?.inputAction;
        
        if (action!=null)
        {
            foreach (var binding in action.bindings)
            {
                if (binding.effectivePath == input.path)
                    return;
            }
            action.AddBinding(input.path);
        }
        else
        {
            Debug.Log("Action not found {name}");
        }
    }

    public void SaveModuleConfig()
    {
        throw new System.NotImplementedException();
    }

    public void LoadModuleConfig()
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdateModule()
    {
        if (capturingKey)
        {
            return;
        }
        if (isInputEnable==false)
        {
            return;
        }
    }

    private void EnableInput()
    {
        isInputEnable = true;
    }

    private void DisableInput()
    {
        isInputEnable = false;
    }

    private void OnGamePaused(PauseGameEvent e)
    {
        DisableInput();
    }

    private void OnGameResume(UnpauseGameEvent e)
    {
        EnableInput();
    }

    public void ResetToDefault()
    {
        actionlist.Clear();
        var referlist= GameManagerRefactor.Instance.GetInputDefault().GetActionList;
        foreach (var mapping in referlist)
        {
            actionlist.Add(new NameActionMapping
            {
                actionName = mapping.actionName,
                inputAction = mapping.inputAction.Clone()
            });
        }
    }
}

[System.Serializable]
public class NameActionMapping
{
    public string actionName;
    public InputAction inputAction;
}

[CreateAssetMenu(
    fileName = "InputManagerDefaultSetting",
    menuName = "Input/Input Manager Default Setting"
)]
public class InputManagerDefaultSetting:ScriptableObject
{
    [SerializeField]
    List<NameActionMapping> actionlist;

    public List<NameActionMapping> GetActionList
    {
        get
        {
            return actionlist;
        }
    }
}

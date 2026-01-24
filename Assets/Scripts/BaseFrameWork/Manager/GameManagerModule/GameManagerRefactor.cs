using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManagerRefactor : MonoBehaviour
{
    private List<IModule> modules = new List<IModule>();
    private static GameManagerRefactor instance;
    [SerializeField] InputManagerDefaultSetting inputDefaultSetting;
    private GameManagerRefactor() { }

    private List<Type> moduleTypeList = new List<Type>()
    {
        typeof(EventBus),
        typeof(GameStateHolderModule),
        // Add other module types here
    };

    public InputManagerDefaultSetting GetInputDefault()
    {
        return inputDefaultSetting;
    }

    public static GameManagerRefactor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GameManagerRefactor>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManagerRefactor>();
                    singletonObject.name = typeof(GameManagerRefactor).ToString();
                    DontDestroyOnLoad(singletonObject);
                }
                else
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    public T GetModule<T>() where T : IModule
    {
        foreach (var module in modules)
        {
            if (module is T)
            {
                return (T)module;
            }
        }
        Debug.Log($"Module of type {typeof(T)} not found.");
        return default;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        //All Module add in here
        AwakeAllModule();
    }

    private bool IsValidModuleList()
    {
        foreach (var module in modules)
        {
            if (module == null)
            {
                return false;
            }
            if (!moduleTypeList.Contains(module.GetType()))
            {
                return false;
            }
        }
        foreach (var type in moduleTypeList)
        {
            int count = 0;
            foreach (var module in modules)
            {
                if (module.GetType() == type)
                {
                    count++;
                }
            }
            if (count != 1)
            {
                return false;
            }
        }
        return true;
    }

    private void AwakeAllModule()
    {
        if (!IsValidModuleList())
        {
            ReBuildModuleList();
        }
        foreach (var module in modules)
        {
            module.AwakeModule();
        }
    }

    private void ReBuildModuleList()
    {
        modules.Clear();
        foreach (var moduleType in moduleTypeList)
        {
            if (!typeof(IModule).IsAssignableFrom(moduleType))
            {
                throw new Exception($"{moduleType.Name} does not implement IModule");
            }

            if (moduleType.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new Exception($"{moduleType.Name} must have a parameterless constructor");
            }

            IModule moduleInstance = (IModule)Activator.CreateInstance(moduleType);
            modules.Add(moduleInstance);
        }
    }

    private void Update()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i] is IUpdateModule updateModule)
            {
                updateModule.UpdateModule();
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i] is IFixedUpdateModule fixedUpdateModule)
            {
                fixedUpdateModule.FixedUpdateModule();
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (modules[i] is ILateUpdateModule lateUpdateModule)
            {
                lateUpdateModule.LateUpdateModule();
            }
        }
    }
}
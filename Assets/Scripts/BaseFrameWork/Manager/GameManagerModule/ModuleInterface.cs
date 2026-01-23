using UnityEngine;

public interface IModule
{
    public void AwakeModule();
}

public interface IConfigModule : IModule
{
    public void SaveModuleConfig();

    public void LoadModuleConfig();
}

public interface ILateUpdateModule : IModule
{
    public void LateUpdateModule();
}

public interface IFixedUpdateModule : IModule
{
    public void FixedUpdateModule();
}

public interface IUpdateModule : IModule
{
    public void UpdateModule();
}
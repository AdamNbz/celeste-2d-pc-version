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
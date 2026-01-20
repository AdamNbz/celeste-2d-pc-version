using UnityEngine;

public interface IModule
{
    public void InitModule();
}

public interface IConfigModule : IModule
{
    public void SaveModuleConfig();

    public void LoadModuleConfig();
}
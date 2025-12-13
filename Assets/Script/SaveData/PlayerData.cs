using UnityEditor;
using UnityEngine;
using System.Collections;
namespace Assets.Script.SaveData
{
    [System.Serializable]
    public class PlayerData
    {
        [Header("Position")]
        [SerializeField]string currentcheckpoint="";
        [SerializeField]string currentstage="";

        [Header("Progress")]
        [SerializeField]string SaveId;
        [SerializeField]Skill skills=new Skill();

        public void SetCheckpoint(string checkpoint)
        {
            currentcheckpoint = checkpoint;
        }

        public void SetStage(string stage)
        {
            currentstage = stage;
        }

        public void SetSaveId(string id)
        {
            SaveId = id;
        }
    }
    [System.Serializable]
    public class  Skill
    {
        [SerializeField] bool CanDoubleJump;
        [SerializeField] bool CanDash;
        [SerializeField] bool CanWallClimb;

        public void UnlockDoubleJump()
        {
            CanDoubleJump = true;
        }

        public void UnlockDash()
        {
            CanDash = true;
        }

        public void UnlockWallClimb()
        {
            CanWallClimb = true;
        }

        public bool GetDoubleJumpStatus()
        {
            return CanDoubleJump;
        }

        public bool GetDashStatus()
        {
            return CanDash;
        }

        public bool GetWallClimbStatus()
        {
            return CanWallClimb;
        }
    }
}
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
namespace Player_State
{
    public class Death : Player_State.PlayerState
    {
        public Death(PlayerController playerController) : base(playerController)
        {
        }
        public override void Enter()
        { 
            playerController.DisableInput();
            playerController.StartCoroutine(PlayDeathAnimation());
        }
        
        private IEnumerator PlayDeathAnimation()
        {
            Animator animator = playerController.GetAnimator();
            animator.Play("PlayerDeath");
            
            yield return null;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
            
            playerController.gameObject.SetActive(false);
            GameManager.GetInstance().SpawnDeathEffect(playerController.transform.position);
        }
        
        public override void Exit()
        {
        }
        public override void FixedUpdate()
        {
        }
        public override void Update()
        {
        }
    }
}
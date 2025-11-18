Guide for the state machine framework:
1. Overview
-This framework provides a structured way to manage states and transitions in your application. 
It is designed to be flexible and extensible, allowing you to define custom states and behaviors.
2. Key Components
-Controller: The main class that manages the state machine. It handles state transitions and updates and holds references to 
the animator and state instances.
-State: An abstract base class that defines the structure for all states. Each state must implement methods for entering,
exiting, updating, and fixed updating.(for player is PlayerState, for enemy is EnermyState)
-Concrete States: Specific implementations of the State class that define the behavior for each state.
-namespace: Organizes the code into logical groups, making it easier to manage and understand. Each character or entity can have its own namespace.
3. Usage
-To use the framework, inherit from the state.
-Controller class current have two type:
 a. EnemyStateController: For managing enemy states.
 b. PlayerStateController: For managing player states.
 -Define your custom states by inheriting from the EnermyState or PlayerState class and implementing the required methods.
 -All states must be placed within the appropriate namespace for better organization.
 -Input hadle can be managed through the Controller or the parent state class.
- Controller can access in states through protected variable "controller".
- Controller can access animator through protected variable "animator".
4. Example
-See the provided example states and controllers for reference on how to implement your own states.
using UnityEngine;

namespace Player_State
{
    public class Idle : PlayerState
    {
        public Idle(PlayerController playerController) : base(playerController)
        {
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {
            Debug.Log("Idle FixedUpdate" + " " + GetStateName());
        }

        public override void Update()
        {

        }
    }
}
*Important:
  - the higher priority state must be handle the last for the animator and state set and cannot be override
Guide for Object Pooling Framework:
1. Overview
-This framework provides a way to manage object pooling in your application.
 It helps to optimize performance by reusing objects instead of creating and destroying them frequently.
 2. Key Components
 -PoolManager: The main class that manages the object pools. It handles the creation, retrieval, and recycling of pooled objects.
 -PooledObject: A base class that defines the structure for all pooled objects. Each pooled object must implement methods for
 initializing and resetting the object.
 -Concrete Pooled Objects: Specific implementations of the PooledObject class that define the behavior for each type of pooled object.
 3. Usage
 -To use the framework just call PoolManager.Instance.GetObject("ObjectName") to get an object from the pool.
 -If the pool does not have an available object, it will create a new one.
 -When you are done with the object, call PoolManager.Instance.ReturnObject(pooledObject) to return it to the pool.
 -If return object is null or not managed by PoolManager, it will be destroyed.
 4. Example
 -See the provided example for creating and using pooled objects.
 using UnityEngine;
 public class Bullet : PooledObject
 {
     public override void Initialize()
     {
         // Initialize bullet properties
         ObjectPool.GetInstance().GetObjectFromPool(this,new Vector2(1,2),Quarternion.Identity);
     }
     public override void ResetObject()
     {
         // Reset bullet properties
     }
     private void OnCollisionEnter(Collision collision)
     {
         // Handle collision
         ObjectPool.GetInstance().ReturnObject(this);
     }
 }
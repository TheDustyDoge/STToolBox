using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actors.Controllers;

namespace Actors
{
    public abstract class Actor : MonoBehaviour
    {
        public AnimationControllerBase animationController;
        public BehaviourControllerBase behaviourController;
        public MovementControllerBase movementController;

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
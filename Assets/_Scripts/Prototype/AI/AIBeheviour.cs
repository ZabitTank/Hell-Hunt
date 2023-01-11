using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public abstract class AIBeheviour : MonoBehaviour
{
    public abstract void PerformAction(CharacterController characterController, AIDetector detector);
}
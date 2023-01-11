using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
public abstract class AIBeheviour : MonoBehaviour
{
    private BaseEnemyAI parent;
    public BaseEnemyAI Parent
    {
        get
        {
            return parent;
        }
        set
        {
            parent = value;
        }
    }
    public abstract void PerformAction(CharacterController characterController, AIDetector detector);
}
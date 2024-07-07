using System;
using UnityEngine;

namespace Boss.States.Weave
{
    [RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
    public class WeaveLineBase : MonoBehaviour
    {
        [SerializeField]
        protected LineWobbler Wobbler;

        protected virtual void Awake()
        {
            Wobbler.Initialise(GetComponent<LineRenderer>(), GetComponent<EdgeCollider2D>());
        }

        protected virtual void Update()
        {
            Wobbler.UpdateLine();
        }
    }
}
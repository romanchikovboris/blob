using System;
using System.Collections;
using Obi;
using UnityEditor;
using UnityEngine;

namespace BlobGame
{
    public class Test : MonoBehaviour
    {
        [SerializeField]
        private float _scaleFactor = 3f;
        
        [SerializeField]
        private ObiSolver _solver;

        [SerializeField]
        private ObiSoftbody _ball;

        private void FixedUpdate()
        {
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                ScaleTo(_scaleFactor);
            }
            
            if (Input.GetKeyDown(KeyCode.V))
            {
                ScaleTo(1);
            }
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(SmoothScale(1));
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(SmoothScale(_scaleFactor));
            }
        }

        private IEnumerator SmoothScale(float targetScale)
        {
            var startScale = _solver.transform.localScale.x;
            
            var duration  = 1f;
            var timer = 0f;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                var t = timer / duration;

                var scaleFactor = Mathf.Lerp(startScale, targetScale, t);
                ScaleTo(scaleFactor);
                
                yield return null;
            }
        }

        [ContextMenu("TEST")]
        public void ScaleTo(float factor)
        {
            float oldScale = _solver.transform.localScale.x;
            var oldPos = _ball.transform.position;
            var oldRotation = _ball.transform.rotation;
            
            var originalVelocities = new Vector4[_ball.solverIndices.Length];
            
            // Store the original velocities
            for (int i = 0; i < _ball.solverIndices.Length; ++i){
                int solverIndex = _ball.solverIndices[i];
                originalVelocities[i] = _ball.solver.velocities[solverIndex];
            }

            //_solver.transform.position = oldPos;
            _solver.transform.localScale = Vector3.one * factor;

            var vertOffset = Vector3.up * (factor - oldScale);
            _ball.Teleport(oldPos + vertOffset, oldRotation);
            
            // Reset the velocities
            for (int i = 0; i < _ball.solverIndices.Length; ++i){
                int solverIndex = _ball.solverIndices[i];
                _ball.solver.velocities[solverIndex] = originalVelocities[i];
            }
        }
    }
}
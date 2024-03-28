using System.Collections;
using Obi;
using UnityEditor;
using UnityEngine;

namespace BlobGame
{
    public class VolumeMerger : MonoBehaviour
    {
        [SerializeField]
        private ObiSolver _obiSolver;

        [SerializeField]
        private ObiSoftbody _softbody;

        private void Start()
        {
            //softbody mass is calculated with 1 kilo per particle. Reset it to 1 overall
            _softbody.SetMass(1f);
            _softbody.UpdateParticleProperties();
        }

        public void MergeWithVolume(float mergingVolume)
        {
            var myVolume = _obiSolver.transform.localScale.x;
            var targetVolume = myVolume + mergingVolume;
            
            StartCoroutine(ScaleTo(targetVolume));

            //EditorApplication.isPaused = true;
        }
        
        //we are changing scale of solver(parent) to scale the object. 
        //no other way with softBody
        private IEnumerator ScaleTo(float factor)
        {
            yield return new WaitForFixedUpdate();
            
            float oldScale = _obiSolver.transform.localScale.x;
            var oldPos = _softbody.transform.position;
            var oldRotation = _softbody.transform.rotation;
            
            var originalVelocities = new Vector4[_softbody.solverIndices.Length];
            
            // Store the original velocities
            for (int i = 0; i < _softbody.solverIndices.Length; ++i){
                int solverIndex = _softbody.solverIndices[i];
                originalVelocities[i] = _softbody.solver.velocities[solverIndex];
            }
            
            _obiSolver.transform.position = oldPos;
            _obiSolver.transform.localScale = Vector3.one * factor;

            //var vertOffset = Vector3.up * (factor - oldScale);
            _softbody.Teleport(oldPos, oldRotation);
            
            _obiSolver.UpdateBackend();
            
            // Reset the velocities
            for (int i = 0; i < _softbody.solverIndices.Length; ++i){
                int solverIndex = _softbody.solverIndices[i];
                _softbody.solver.velocities[solverIndex] = originalVelocities[i];
            }
            
            _softbody.SetMass(factor);
            _softbody.UpdateParticleProperties();
        }
    }
}
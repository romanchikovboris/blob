using System;
using UnityEngine;

namespace BlobGame
{
    public class Test2 : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<Test2>(out var component))
            {
                Debug.Log("Collides!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<Test2>(out var component))
            {
                Debug.Log("Triggers!");
            }
        }
    }
}
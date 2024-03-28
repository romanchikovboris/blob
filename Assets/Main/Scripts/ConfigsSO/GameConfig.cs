using UnityEngine;

namespace BlobGame
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Ball movement")]
        public float Acceleration = 7f;
        public float AirControl = 0.2f;

        [Header("Merge")]
        public float MergeAnimationYOffset = 1f;
        public float MergeAnimationTime = 0.5f;
    }
}
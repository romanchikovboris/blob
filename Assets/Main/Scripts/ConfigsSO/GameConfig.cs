using UnityEngine;

namespace BlobGame
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Ball movement")]
        public float Acceleration = 7f;

        public float AirControl = 0.2f;

        public float JumpPower = 5f;
    }
}
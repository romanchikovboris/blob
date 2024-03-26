using UnityEngine;

namespace BlobGame
{
    public class MobileInputService : InputServiceBase
    {
        public override Vector2 Axis => SimpleInputAxis();
    }
}
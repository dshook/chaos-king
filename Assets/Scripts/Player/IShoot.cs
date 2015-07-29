using UnityEngine.Networking;

namespace Player
{
    public interface IShoot
    {
        /// <summary>
        /// Called each time the player _wants_ to shoot
        /// </summary>
        void Shoot();

        void Enable(PlayerShooting ps);
    }
}

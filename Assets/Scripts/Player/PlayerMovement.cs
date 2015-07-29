using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float speed = 6f;

        Vector3 movement;
        Animator anim;
        Rigidbody playerRigidbody;
        Quaternion oldRotation = Quaternion.identity;
        int floorMask;
        float camRayLength = 100f;

        void Awake()
        {
            floorMask = LayerMask.GetMask ("Floor");
            anim = GetComponent<Animator> ();
            playerRigidbody = GetComponent<Rigidbody> ();
        }

        [ClientCallback]
        void Update()
        {
            if (!isLocalPlayer) return;

            var h = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
            var v = CrossPlatformInputManager.GetAxisRaw ("Vertical");

            if (h != 0f || v != 0f)
            {
                CmdMove(h, v);
            }
            Turning ();
            Animating(h, v);
        }

        [Command]
        void CmdMove(float h, float v)
        {
            movement.Set (h, 0f, v);
            movement = movement.normalized * speed * Time.deltaTime;

            playerRigidbody.MovePosition (transform.position + movement);
        }

        void Turning()
        {
            Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

            RaycastHit floorHit;
            if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
                Vector3 playerToMouse = floorHit.point - transform.position;
                playerToMouse.y = 0f;

                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                if (newRotation != oldRotation)
                {
                    CmdTurning(newRotation);
                }
                oldRotation = newRotation;
            }
        }

        [Command]
        void CmdTurning(Quaternion newRotation)
        {
            playerRigidbody.MoveRotation(newRotation);
        }

        void Animating(float h, float v){
            bool walking = h != 0f || v != 0f;
            anim.SetBool ("IsWalking", walking);
        }
    }
}

using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using Util;

namespace Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        public float startingSpeed = 6f;
        public float speed = 6f;

        [SyncVar]
        private bool isMoving = false;

        Vector3 movement;
        Animator anim;
        Rigidbody playerRigidbody;
        Quaternion oldRotation = Quaternion.identity;
        int floorMask;
        float camRayLength = 100f;

        float h = 0f;
        float v = 0f;
        float lastH = 0f;
        float lastV = 0f;

        void Awake()
        {
            floorMask = LayerMask.GetMask ("Floor");
            anim = GetComponent<Animator> ();
            playerRigidbody = GetComponent<Rigidbody> ();
        }

        [ClientCallback]
        void Update()
        {
            Animating();
            if (!isLocalPlayer) return;

            var h = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
            var v = CrossPlatformInputManager.GetAxisRaw ("Vertical");

            if (h != 0f || v != 0f)
            {
                if (!FloatUtils.CloseEnough(lastH, h) || !FloatUtils.CloseEnough(lastV, v))
                {
                    CmdMove(h, v);
                }
            }
            else
            {
                if (!FloatUtils.CloseEnough(lastH, h) || !FloatUtils.CloseEnough(lastV, v))
                {
                    CmdStopMoving();
                }
            }
            lastH = h;
            lastV = v;

            Turning ();
        }

        void FixedUpdate()
        {
            if (!isServer) return;

            if (isMoving)
            {
                movement.Set(h, 0f, v);
                movement = movement.normalized * speed * Time.deltaTime;
                playerRigidbody.MovePosition(transform.position + movement);
            }
        }

        [Command]
        void CmdStopMoving()
        {
            isMoving = false;
            h = 0;
            v = 0;
        }

        [Command]
        void CmdMove(float h, float v)
        {
            isMoving = true;
            this.h = h;
            this.v = v;

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

        void Animating(){
            anim.SetBool ("IsWalking", isMoving);
        }

        public void ResetSpeed()
        {
            speed = startingSpeed;
        }
    }
}

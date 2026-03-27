using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    public class PadlockScript : GrabbableObject
    {
        DoorLock viewedDoorLock;
        AudioSource lockSound;

        public void Awake()
        {
            AudioSource[] Sounds = this.transform.Find("PadlockSounds").gameObject.GetComponents<AudioSource>();
            lockSound = Sounds[0];
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (Physics.Raycast(new Ray(this.playerHeldBy.gameplayCamera.transform.position, this.playerHeldBy.gameplayCamera.transform.forward), out RaycastHit raycastHit, 3f, LayerMask.GetMask("InteractableObject")))
            {
                viewedDoorLock = raycastHit.transform.GetComponent<DoorLock>();
                if (viewedDoorLock == null)
                {
                    TriggerPointToDoor component = raycastHit.transform.GetComponent<TriggerPointToDoor>();
                    if (component != null)
                    {
                        viewedDoorLock = component.pointToDoor;
                    }
                }

                if (viewedDoorLock != null && !viewedDoorLock.isLocked)
                {
                    print("US - US_Padlock locking door!");
                    UsePadlockServerRpc();
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void UsePadlockServerRpc()
        {
            UsePadlockClientRpc();
        }
        [ClientRpc]
        public void UsePadlockClientRpc()
        {
            UsePadlock();
        }
        public void UsePadlock()
        {
            if (Physics.Raycast(new Ray(this.playerHeldBy.gameplayCamera.transform.position, this.playerHeldBy.gameplayCamera.transform.forward), out RaycastHit raycastHit, 3f, LayerMask.GetMask("InteractableObject")))
            {
                viewedDoorLock = raycastHit.transform.GetComponent<DoorLock>();
                if (viewedDoorLock == null)
                {
                    TriggerPointToDoor component = raycastHit.transform.GetComponent<TriggerPointToDoor>();
                    if (component != null)
                    {
                        viewedDoorLock = component.pointToDoor;
                    }
                }
                viewedDoorLock.LockDoor(30f);
                AudioSource.PlayClipAtPoint(lockSound.clip, viewedDoorLock.transform.position);
                if (this.radarIcon != null)
                {
                    UnityEngine.Object.Destroy(this.radarIcon.gameObject);
                }
                playerHeldBy.DespawnHeldObject();
            }
        }
    }
}


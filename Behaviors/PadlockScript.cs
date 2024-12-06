using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class PadlockScript : GrabbableObject
    {
        DoorLock viewedDoorLock;
        AudioSource audioSource;

        public void Awake()
        {
            GameObject audioGameObject = this.transform.Find("LockingSound").gameObject;
            audioSource = audioGameObject.GetComponent<AudioSource>();
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (Physics.Raycast(new Ray(this.playerHeldBy.gameplayCamera.transform.position, this.playerHeldBy.gameplayCamera.transform.forward), out RaycastHit raycastHit, 3f, LayerMask.GetMask("InteractableObject")))
            {
                viewedDoorLock = raycastHit.transform.GetComponent<DoorLock>();

                if (viewedDoorLock != null && !viewedDoorLock.isLocked)
                {
                    print("locking door");
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
                viewedDoorLock.LockDoor(30f);
                audioSource.PlayOneShot(audioSource.clip);
                if (this.radarIcon != null)
                {
                    UnityEngine.Object.Destroy(this.radarIcon.gameObject);
                }
                playerHeldBy.DespawnHeldObject();
            }
        }
    }
}

using UnityEngine;
using UsualScrap.Behaviors.OvertimeEffects;
using Unity.Netcode;

namespace UsualScrap.Behaviors
{
    public class EmergencyInjectorScript : GrabbableObject
    {
        AudioSource audioSource;
        GameObject InjectorContents;
        bool hasBeenUsed = false;

        public void Awake()
        {
            GameObject audioChild = this.transform.Find("InjectSound").gameObject;
            audioSource = audioChild.GetComponent<AudioSource>();
            InjectorContents = this.transform.Find("EmergencyInjectorModel").gameObject.transform.Find("InjectorContents").gameObject;
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown && hasBeenUsed == false)
            {
                if (playerHeldBy.gameObject.GetComponent<EmergencyInjectorEffect>() != null)
                {
                    playerHeldBy.gameObject.AddComponent<EmergencyInjectorOverdoseEffect>();
                }
                else
                {
                    playerHeldBy.gameObject.AddComponent<EmergencyInjectorEffect>();
                }
                UseUpInjectorServerRpc();
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void UseUpInjectorServerRpc()
        {
            UseUpInjectorClientRpc();
        }
        [ClientRpc]
        public void UseUpInjectorClientRpc()
        {
            InjectorContents.SetActive(false);
            AudioSource.PlayClipAtPoint(audioSource.clip, playerHeldBy.transform.position);
            hasBeenUsed = true;
        }
    }
}


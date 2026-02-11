using UnityEngine;
using Unity.Netcode;
using UsualScrap.Behaviors.Effects;

namespace UsualScrap.Behaviors
{
    public class ProductivityAutoinjectorScript : GrabbableObject
    {
        AudioSource audioSource;
        MeshRenderer InjectorDisplay;
        public NetworkVariable<int> usedUp = new NetworkVariable<int>(2, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public void Awake()
        {
            GameObject audioChild = this.transform.Find("InjectSound").gameObject;
            audioSource = audioChild.GetComponent<AudioSource>();
            GameObject g = this.transform.Find("ProductivityAutoinjectorModel").gameObject.transform.Find("InjectorDisplay").gameObject;
            InjectorDisplay = g.GetComponent<MeshRenderer>();
        }
        public override int GetItemDataToSave()
        {
            base.GetItemDataToSave();
            return usedUp.Value;
        }

        public override void LoadItemSaveData(int saveData)
        {
            base.LoadItemSaveData(saveData);
            usedUp.Value = saveData;
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (itemUsedUp)
            {
                UseUpInjectorServerRpc(true);
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown && itemUsedUp == false)
            {
                if (playerHeldBy.gameObject.GetComponent<ProductivityAutoinjectorEffect>() != null)
                {
                    return;
                }
                else
                {
                    playerHeldBy.gameObject.AddComponent<ProductivityAutoinjectorEffect>();
                }
                UseUpInjectorServerRpc(true);
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void UseUpInjectorServerRpc(bool Used)
        {
            UseUpInjectorClientRpc(Used);
        }
        [ClientRpc]
        public void UseUpInjectorClientRpc(bool Used)
        {
            itemUsedUp = true;
            InjectorDisplay.material.SetColor("_EmissiveColor", Color.red);
            AudioSource.PlayClipAtPoint(audioSource.clip, playerHeldBy.transform.position);
        }
    }
}


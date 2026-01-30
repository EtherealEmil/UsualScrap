using Unity.Netcode;
using UnityEngine;
using UsualScrap.Behaviors.Effects;

namespace UsualScrap.Behaviors
{
    internal class PieceofCandyScript : GrabbableObject
    {
        //int randomEffect = new System.Random().Next(1, 4);
        MeshRenderer candyMesh;
        GameObject candyModel;
        MeshRenderer wrapperObject;
        MeshRenderer candyObject;
        Color setColor;

        public override void Start()
        {
            base.Start();
            SetColorServerRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SetColorServerRpc()
        {
            byte ColorRRoll = (byte)new System.Random().Next(1, 256);
            byte ColorGRoll = (byte)new System.Random().Next(1, 256);
            byte ColorBRoll = (byte)new System.Random().Next(1, 256);


            setColor = new Color32(ColorRRoll, ColorGRoll, ColorBRoll, 255);

            SetColorClientRpc(setColor);
        }
        [ClientRpc]
        public void SetColorClientRpc(Color color)
        {
            candyModel = this.transform.Find("PieceofCandyModel").gameObject;
            wrapperObject = candyModel.gameObject.transform.Find("Wrapper").gameObject.GetComponent<MeshRenderer>();
            wrapperObject.material.color = color;
        }
        
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown)
            {
                playerHeldBy.gameObject.AddComponent<RandomCandyEffect>();
                this.playerHeldBy.DespawnHeldObject();
            }
        }
        [ServerRpc]
        public void DestroyRadarIconServerRpc()
        {
            DestroyRadarIconClientRpc();
        }
        [ClientRpc]
        public void DestroyRadarIconClientRpc()
        {
            Destroy(this.radarIcon.gameObject);
        }
    }
}

using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.NetworkScript;
using _Scripts.Interfaces;
using _Scripts.TrapSystem;

namespace _Scripts.GameplayFeatures.Traps
{
    public class TrapClass1 : NetworkAnimatedObject, IDetectable
    {
        #region Variables/Properties
        [FoldoutGroup("Trap properties")]
        [SerializeField] protected Material trapMaterial;
        [FoldoutGroup("Trap references")]
        [SerializeField] private string defaultLayer = "Default";
        [FoldoutGroup("Trap references")]
        [SerializeField] private string topLayer = "RenderedOnTop";
        [FoldoutGroup("Trap references")]
        [SerializeField] protected GameObject[] trapParts;

        protected Material _sharedMaterial;
        protected AudioSource _audioSource;

        public Tile[] OccupedTiles { get; set; }
        #endregion

        #region Builts_In
        protected virtual void Awake()
        {
            InitalizeMaterial();
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            InitializeTrap();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            ChangeTrapPartsLayer(defaultLayer);

            if (!ViewIsMine())
                return;

            ResetOccupedTiles();
        }
        #endregion

        #region Detection Interaction
        public void GetDetected(float duration)
        {
            if (trapParts.Length <= 0)
                return;

            StartCoroutine(DetectionRoutine(duration));
        }

        public void GetDetected()
        {
            if (trapParts.Length <= 0)
                return;

            StartCoroutine(DetectionRoutine(5f));
        }

        /// <summary>
        /// Change the layer on each part of the trap
        /// </summary>
        /// <param name="layer"> Layer name </param>
        protected void ChangeTrapPartsLayer(string layer)
        {
            foreach (GameObject item in trapParts)
            {
                if (!item)
                    continue;

                item.layer = LayerMask.NameToLayer(layer);
            }
        }

        protected virtual IEnumerator DetectionRoutine(float duration)
        {
            ChangeTrapPartsLayer(topLayer);
            yield return new WaitForSecondsRealtime(duration);
            ChangeTrapPartsLayer(defaultLayer);
        }
        #endregion

        #region Defuse Interaction
        protected virtual IEnumerator GetDefused() { yield return null; }
        #endregion

        #region Methods
        /// <summary>
        /// Create a instance of the trap material and set it in the MeshRenderer
        /// </summary>
        protected void InitalizeMaterial()
        {
            if (!trapMaterial)
                return;

            _sharedMaterial = new Material(trapMaterial);

            foreach (GameObject part in trapParts)
            {
                if (part.TryGetComponent(out MeshRenderer renderer))
                    renderer.sharedMaterial = _sharedMaterial;

                if (part.TryGetComponent(out SkinnedMeshRenderer skinned))
                    skinned.sharedMaterial = _sharedMaterial;
            }
        }

        /// <summary>
        /// Initialize some variables for the trap behaviour
        /// Executed during OnEnable
        /// </summary>
        protected virtual void InitializeTrap() { }

        public virtual void EnableHitbox(int value) { }

        /// <summary>
        /// Synchronize animator current animation over the network
        /// </summary>
        protected void SyncAnimator(int layer)
        {
            float time = Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            int hash = Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            RPCSynchronizeAnimator(Photon.Pun.RpcTarget.OthersBuffered, hash, layer, time);
        }

        /// <summary>
        /// Set the state of the tiles in the array to free
        /// </summary>
        protected virtual void ResetOccupedTiles()
        {
            if (OccupedTiles == null || OccupedTiles.Length <= 0)
                return;

            foreach (Tile tile in OccupedTiles)
            {
                if (!tile)
                    return;

                tile.NewTileState(Tile.TileState.Free);
            }

            OccupedTiles = new Tile[0];
        }
        #endregion
    }
}

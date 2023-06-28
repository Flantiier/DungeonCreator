using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using Sirenix.OdinInspector;

namespace _Scripts.UI.Menus
{
    public class LoadingMenu : MonoBehaviour
    {
        #region Variables
        [SerializeField] private PhotonView view;
        [SerializeField] private string sceneName;
        [SerializeField] private Slider slider;
        [SerializeField] private AnimatedTextField textField;
        [SerializeField] private float additionalTime = 10f;

        [Header("Multiplayer loading")]
        [ShowInInspector] private List<PlayerLoading> _loadings = new List<PlayerLoading>();
        [ShowInInspector] private bool _localReady;
        #endregion

        #region Builts_In
        private void Awake()
        {
            MenuAudio.Instance.DestroyInstance();

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PlayerLoading instance = new PlayerLoading(player);
                _loadings.Add(instance);
            }
        }

        public IEnumerator Start()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            textField.SetBaseText("chargement de la partie");
            slider.value = 0f;

            yield return new WaitForSecondsRealtime(additionalTime);

            while (!operation.isDone)
            {
                slider.value = operation.progress;
                if (operation.progress >= 0.9f)
                {
                    if (!_localReady)
                    {
                        view.RPC("LoadingRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
                        textField.SetBaseText("en attente des autres joueurs");
                        _localReady = true;
                    }

                    while (!AllPlayersReady())
                        yield return null;

                    textField.SetBaseText("debut de la partie");
                    yield return new WaitForSecondsRealtime(5f);

                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Indicates if all players in the room have loaded the game scene
        /// </summary>
        private bool AllPlayersReady()
        {
            foreach (PlayerLoading player in _loadings)
            {
                if (!player.sceneReady)
                    return false;
                else
                    continue;
            }

            return true;
        }

        [PunRPC]
        public void LoadingRPC(Player player)
        {
            PlayerLoading target = _loadings.Find(x => x.player == player);
            target.sceneReady = true;
        }
        #endregion
    }
}

#region PlayerLoading class
[System.Serializable]
public class PlayerLoading
{
    public Player player;
    public bool sceneReady;

    public PlayerLoading(Player m_player)
    {
        player = m_player;
        sceneReady = false;
    }
}
#endregion

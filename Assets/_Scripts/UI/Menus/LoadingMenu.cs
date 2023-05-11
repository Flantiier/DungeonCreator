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
        [SerializeField] private PhotonView view;
        [SerializeField] private string sceneName;
        [SerializeField] private Slider slider;
        [SerializeField] private AnimatedTextField textField;
        [ShowInInspector] private List<LoadingState> _loadings = new List<LoadingState>();
        [ShowInInspector] private bool _localReady;

        private void Awake()
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                LoadingState instance = new LoadingState(player);
                _loadings.Add(instance);
            }
        }

        public IEnumerator Start()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            textField.SetBaseText("Loading Scene");

            while (!operation.isDone)
            {
                slider.value = operation.progress;
                if (operation.progress >= 0.9f)
                {
                    if (!_localReady)
                    {
                        view.RPC("LoadingRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
                        textField.SetBaseText("Waiting other players");
                        _localReady = true;
                    }

                    while (!AllPlayersReady())
                        yield return null;

                    textField.SetBaseText("Game Starting");
                    yield return new WaitForSecondsRealtime(5f);

                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        /// <summary>
        /// Indicates if all players in the room have loaded the game scene
        /// </summary>
        private bool AllPlayersReady()
        {
            foreach (LoadingState player in _loadings)
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
            Debug.Log($"{player} is ready to play");
            LoadingState target = _loadings.Find(x => x.player == player);
            target.sceneReady = true;
        }
    }
}

[System.Serializable]
public class LoadingState
{
    public Player player;
    public bool sceneReady;

    public LoadingState(Player m_player)
    {
        player = m_player;
        sceneReady = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField] private GameObject loadingUI;
		public AsyncOperation AsyncOperation { get; private set; }

		public void EnableUI(bool enabled)
		{
			if (!loadingUI)
				return;

			loadingUI.SetActive(enabled);
		}

		public void Quit()
		{
			Application.Quit();
		}

		/// <summary>
		/// Loading a given scene
		/// </summary>
		/// <param name="sceneName"> Scene to load </param>
		public void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);
		}

        /// <summary>
        /// Asynchronically loading a given scene
        /// </summary>
        /// <param name="sceneName"> Scene to load </param>
        public void LoadSceneAsync(string sceneName)
		{
			EnableUI(true);
			StartCoroutine(AsyncLoadingRoutine(sceneName));
		}


		/// <summary>
		/// Async Loading routine 
		/// </summary>
		private IEnumerator AsyncLoadingRoutine(string sceneName)
		{
			EnableUI(true);
			AsyncOperation = SceneManager.LoadSceneAsync(sceneName);

			while (!AsyncOperation.isDone)
				yield return null;
		}
	}
}

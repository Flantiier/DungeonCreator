using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
	public class SceneLoader : PersistentSingleton<SceneLoader>
	{
		/// <summary>
		/// The current async operation running
		/// </summary>
		public AsyncOperation AsyncOperation { get; private set; }

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
			StartCoroutine(AsyncLoadingRoutine(sceneName));
		}

		/// <summary>
		/// Async Loading routine 
		/// </summary>
		private IEnumerator AsyncLoadingRoutine(string sceneName)
		{
			AsyncOperation = SceneManager.LoadSceneAsync(sceneName);

			while (!AsyncOperation.isDone)
				yield return null;

			Debug.Log("Chargement terminé");
		}
	}
}

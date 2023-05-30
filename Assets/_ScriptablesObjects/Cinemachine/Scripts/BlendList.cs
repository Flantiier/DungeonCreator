using Cinemachine;
using UnityEngine;

namespace _ScriptableObjects.Cinemachine
{
	[CreateAssetMenu(fileName = "New Blend List", menuName = "SO/Cinemachine/BlendList")]
	public class BlendList : ScriptableObject
	{
		public CinemachineBlendDefinition defaultBlend = new CinemachineBlendDefinition();
		public BlendProperties[] blendsList;

		/// <summary>
		/// Return a blend based on the given accessor value
		/// </summary>
		/// <param name="accessor"> Name of the blend to access </param>
		public CinemachineBlendDefinition GetBlend(string accessor)
		{
			foreach (BlendProperties blend in blendsList)
			{
				if (blend.name != accessor)
					continue;
				else
					return blend.blend;
			}

			return defaultBlend;
		}
	}

	[System.Serializable]
	public class BlendProperties
	{
		public string name;
		public CinemachineBlendDefinition blend = new CinemachineBlendDefinition();
	}
}

namespace PURR {
	using static UnityAsync.Await;
	using static Unity.Mathematics.math;
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using TMPro;
	using UnityAsync;
	using UnityEngine;
	using UnityEngine.EventSystems;

	///<summary>Character revealer using TextMeshPro.</summary>
	[RequireComponent(typeof(AudioSource))]
	public class Teletype : MonoBehaviour, ISubmitHandler {
		private static Teletype instance => Resources.FindObjectsOfTypeAll<Teletype>()[0];

		public GameObject arrow;

		private bool submit = false;

		private AudioSource audioSource => GetComponent<AudioSource>();
		private TMP_Text text => GetComponent<TMP_Text>();

		///<summary>List of timings for specific characters, in frames. Default is `1`.</summary>
		private Dictionary<char, int> timings = new Dictionary<char, int> {
			{ 'A', 2 },
			{ 'E', 2 },
			{ 'I', 2 },
			{ 'O', 2 },
			{ 'U', 2 },
			{ ' ', 3 },
			{ ',', 3 },
			{ '.', 3 },
			{ '!', 3 },
			{ '?', 3 },
			{ '~', 5 },
		};

		public void OnSubmit(BaseEventData eventData) {
			submit = true;
		}
		
		/// <summary>
		/// Pause text progression and show a bobbing arrow until the player presses submit.
		/// </summary>
		private async Task WaitForSubmit() {
			audioSource.Stop();
			submit = false;
			arrow.SetActive(true);
			while (!submit) {
				await NextUpdate();
			}
			arrow.SetActive(false);
			submit = false;
		}
	}
}
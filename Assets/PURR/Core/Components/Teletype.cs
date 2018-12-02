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

		public static void Reveal(Lýsing lýsing) => instance.RevealAsync(lýsing);

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

		///<summary>Display characters in the TextMeshPro text one at a time, with sound.</summary>
		private async void RevealAsync(Lýsing lýsing) {
			var random = new Unity.Mathematics.Random(BitConverter.ToUInt32(BitConverter.GetBytes(Time.time), 0));
			using (new SelectedGameObject(gameObject)) {
				var samplesPerLetter = audioSource.clip.samples / ('Z' - 'A' + 1);
				var sampleOffset = samplesPerLetter / 32;
				audioSource.pitch = random.NextFloat(0.5f, 1.5f);
				text.pageToDisplay = 1;
				text.maxVisibleCharacters = 0;
				text.SetText(lýsing.text);
				//text.SetAllDirty();
				await NextUpdate();
				transform.root.gameObject.SetActive(true);
				do {
					int firstCharacterIndexOfNextPage;
					if (text.pageToDisplay > text.textInfo.pageCount - 1) {
						firstCharacterIndexOfNextPage = text.textInfo.characterCount - 1;
					} else {
						firstCharacterIndexOfNextPage = text.textInfo.pageInfo[text.pageToDisplay].firstCharacterIndex;
					}
					if (firstCharacterIndexOfNextPage > 0 && firstCharacterIndexOfNextPage < text.maxVisibleCharacters) {
						await WaitForSubmit();
						text.pageToDisplay++;
					} else if (lýsing.pauses.Contains(text.maxVisibleCharacters)) {
						await WaitForSubmit();
					}
					++text.maxVisibleCharacters;
					var c = lýsing.plainText.ToUpper()[text.maxVisibleCharacters - 1];
					if (c >= 'A' && c <= 'Z') {
						audioSource.timeSamples = mad((c - 'A'), samplesPerLetter, sampleOffset);
						audioSource.Play();
					}
					await NextUpdate();
					if (timings.TryGetValue(c, out var timing)) {
						await new WaitForFrames(timing);
					}
				} while (text.maxVisibleCharacters < text.textInfo.characterCount);
				await WaitForSubmit();
			}
			transform.root.gameObject.SetActive(false);
		}
	}
}
﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class LocalizedText : MonoBehaviour
    {
        public string LocalizationKey;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<TextMeshPro>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}
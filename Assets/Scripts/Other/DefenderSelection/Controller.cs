using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Controllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// All of this should have been done better.

namespace Other.DefenderSelection
{
    public class Controller : MonoBehaviour
    {
        private GameManager gm;

        [SerializeField] private Container availableContainer;
        [SerializeField] private Container selectedContainer;

        private List<SimplifiedDefender> availableDefenders;
        private List<SimplifiedDefender> selectedDefenders;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            if (gm is null) throw new NullReferenceException("No gm found!");

            availableDefenders = gm.cbm.Defenders.ToList().Select(d => DefenderToSimplified(d.Value))
                .ToList();
            selectedDefenders = gm.cbm.Defenders.ToList().Select(d => DefenderToSimplified(d.Value, 0))
                .ToList();

            RefreshUI();

            availableContainer.OnDefenderCardClicked += AvailableContainerOnDefenderCardClicked;
            selectedContainer.OnDefenderCardClicked += SelectedContainerOnDefenderCardClicked;
        }

        private void AvailableContainerOnDefenderCardClicked(DefenderType type)
        {
            if (selectedDefenders.Aggregate(0, (agg, d) => agg + d.Count) >= 4) return;

            var availableDefender = availableDefenders.Find(d => d.DefenderType == type);
            if (availableDefender.Count <= 0)
                return; // in theory user should not be able to trigger this event if that happens

            availableDefenders = availableDefenders
                .Select(d => d.DefenderType == type ? new SimplifiedDefender(type, (byte) (d.Count - 1)) : d).ToList();
            selectedDefenders = selectedDefenders
                .Select(d => d.DefenderType == type ? new SimplifiedDefender(type, (byte) (d.Count + 1)) : d).ToList();

            RefreshUI();
        }

        private void SelectedContainerOnDefenderCardClicked(DefenderType type)
        {
            var selectedDefender = selectedDefenders.Find(d => d.DefenderType == type);
            if (selectedDefender.Count <= 0)
                return; // in theory user should not be able to trigger this event if that happens

            availableDefenders = availableDefenders
                .Select(d => d.DefenderType == type ? new SimplifiedDefender(type, (byte) (d.Count + 1)) : d).ToList();
            selectedDefenders = selectedDefenders
                .Select(d => d.DefenderType == type ? new SimplifiedDefender(type, (byte) (d.Count - 1)) : d).ToList();

            RefreshUI();
        }

        private void RefreshUI()
        {
            availableContainer.Items = availableDefenders.ToArray();
            selectedContainer.Items = selectedDefenders.ToArray();
        }

        private static SimplifiedDefender DefenderToSimplified(Defender defender, byte? amount = null)
        {
            amount ??= defender.Amount;
            return new SimplifiedDefender(defender.Type, (byte) amount);
        }

        public void OnStartButtonClick()
        {
            var selectedDefendersCount = selectedDefenders.ToList().Aggregate(0, (agg, d) => agg + d.Count);
            if (selectedDefendersCount != 4)
            {
                EditorUtility.DisplayDialog("Cannot start battle",
                    "You have to select 4 defenders before starting battle!", "OK");
                return;
            }

            SaveToGM();
            SceneManager.LoadScene("Scenes/Battle");
        }

        // ReSharper disable once InconsistentNaming
        private void SaveToGM()
        {
            gm.selectedDefenders = new DefenderType[] { };

            foreach (var selectedDefender in selectedDefenders)
            {
                for (var i = 0; i <= selectedDefender.Count; i++)
                {
                    gm.selectedDefenders = gm.selectedDefenders.Append(selectedDefender.DefenderType).ToArray();
                }
            }
        }
    }
}
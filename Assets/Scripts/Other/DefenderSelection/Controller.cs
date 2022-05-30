using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Controllers;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

// All of this should have been done better.

namespace Other.DefenderSelection
{
    public class Controller : MonoBehaviour
    {
        private Dictionary<DefenderType, Character> DefenderTypeCharacter =>
            new()
            {
                {DefenderType.Drone, drone},
                {DefenderType.Fighter, fighter},
                {DefenderType.Mech, mech},
                {DefenderType.Medic, medic},
                {DefenderType.Shooter, shooter}
            };

        private GameManager gm;

        [SerializeField] private Container availableContainer;
        [SerializeField] private Container selectedContainer;

        public List<SimplifiedDefender> availableDefenders;
        public List<SimplifiedDefender> selectedDefenders;
        
        #region Characters

        [Header("All player characters")] 
        public Character drone;
        public Character fighter;
        public Character shooter;
        public Character mech;
        public Character medic;
        [Header("All bosses")]
        public Character octomedusa;
        public Character salamander;
        public Character molemother;
        [Header("All minor enemies")]
        public Character cockroach;
        public Character crab;
        public Character molerat;
        public Character mosquito;
        public Character sleeker;
        public Character toxicJelly;


        #endregion
        
        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            if (gm is null) throw new NullReferenceException("No gm found!");
            
            availableDefenders = gm.cbm.Defenders.ToList().Select(d => DefenderToSimplified(d.Value))
                .ToList();
            selectedDefenders = gm.cbm.Defenders.ToList().Select(d => DefenderToSimplified(d.Value, 0))
                .ToList();

            if (availableDefenders.Aggregate(0, (agg, d) => d.Count > 0 ? agg + 1 : agg) < 4)
            {
                SceneManager.LoadScene("Scenes/GameOver");
                return;
            }
        
            RefreshUI();
        
            availableContainer.OnDefenderCardClicked += AvailableContainerOnDefenderCardClicked;
            selectedContainer.OnDefenderCardClicked += SelectedContainerOnDefenderCardClicked;
        }
        
        private void AvailableContainerOnDefenderCardClicked(DefenderType type)
        {
            if (selectedDefenders.Aggregate(0, (agg, d) => agg + d.Count) >= 4) return;
            if (selectedDefenders.FindIndex(d=> d.DefenderType == type && d.Count > 0) != -1) return; // :/
        
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
                EditorUtility.DisplayDialog("Przed wyruszeniem w drogę należy zebrać drużynę!",
                    "You have to select 4 defenders before starting battle!", "OK");
                return;
            }
        
            SaveToGM();
            SceneManager.LoadScene("Scenes/Battle");
        }
        
        // ReSharper disable once InconsistentNaming
        private void SaveToGM()
        {
            gm.selectedDefenders = new List<Character>();
        
            foreach (var selectedDefender in selectedDefenders)
            {
                for (var i = 0; i < selectedDefender.Count; i++)
                {
                    gm.selectedDefenders.Add(DefenderTypeCharacter[selectedDefender.DefenderType]);
                }
            }
            CreateEnemyTeam();
        }

        private void CreateEnemyTeam()
        {
            var randomChoice = Random.Range(0, 2);

            switch (randomChoice)
            {
                case 1:
                    gm.thisEncounterEnemies = new List<Character>
                    {
                        crab,
                        toxicJelly,
                        octomedusa
                    };
                    break;
                case 2:
                    gm.thisEncounterEnemies = new List<Character>
                    {
                        molerat,
                        cockroach,
                        molemother
                    };
                    break;
                case 3:
                    gm.thisEncounterEnemies = new List<Character>
                    {
                        mosquito,
                        sleeker,
                        salamander
                    };
                    break;
            }
        }
    }
}
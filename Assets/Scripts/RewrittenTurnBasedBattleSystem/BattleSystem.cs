using System;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private BattleHandler battleHandler;
        private readonly TeamSpawner teamSpawner = new();

        [SerializeField] CharacterData[] playerTeam;
        [SerializeField] CharacterData[] enemyTeam;
        [ContextMenu("Start battle")]
        void StartBattle()
        {
            battleHandler.playerTeam = teamSpawner.SpawnTeam(playerTeam);
            battleHandler.enemyTeam = teamSpawner.SpawnTeam(enemyTeam);
            battleHandler.Prepare();
        }
        [ContextMenu("Tick")]
        void Tick()
        {
            battleHandler.Tick();
        }

        private event Action OnBattleEnded;
        
    }
}
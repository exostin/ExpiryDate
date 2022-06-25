using System;
using RewrittenTurnBasedBattleSystem.ScriptableObjects;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private CharacterData[] playerTeamData;
        [SerializeField] private CharacterData[] enemyTeamData;
        private readonly BattleHandler battleHandler = new();
        private readonly TeamSpawner teamSpawner = new();

        private void Start()
        {
            battleHandler.AssignCoroutineInvokerReference(this);
        }

        [ContextMenu("Prepare")]
        private void Prepare()
        {
            battleHandler.PlayerTeam = teamSpawner.SpawnTeam(playerTeamData);
            battleHandler.EnemyTeam = teamSpawner.SpawnTeam(enemyTeamData);
        }

        [ContextMenu("Tick")]
        private void Tick()
        {
            battleHandler.Tick();
        }

        private event Action OnBattleEnded;
    }
}
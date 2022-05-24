using System;
using System.Collections;
using UnityEngine;

namespace RewrittenTurnBasedBattleSystem
{
    internal class AICharacterHandler : ICharacterHandler
    {
        private Character character;
        private MonoBehaviour coroutineInvoker;

        public AICharacterHandler(Character character, MonoBehaviour coroutineInvoker)
        {
            this.character = character;
            this.coroutineInvoker = coroutineInvoker;
        }

        public Character Character => character;

        public event Action OnActionFinished;

        public void PerformAction()
        {
            coroutineInvoker.StartCoroutine(PerformActionCoroutine());
        }

        private IEnumerator PerformActionCoroutine()
        {
            yield return new WaitForSeconds(1);
            character.abilities[0].Perform();
            OnActionFinished?.Invoke();
        }
    }
}
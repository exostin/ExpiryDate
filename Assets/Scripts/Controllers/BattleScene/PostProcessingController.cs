using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace Controllers.BattleScene
{
    public class PostProcessingController : MonoBehaviour
    {
        [SerializeField] private Volume postprocessingVolume;
        [SerializeField] private float defaultVignetteIntensity;
        [SerializeField] private float enemyTurnVignetteIntensity;
        [SerializeField] private float attackChromaticAberrationIntensity = 0.5f;
        private Vignette vignette;
        [SerializeField] private Color defaultVignette;
        [SerializeField] private Color enemyTurnVignette;
        private ChromaticAberration chromaticAberration;

        void Start()
        {
            GetPostProcessingReferences();
        }
        
        /// <summary>
        ///     Get references to post processing components
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        private void GetPostProcessingReferences()
        {
            var postprocessingVolumeProfile = postprocessingVolume.GetComponent<Volume>()?.profile;
            if (!postprocessingVolumeProfile) throw new NullReferenceException(nameof(VolumeProfile));
            if (!postprocessingVolumeProfile.TryGet(out vignette)) throw new NullReferenceException(nameof(vignette));
            if (!postprocessingVolumeProfile.TryGet(out chromaticAberration))
                throw new NullReferenceException(nameof(chromaticAberration));
        }
        
        /// <summary>
        ///     Toggles post effects that should be visible during the enemy turn
        /// </summary>
        public void ToggleEnemyTurnPostEffects(GameManager gm)
        {
            vignette.intensity.Override(gm.stateController.fsm.State == StateController.States.EnemyTurn
                ? enemyTurnVignetteIntensity
                : defaultVignetteIntensity);
            vignette.color.Override(gm.stateController.fsm.State == StateController.States.EnemyTurn
                ? enemyTurnVignette
                : defaultVignette);
        }

        /// <summary>
        ///     Toggles post effects that should be visible when a character makes an attack
        /// </summary>
        public IEnumerator MakeAttackPostEffects()
        {
            for (float i = 0; i < attackChromaticAberrationIntensity; i += 0.05f)
            {
                Debug.Log($"Setting chromatic aberration to: {i}, actual: {attackChromaticAberrationIntensity}");
                chromaticAberration.intensity.Override(i);
                yield return new WaitForSecondsRealtime(0.02f);
            }

            for (var i = attackChromaticAberrationIntensity; i > 0; i -= 0.05f)
            {
                chromaticAberration.intensity.Override(i);
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }
    }
}

using HarmonyLib;
using Reptile;
using UnityEngine;

namespace AnimateOnTwos.Patches
{
    internal static class PlayerPatch
    {
        [HarmonyPatch(typeof(Player), nameof(Player.Init))]
        [HarmonyPostfix]
        private static void Player_Init_Postfix(Player __instance)
        {
            if (!__instance.isAI) { AnimateOnTwosPlugin.player = __instance; }
            var state = new AnimatorPatchState();
            AnimateOnTwosPlugin.PlayerPatchStates.Add(__instance, state);
        }

        [HarmonyPatch(typeof(Player), nameof(Player.UpdatePlayer))]
        [HarmonyPostfix]
        private static void Player_UpdatePlayer_Postfix(Player __instance)
        {
            if(AnimateOnTwosPlugin.PlayerTargetFps == 0) return;
            AnimateOnTwosPlugin.PlayerPatchStates.TryGetValue(__instance, out AnimatorPatchState state);
            var targetFrameDuration = 1f / AnimateOnTwosPlugin.PlayerTargetFps;
            state.animator = state.animator != null ? state.animator : __instance.GetComponentInChildren<Animator>();
            state.time += Time.deltaTime;
            if(state.animator.speed != 0 && state.animator.speed != state.iSetThisSpeed) {
                state.someoneElseSetThisSpeed = state.animator.speed;
            }
            state.animator.speed = state.iSetThisSpeed = 0;
            if(state.time > targetFrameDuration) {
                state.time -= targetFrameDuration;
                // Set speed such that this frame advances the animator exactly as far as we want.
                // If we want to advance animation by .2 seconds, and delta time is .1 seconds,
                // then animation speed should be 2x.
                state.animator.speed = state.iSetThisSpeed = state.someoneElseSetThisSpeed * targetFrameDuration / Time.deltaTime;
            }
        }
    }
}

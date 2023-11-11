using System;
using System.Runtime.InteropServices;
using HarmonyLib;
using Reptile;
using UnityEngine;

namespace AnimateOnTwos.Patches
{
    internal static class PedestrianPatch
    {
        [HarmonyPatch(typeof(StreetLife), nameof(StreetLife.Init))]
        [HarmonyPostfix]
        private static void Init_Postfix(StreetLife __instance)
        {
            if (__instance is Pedestrian p)
            {
                var state = new AnimatorPatchState();
                try {
                    AnimateOnTwosPlugin.PedestrianPatchStates.Add(p, state);
                } catch(ArgumentException) {}
                AnimateOnTwosPlugin.Pedestrians.Add(p);
            }
        }

        [HarmonyPatch(typeof(StreetLife), nameof(StreetLife.OnDestroy))]
        [HarmonyPrefix]
        private static void OnDestroy_Prefix(StreetLife __instance) {
            if (__instance is Pedestrian p)
            {
                AnimateOnTwosPlugin.Pedestrians.Remove(p);
            }
        }

        internal static void Update()
        {
            if(AnimateOnTwosPlugin.PedestrianTargetFps == 0) return;

            var targetFrameDuration = 1f / AnimateOnTwosPlugin.PedestrianTargetFps;
            foreach(var __instance in AnimateOnTwosPlugin.Pedestrians) {
                AnimateOnTwosPlugin.PedestrianPatchStates.TryGetValue(__instance, out AnimatorPatchState state);
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
}

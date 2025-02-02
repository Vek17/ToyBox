﻿// Copyright < 2021 > Narria(github user Cabarius) - License: MIT
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.BundlesLoading;
using ModKit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToyBox.classes.MainUI;
using UnityEngine;

namespace ToyBox {
    public class BlueprintLoader : MonoBehaviour {
        public delegate void LoadBlueprintsCallback(IEnumerable<SimpleBlueprint> blueprints);

        private LoadBlueprintsCallback callback;
        private List<SimpleBlueprint> _blueprintsInProcess;
        private List<SimpleBlueprint> blueprints => loader.IsDone ? loader.Blueprints : null;
        //private List<SimpleBlueprint> blueprints;
        public float progress => loader.Progress;
        private static BlueprintLoader _shared;
        private static TestLoader.BlueprintCacheLoader loader = new TestLoader.BlueprintCacheLoader();
        public static BlueprintLoader Shared {
            get {
                if (_shared == null) {
                    _shared = new GameObject().AddComponent<BlueprintLoader>();
                    DontDestroyOnLoad(_shared.gameObject);
                }
                return _shared;
            }
        }
        private IEnumerator coroutine;
        /*
        private void UpdateProgress(int loaded, int total) {
            if (total <= 0) {
                progress = 0.0f;
                return;
            }
            progress = loaded / (float)total;
        }
        */
        private IEnumerator LoadBlueprints() {
            yield return null;
            var bpCache = ResourcesLibrary.BlueprintsCache;
            while (bpCache == null) {
                yield return null;
                bpCache = ResourcesLibrary.BlueprintsCache;
            }
            _blueprintsInProcess = new List<SimpleBlueprint> { };
            var toc = ResourcesLibrary.BlueprintsCache.m_LoadedBlueprints;
            while (toc == null) {
                yield return null;
                toc = ResourcesLibrary.BlueprintsCache.m_LoadedBlueprints;
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
#if true    // TODO - Truinto for evaluation; my result improved from 2689 to 17 milliseconds
            var loaded = 0;
            var total = 1;
            var allGUIDs = new List<BlueprintGuid> { };
            foreach (var key in toc.Keys) {
                allGUIDs.Add(key);
            }
            total = allGUIDs.Count;
            Mod.Log($"Loading {total} Blueprints");
            //UpdateProgress(loaded, total);
            foreach (var guid in allGUIDs) {
                SimpleBlueprint bp;
                try {
                    bp = bpCache.Load(guid);
                }
                catch {
                    Mod.Warn($"cannot load GUID: {guid}");
                    continue;
                }
                _blueprintsInProcess.Add(bp);
                loaded += 1;
                //UpdateProgress(loaded, total);
                if (loaded % 1000 == 0) {
                    yield return null;
                }
            }
#else
            blueprints = ResourcesLibrary.BlueprintsCache.m_LoadedBlueprints.Values.Select(s => s.Blueprint).ToList();
#endif
            watch.Stop();
            Mod.Log($"loaded {_blueprintsInProcess.Count} blueprints in {watch.ElapsedMilliseconds} milliseconds");
            callback(_blueprintsInProcess);
            yield return null;
            StopCoroutine(coroutine);
            coroutine = null;
        }
        private void Load(LoadBlueprintsCallback callback) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            var loader = new TestLoader.BlueprintCacheLoader();
            loader.Start();
            this.callback = callback;
            //coroutine = loader.WaitFor(UpdateProgress);
            StartCoroutine(coroutine);
            //blueprints = loader.Blueprints.ToList();
            callback.Invoke(loader.Blueprints);
        }
        public bool IsLoading => loader.IsRunning;
        /*
        public List<SimpleBlueprint> GetBlueprints() {
            if (blueprints == null) {
                if (BlueprintLoader.Shared.IsLoading) { return null; }
                else {
                    Mod.Debug($"calling BlueprintLoader.Load");
                    BlueprintLoader.Shared.Load((bps) => {
                        _blueprintsInProcess = bps.ToList();
                        blueprints = _blueprintsInProcess;
                        Mod.Debug($"success got {bps.Count()} bluerints");
                    });
                    return null;
                }
            }
            return blueprints;
        }
        */
        public List<SimpleBlueprint> GetBlueprints() {
            if (!loader.IsDone) {
                if (loader.IsRunning) { return null; }
                else {
                    Mod.Debug($"calling BlueprintLoader.Load");
                    loader.Start();
                    //BlueprintLoader.Shared.Load((bps) => {
                    //    _blueprintsInProcess = bps.ToList();
                    //    blueprints = _blueprintsInProcess;
                    //    Mod.Debug($"success got {bps.Count()} bluerints");
                    //});
                    return null;
                }
            }
            return blueprints;
        }
        public List<BPType> GetBlueprints<BPType>() {
            var bps = GetBlueprints();
            return bps?.OfType<BPType>().ToList() ?? null;
        }
        //[HarmonyPatch(typeof(StartGameLoader), nameof(StartGameLoader.LoadAllJson))]
        private static class SlientInit {
            [HarmonyPostfix]
            static void Postfix() {
                loader.Start();
                Mod.Log($"Blueprint Loading Started");
            }
        }
    }

    public static class BlueprintLoader<BPType> {
        public static IEnumerable<BPType> blueprints = null;
    }

    public static class BlueprintLoaderOld {
        public delegate void LoadBlueprintsCallback(IEnumerable<SimpleBlueprint> blueprints);

        private static AssetBundleRequest LoadRequest;
        public static float progress = 0;
        public static void Load(LoadBlueprintsCallback callback) {
#if false
            var bundle = (AssetBundle)AccessTools.Field(typeof(ResourcesLibrary), "s_BlueprintsBundle").GetValue(null);
            Main.Log($"got bundle {bundle}");
            LoadRequest = bundle.LoadAllAssetsAsync<BlueprintScriptableObject>();
#endif
            var bundle = BundlesLoadService.Instance.RequestBundle(AssetBundleNames.BlueprintAssets);
            BundlesLoadService.Instance.LoadDependencies(AssetBundleNames.BlueprintAssets);
            LoadRequest = bundle.LoadAllAssetsAsync<object>();
            Mod.Trace($"created request {LoadRequest}");
            LoadRequest.completed += (asyncOperation) => {
                Mod.Trace($"completed request and calling completion - {LoadRequest.allAssets.Length} Assets ");
                callback(LoadRequest.allAssets.Cast<SimpleBlueprint>());
                LoadRequest = null;
            };
        }
        public static bool LoadInProgress() {
            if (LoadRequest != null) {
                progress = LoadRequest.progress;
                return true;
            }
            return false;
        }
    }
}

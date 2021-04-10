/*
 * Example Game Mod - An example mod that makes the last names of 25% of cims' be "Modmaker"
 *
 * Written in 2021 by Radu Hociung (radu.csmods@ohmi.org)
 *
 * To the extent possible under law, the author(s) have dedicated all copyright and related
 * and neighboring rights to this software to the public domain worldwide. This software is
 * distributed without any warranty.
 *
 * You should have received a copy of the CC0 Public Domain Dedication along with this
 * software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
 */

using ICities;

namespace ExampleGameMod
{
    /*
     * This is mod that changes the behaviour of the game. It changes the last names
     * of some of the cims (a pseudo-random 25% sampling) to "ModMaker".
     * It demonstrates attribute-driven patching using HarmonyLib to
     * patch the CitizenAI.GenerateCitizenName() method and override it.
     *
     * If Harmony is not already installed, it will be automatically installed and the
     * patch will be done when Harmony is ready.
     *
     * The mod demonstrates simple mod lifecycle:
     * - it operates only when Game mode starts (ie, when a city is loaded)
     * - it removes the patching callback request in case Harmony install is delayed or fails
     * - it removes its own patches when Game mode ends.
     * - it removes itself cleanly when the assembly file is deleted (ie, it can be unsubscribed
     *   or deleted mid-game)
     */
    public class ExampleMod : LoadingExtensionBase, IUserMod
    {
        public string Name => "Example Cim Name Mod";

        public string Description => "Change some cim last names to 'Modmaker'";

        /* This object is a HarmonyLib.Harmony object but it must be declared as a generic
         * object so it does not need to be resolved on GetExportedTypes(), or it will
         * cause the legacy plugin manager to crash.
         */
        object patcher;

        /* When the mod assembly is removed by unsubscribing or deleting the mod from
         * the disk, the plugin manager calls OnDisabled().
         *
         * Upon this trigger, the mod will cleanly unload.
         */
        public void OnDisabled()
        {
            Unload();
        }

        /* LoadingExtension.OnCreated() is called when the player starts an appmode
         * by loading a city, the asset editor or a map editor.
         */
        public override void OnCreated(ILoading loading)
        {
            if (loading.currentMode == AppMode.Game)
            {
                HarmonyManager.Harmony.DoOnHarmonyReady(PatchGame);
            }
            base.OnCreated(loading);
        }

        /* LoadingExtension.OnReleased() is called when the player
         * returns to the main menu.
         */
        public override void OnReleased()
        {
            Unload();
            base.OnReleased();
        }

        void PatchGame()
        {
            if (patcher == null)
            {
                /* On first call, allocate the Harmony instance */
                patcher = new HarmonyLib.Harmony(Name);
            }
            (patcher as HarmonyLib.Harmony).PatchAll();
        }

        void Unload()
        {
            /* If Harmony installation is still pending, cancel the callback */
            HarmonyManager.Harmony.CancelOnHarmonyReady();

            /* If patching was done, remove it */
            (patcher as HarmonyLib.Harmony)?.UnpatchAll(Name);
        }
    }
}

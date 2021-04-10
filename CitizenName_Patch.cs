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

/* This file contains patches to be installed on the Cities Skylines game
 * assembly.
 * It implements annotations that direct the Harmony Library's patching process.
 * Please see documentation on the annotations and the workings of the
 * Harmony Library at the author's manual webpage at
 * https://harmony.pardeike.net/articles/annotations.html
 *
 */


namespace ExampleMod.Patch
{
    using HarmonyLib;
    using ColossalFramework.Math;

    [HarmonyPatch(typeof(CitizenAI), "GenerateCitizenName")]
    internal class CitizenAI_GenerateCitizenName_Patch
    {
        public static bool Prefix(CitizenAI __Instance, uint citizenID, byte family, ref string __result)
        {
            Randomizer randomizer = new Randomizer(citizenID);
            Randomizer randomizer2 = new Randomizer(family);
            string firstName;
            string lastName;
            if (Citizen.GetGender(citizenID) == Citizen.Gender.Male)
            {
                firstName = "NAME_MALE_FIRST";
                lastName = "NAME_MALE_LAST";
            } else
            {
                firstName = "NAME_FEMALE_FIRST";
                lastName = "NAME_FEMALE_LAST";

            }
            firstName = ColossalFramework.Globalization.Locale.Get(firstName, randomizer.Int32(ColossalFramework.Globalization.Locale.Count(firstName)));
            if (randomizer.Int32(4) == 0)
            {
                lastName = "Modmaker";
            } else
            {
                lastName = ColossalFramework.Globalization.Locale.Get(lastName, randomizer2.Int32(ColossalFramework.Globalization.Locale.Count(lastName)));
            }
            __result = StringUtils.SafeFormat(lastName, firstName);

            return false;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace Stats {

    //This is a component to facilitate easy and fast networking. If it were a pure C# class, it'd allocate each time it was sync'd
    //Being a component also allows us to set the StatsConfig directly
    //I'm unsure what shape single player/multiplayer saving will take, but it is trivial to save this once that is chosen
    [System.Serializable]
    public class PlayerStats : MonoBehaviour {

        //Holds a stat and its xp, readonly so that when syncvars are applied it syncs correctly
        private readonly struct Stat {
            public readonly int value;
            public readonly int xp;

            public Stat(int value) {
                this.value = value;
                this.xp = 0;
            }

            public Stat(int value, int xp) {
                this.value = value;
                this.xp = xp;
            }

            public Stat AddXp(int newXp) {
                return new Stat(this.value, this.xp + newXp);
            }
        }

        [Header("Configuration, must set")]
        [SerializeField]
        private StatsConfig config;

        //It may be better to move these to a central manager with a player id for networking, along with any other per-player events
        //
        //Delegate things can subscribe to in order to change the type of xp coming in
        public System.Func<CharacterStat, CharacterStat> changeStatXpType;
        //Called when a stat is successfully leveled up
        public System.Action<CharacterStat, int> onStatUp;
        //Called when the character is successfully levels up
        public System.Action<int> onLevelUp;

        //TODO: make a sync dictionary
        private Dictionary<CharacterStat, Stat> stats;

        public int Physique { get => stats[CharacterStat.physique].value; }
        public int Spirit { get => stats[CharacterStat.spirit].value; }
        public int Strength { get => stats[CharacterStat.strength].value; }
        public int Force { get => stats[CharacterStat.force].value; }
        public int Resistance { get => stats[CharacterStat.resistance].value; }
        public int Ego { get => stats[CharacterStat.ego].value; }

        //TODO: Sync these
        [field: Header("Level info")]
        [field: SerializeField]
        public int Level { get; private set; }

        [field: SerializeField]
        public int AvailableSkillPoints { get; private set; }

        private int statPointsToLevel;

        /// <summary>
        /// Sets up a brand new character from a class stat block
        /// </summary>
        public void SetUpForClass(BaseStatBlock classStats) {
            stats = new Dictionary<CharacterStat, Stat>();

            stats.Add(CharacterStat.physique, new Stat(classStats.Physique));
            stats.Add(CharacterStat.spirit, new Stat(classStats.Spirit));
            stats.Add(CharacterStat.strength, new Stat(classStats.Strength));
            stats.Add(CharacterStat.force, new Stat(classStats.Force));
            stats.Add(CharacterStat.resistance, new Stat(classStats.Resistance));
            stats.Add(CharacterStat.ego, new Stat(classStats.Ego));

            Level = config.StartingLevel;
            AvailableSkillPoints = 0;
            statPointsToLevel = config.StatPointsPerLevel;
        }

        public void SetUpFromLoad() {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Convenience method for getting a stat given the enum
        /// </summary>
        public int GetStat(CharacterStat stat) {
            return stats[stat].value;
        }

        /// <summary>
        /// Returns whether a stat is valid to be leveled
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public bool CanLevelStat(CharacterStat stat) {
            var s = stats[stat];

            int gapToLowest = 0;

            foreach(var other in stats.Values) {
                gapToLowest = Mathf.Max(s.value - other.value);
            }

            if(gapToLowest > config.MaxStatGap) {
                //TODO show message to player indicating "cannot raise stats too far beyond other stats"

                return false;
            }

            if(s.value > config.StatMax) {
                return false;
            }

            return true;
        }

        //TODO: MAKE SERVER-SIDE ONLY
        /// <summary>
        /// Gains xp in a stat, leveling it up if applicable
        /// </summary>
        public void GainXp(CharacterStat stat, int xp) {
            var s = stats[stat];

            s = s.AddXp(xp);

            var reqXp = config.StatXpCurve.GetRequiredXpForLevel(s.value);
            if(s.xp > reqXp && CanLevelStat(stat)) {
                //Stat up

                s = new Stat(s.value + 1, s.xp - reqXp);

                onStatUp(stat, s.value);
                statPointsToLevel--;

                if(statPointsToLevel <= 0) {
                    //LevelUp
                    statPointsToLevel = config.StatPointsPerLevel;
                    Level++;

                    onLevelUp(Level);
                }
            }

            stats[stat] = s;
        }

        public void UpStat(CharacterStat stat, int amount) {
            var s = stats[stat];

            for(int i = 0; i < amount; i++) {
                if(CanLevelStat(stat)) {
                    s = new Stat(s.value + 1, s.xp);
                } else {
                    break;
                }
            }
        }

        public void UpLevel(int amount) {
            Level += amount;
            Level = Mathf.Clamp(Level, 0, config.LevelMax);
        }

    }

}
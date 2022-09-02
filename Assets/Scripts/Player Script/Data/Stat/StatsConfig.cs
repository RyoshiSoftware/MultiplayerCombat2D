using UnityEngine;

namespace Stats {
    [CreateAssetMenu(fileName = "StatsConfig", menuName = "ScriptableObject/Stats/StatsConfig")]
    public class StatsConfig : ScriptableObject {

        public XpCurve StatXpCurve;
        public int StatMax;
        public int MaxStatGap;
        public int StartingLevel;
        public int LevelMax;
        public int StatPointsPerLevel;

    }
}
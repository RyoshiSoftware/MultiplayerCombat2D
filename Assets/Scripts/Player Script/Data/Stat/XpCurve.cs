using UnityEngine;

namespace Stats {

    [CreateAssetMenu(fileName = "XpCurve", menuName = "ScriptableObject/Stats/XpCurve")]
    public class XpCurve : ScriptableObject {

        [SerializeField]
        private AnimationCurve curve;

        public int GetRequiredXpForLevel(int level) {
            if(level < 0 || level > GetMaxLevel()) {
                throw new System.IndexOutOfRangeException($"Trying to level to {level}, must be between 0 and {GetMaxLevel()}");
            }

            return (int) curve.Evaluate(level);
        }

        public int GetMaxLevel() {
            return (int) curve.keys[curve.length - 1].time;
        }
    }

}
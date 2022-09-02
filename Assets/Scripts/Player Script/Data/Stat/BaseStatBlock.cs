using UnityEngine;

namespace Stats {
    [CreateAssetMenu(fileName = "ClassStat", menuName = "ScriptableObject/Stats/BaseStatBlock")]
    public class BaseStatBlock : ScriptableObject {
        [field: SerializeField]
        public int Physique { get; private set; }

        [field: SerializeField]
        public int Spirit { get; private set; }

        [field: SerializeField]
        public int Strength { get; private set; }

        [field: SerializeField]
        public int Force { get; private set; }

        [field: SerializeField]
        public int Resistance { get; private set; }

        [field: SerializeField]
        public int Ego { get; private set; }
    }
}
using UnityEngine;

namespace Stats {

    public class Health : MonoBehaviour {
    
        [field: SerializeField]
        public int Amount { get; private set; }

        [field: SerializeField]
        public int MaxHealth { get; private set; }

        //Should callbacks be moved to a callback manager with a player/entity id?
        public System.Action<int> onTakeDamage;
        public System.Action<int> onHeal;
        public System.Action onDeath;

        public void ChangeHealth(int amount) {
            Amount += amount;
            
            Amount = Mathf.Clamp(Amount, 0, MaxHealth);

            if(Amount == 0) {
                onDeath?.Invoke();
            }

            if(amount < 0) {
                onTakeDamage?.Invoke(Mathf.Abs(amount));
            } else if (amount > 0) {
                onHeal?.Invoke(amount);
            }
        }

        public void SetMaxHealth(int max) {
            MaxHealth = max;
            Amount = Mathf.Clamp(Amount, 0, MaxHealth);
        }

    }

}
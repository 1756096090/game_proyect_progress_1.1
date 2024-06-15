using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Stats
{
    public class PlayerStats : MonoBehaviour
    {
        
        [BoxGroup("Stats")]
        [LabelText("Vida M�xima")]
        [SerializeField] public float maxHealth;
        [BoxGroup("Stats")]
        [LabelText("Ataque Base")]
        [SerializeField] private float attack;
        [BoxGroup("Stats")]
        [LabelText("Defensa")]
        [SerializeField] private int defense;
        

        private float _currentHealth;


        private void Start()
        {
            InitDefaults();
        }

        public float Health { get 
            {
                return _currentHealth;
            } set 
            {
                if(value != _currentHealth)
                {
                    Debug.Log("Target is: " + gameObject.name);
                    Debug.Log("Health changed from " + _currentHealth + " to " + value);
                    _currentHealth = Mathf.Max(value, 0f);
                    HealthChanged?.Invoke(_currentHealth);
                }
            } 
        }

        public float Attack { get; set;}
        public int Defense { get; set; }


        public UnityAction<float> HealthChanged;

        public void InitDefaults()
        {
            Health = maxHealth;
            Attack = attack;
            Defense = defense;
            
        }

        public void AumentarVidaMaxima(float cantidad)
        {
            maxHealth += cantidad;
        }
    }
}
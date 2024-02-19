using System.Text;
using SGoap;
using UnityEngine;
using UnityEngine.UI;

namespace SGOAP.Examples
{
    public class DemoInput : MonoBehaviour
    {
        public Character Player;
        public Enemy Enemy;
        public AgentPickUpSystem AgentPickUpSystem;
        public Agent Agent;

        public Text PlayerHealthLabel;
        public Text AgentHealthLabel;
        public Text AgentScoreLabel;

        private void Awake()
        {
            Player.OnHealthChanged += OnPlayerHealthChanged;
            Enemy.OnHealthChanged += OnEnemyHealthChanged;
            Enemy.OnPointChanged += OnEnemyPointChanged;
            
            OnPlayerHealthChanged();
            OnEnemyHealthChanged();
            OnEnemyPointChanged(Enemy.Points);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Enemy.TakeDamage(1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach (var itemObject in AgentPickUpSystem.FoundItems)
                    itemObject.gameObject.SetActive(true);

                // When you have a 'new key' change, you might want to force replan if this is how you to handle it.
                Agent.ForceReplan();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Player.HP = Player.MaxHP;
                Player.gameObject.SetActive(true);
                Agent.ForceReplan();
            }
        }
        private void OnEnemyHealthChanged()
        {
            AgentHealthLabel.text = $"{"Enemy HP: ".ToBold()} {Enemy.HP}";
        }

        private void OnPlayerHealthChanged()
        {
            PlayerHealthLabel.text = $"{"Player HP: ".ToBold()} {Player.HP}";
        }

        private void OnEnemyPointChanged(int point)
        {
            AgentScoreLabel.text = $"{"Enemy Points: ".ToBold()}{point}";
        }
    }

    public static class StringExtensions
    {
        public static string ToBold(this string s)
        {
            return $"<b>{s}</b>";
        }
    }
}
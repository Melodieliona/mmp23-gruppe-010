using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public Color low;
        public Color high;

        public Vector3 offset;

        private void Update()
        {
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
        }

        public void SetHealth(double health, double maxHealth)
        {
            slider.gameObject.SetActive(health < maxHealth);
            slider.value = (float) health;
            slider.maxValue = (float) maxHealth;

            slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
        }
    }
}
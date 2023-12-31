using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace STATUS
{
    public class StatusBar : MonoBehaviour
    {
        public static StatusBar instance;
        [System.Serializable]
        public class StatusElement
        {
            public TMP_Text nameText;
            public TMP_Text valueText;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] private StatusElement employeeCount;
        [SerializeField] private StatusElement employeeHappiness;
        [SerializeField] private StatusElement employeeSkill;
        [SerializeField] private StatusElement marketing;
        [SerializeField] private StatusElement reputation;
        [SerializeField] private StatusElement budget;
        [SerializeField] private StatusElement work;
        [SerializeField] private StatusElement working;
        [SerializeField] private TMP_Text monthText;
        [SerializeField] private TMP_Text yearText;

        public CompanyManager companyManager;
        public DecisionManager decisionManager;

        [SerializeField] private int currentMonth = 1;
        [SerializeField] private int currentYear = 1;
        [SerializeField] private int decisionsCount = 0;
        [SerializeField] private CanvasGroup canvasGroup;
        private CanvasGroupController cg;

        private string[] thaiMonthNames = new string[]
        {
            "มกราคม",
            "กุมภาพันธ์",
            "มีนาคม",
            "เมษายน",
            "พฤษภาคม",
            "มิถุนายน",
            "กรกฎาคม",
            "สิงหาคม",
            "กันยายน",
            "ตุลาคม",
            "พฤศจิกายน",
            "ธันวาคม"
        };

        private void Start()
        {
            cg = new CanvasGroupController(this, canvasGroup);
            cg.alpha = 0;
            cg.SetInteractableState(active: false);
            UpdateStatusValues();
        }

        public void Show()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f; // Set the alpha to make it visible
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true; // Optional: Enable raycasts if needed
            }
        }

        public void Hide()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f; // Set the alpha to make it invisible
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false; // Optional: Disable raycasts if needed
            }
        }


        public void UpdateStatusValues()
        {
            employeeCount.valueText.text = companyManager.employeeNumber.ToString();
            employeeHappiness.valueText.text = companyManager.employeeHappiness.ToString();
            employeeSkill.valueText.text = companyManager.employeeSkills.ToString();
            marketing.valueText.text = companyManager.marketing.ToString();
            reputation.valueText.text = companyManager.reputation.ToString();
            budget.valueText.text = companyManager.budget.ToString();
            work.valueText.text = companyManager.work.ToString();

            float workingValue = (companyManager.employeeNumber * companyManager.employeeSkills) / 10f;
            working.valueText.text = workingValue.ToString("F1"); // Format to 1 decimal place

            // Update the month and year based on decision count
            UpdateMonthAndYearThai();

        }

        private void UpdateMonthAndYearThai()
        {
            string monthName = GetThaiMonthName(currentMonth);
            monthText.text = monthName;
            yearText.text = currentYear.ToString();
        }

        private string GetThaiMonthName(int monthNumber)
        {
            if (monthNumber < 1 || monthNumber > 12)
                return "";

            return thaiMonthNames[monthNumber - 1];
        }

        // Sell Work button functionality
        public void SellWork()
        {
            if (companyManager.work > 0)
            {
                companyManager.work--;
                companyManager.marketing--;
                companyManager.budget++;

                UpdateStatusValues();
            }
        }

        // Call this function when a decision is made
        public void DecisionMade()
        {
            decisionsCount++;

            if (decisionsCount % 2 == 0)
            {
                currentMonth++;
            }

            if (decisionsCount % 12 == 0)
            {
                currentMonth = 1;
                decisionsCount = 0;
                currentYear++;
            }
        }
    }
}

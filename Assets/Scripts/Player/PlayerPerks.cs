using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

namespace Player {
    public class PlayerPerks : MonoBehaviour {
        public int availablePerks = 0;
        public int perkChoices = 1;
        public GameObject perkChoiceUI;
        public Transform HUDTransform;

        GameObject playerObject;
        List<IPerk> perksApplied;
        List<Type> allPerks;
        PerkChoiceContainer[] perkChoiceArray;
        bool keyReset = true;

        void Start () {
            playerObject = transform.gameObject;
            perkChoiceArray = new PerkChoiceContainer[perkChoices];
            for (var c = 0; c < perkChoices; c++) {
                perkChoiceArray[c] = new PerkChoiceContainer();
            }
            perksApplied = new List<IPerk>();

            Assembly a = typeof(IPerk).Assembly;
            allPerks = a.GetTypes()
                .Where(type => type != typeof(IPerk) && typeof(IPerk).IsAssignableFrom(type))
                .ToList();
        }
        
        void Update () {
            if(availablePerks > 0) {
                //check this first to allow at least one update loop to happen after perks are assigned
                //to prevent player holding down a perk button and never seeing the choices
                if (perkChoiceArray[0].Perk != null && keyReset)
                {
                    if (CrossPlatformInputManager.GetButton("Skill1"))
                    {
                        AssignPerk(0);
                    }
                    if (CrossPlatformInputManager.GetButton("Skill2"))
                    {
                        AssignPerk(0);
                    }
                    if (CrossPlatformInputManager.GetButton("Skill3"))
                    {
                        AssignPerk(0);
                    }
                }
                else
                {
                    CreateAvailablePerks();
                }

                if (CrossPlatformInputManager.GetButtonUp("Skill1") ||
                   CrossPlatformInputManager.GetButtonUp("Skill2") ||
                   CrossPlatformInputManager.GetButtonUp("Skill3"))
                {
                    keyReset = true;
                }
            }

        }

        public void GrantPerk() {
            availablePerks++;
        }

        int NextPerkLevel(IPerk perk) {
            var thisKindOfPerk = perksApplied.Where(x => x.GetType() == perk.GetType());
            if (thisKindOfPerk.Any())
            {
                return thisKindOfPerk.Max(x => x.level) + 1;
            }
            return 1;
        }

        void CreateAvailablePerks()
        {
            //set up available perks
            var newPerk = (IPerk)Activator.CreateInstance(allPerks.First(), new object[] { playerObject });

            var newChoiceUi = Instantiate(perkChoiceUI);
            newChoiceUi.transform.SetParent(HUDTransform);
            newChoiceUi.transform.position = new Vector3(-2, 2, 0);
            var rectTransform = newChoiceUi.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(1, 0);
            var image = newChoiceUi.transform.Find("Icon").GetComponent<Image>();
            image.sprite = newPerk.icon;

            var descripText = newChoiceUi.transform.Find("DescriptionText").GetComponent<Text>();
            descripText.text = newPerk.GetDescription(NextPerkLevel(newPerk));

            perkChoiceArray[0].Perk = newPerk;
            perkChoiceArray[0].choiceUI = newChoiceUi;
        }

        void AssignPerk(int choice) {
            keyReset = false;
            var perkChoice = perkChoiceArray[choice];

            perksApplied.Add(perkChoice.Perk.ApplyPerk(NextPerkLevel(perkChoice.Perk)));

            foreach (var perkContainer in perkChoiceArray)
            {
                perkContainer.choiceUI.SetActive(false);
                Destroy(perkContainer.choiceUI);
                perkContainer.Perk = null;
                perkContainer.choiceUI = null;
            }

            availablePerks--;
        }

        private class PerkChoiceContainer {
            public IPerk Perk { get; set; }
            public GameObject choiceUI { get; set; }
        }
    }
}

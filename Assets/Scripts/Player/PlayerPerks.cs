﻿using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

namespace Player {
    public class PlayerPerks : MonoBehaviour {
        public int availablePerks = 0;
        public int perkChoices = 3;
        public GameObject perkChoiceUI;
        public Transform HUDTransform;

        GameObject playerObject;
        List<IPerk> perksApplied;
        List<Type> allPerks;
        PerkChoiceContainer[] perkChoiceArray;

        void Start () {
            playerObject = transform.gameObject;
            perkChoiceArray = new PerkChoiceContainer[perkChoices];
            for (var c = 0; c < perkChoices; c++) {
                perkChoiceArray[c] = new PerkChoiceContainer();
                perkChoiceArray[c].choiceUI = CreatePerkUI(c, perkChoices);
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
                if (perkChoiceArray[0].Perk != null)
                {
                    if (CrossPlatformInputManager.GetButtonDown("Skill1"))
                    {
                        AssignPerk(0);
                    }
                    if (CrossPlatformInputManager.GetButtonDown("Skill2"))
                    {
                        AssignPerk(1);
                    }
                    if (CrossPlatformInputManager.GetButtonDown("Skill3"))
                    {
                        AssignPerk(2);
                    }
                }
                else
                {
                    CreateAvailablePerks();
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
            for (int i = 0; i < perkChoices; i++)
            {
                //Create new perk to save in the collection of perks later on
                var newPerk = (IPerk)Activator.CreateInstance(allPerks.First(), new object[] { playerObject });
                var choiceUI = perkChoiceArray[i].choiceUI;

                var image = choiceUI.transform.Find("Icon").GetComponent<Image>();
                image.sprite = newPerk.icon;

                var descripText = choiceUI.transform.Find("DescriptionText").GetComponent<Text>();
                descripText.text = newPerk.GetDescription(NextPerkLevel(newPerk));

                perkChoiceArray[i].Perk = newPerk;
                choiceUI.SetActive(true);
            }
        }

        GameObject CreatePerkUI(int index, int total)
        {
            var newChoiceUI = Instantiate(perkChoiceUI);
            var rectTransform = newChoiceUI.GetComponent<RectTransform>();
            var width = (int)rectTransform.rect.width;
            int padding = (int)(width * 0.05f);
            int xPosition = 2 + ((padding + width) * (total - index - 1));

            rectTransform.SetParent(HUDTransform);
            rectTransform.anchoredPosition = new Vector2(-xPosition, 2);

            var buttonText = newChoiceUI.transform.Find("ButtonText").GetComponent<Text>();
            //super hacky since there doesn't seem to be an easy way to get what button goes with what control
            switch (index) {
                case 0:
                    buttonText.text = "Z";
                    break;
                case 1:
                    buttonText.text = "X";
                    break;
                case 2:
                    buttonText.text = "C";
                    break;
            }

            newChoiceUI.SetActive(false);

            return newChoiceUI;
        }

        void AssignPerk(int choice) {
            var perkChoice = perkChoiceArray[choice];

            perksApplied.Add(perkChoice.Perk.ApplyPerk(NextPerkLevel(perkChoice.Perk)));

            //disable ui and cleanup
            foreach (var perkContainer in perkChoiceArray)
            {
                perkContainer.choiceUI.SetActive(false);
                perkContainer.Perk = null;
            }

            availablePerks--;
        }

        private class PerkChoiceContainer {
            public IPerk Perk { get; set; }
            public GameObject choiceUI { get; set; }
        }
    }
}
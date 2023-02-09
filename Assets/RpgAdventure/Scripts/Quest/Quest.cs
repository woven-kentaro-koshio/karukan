using System;
using UnityEngine;

namespace RpgAdventure
{
    public enum QuestType
    {
        HUNT,
        GATHER,
        TALK,
        EXPLORE
    }
    [System.Serializable]
    public class Quest
    {
        public string uid;
        public string title;
        public string description;
        public int experience;
        public int gold;

        public int amount;
        public string[] target;

        public string talkTo;
        public Vector3 explor;

        public string questGiver;
        public QuestType Type;
    }
}

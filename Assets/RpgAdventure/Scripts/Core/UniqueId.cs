using UnityEngine;
using System.Collections;
using System;

namespace RpgAdventure
{
    public class UniqueId : MonoBehaviour
    {
        [SerializeField]
        private string uid = Guid.NewGuid().ToString();
    }
}
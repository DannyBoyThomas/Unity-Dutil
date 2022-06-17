using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
namespace Dutil
{
    public class ConditionalShowProperty : PropertyAttribute
    {

        public string condition;

        public ConditionalShowProperty(string condition)
        {
            this.condition = condition;
        }
    }
}
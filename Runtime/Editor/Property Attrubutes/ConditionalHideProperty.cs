using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
namespace Dutil
{
    public class ConditionalHideProperty : PropertyAttribute
    {

        public string condition;

        public ConditionalHideProperty(string condition)
        {
            this.condition = condition;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBots.Core.Model.ApplicationModel
{
    
    public class OBAppInstance
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public OBAppInstance()
        {

        }

        public OBAppInstance(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}

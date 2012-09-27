using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoEngine
{
    public class FullMessage
    {
        private List<Option> fields;
        public List<Option> Fields { get { return fields; } }
        private List<String> fieldNames;
        public List<String> FieldNames
        {
            get
            {
                if (fieldNames == null)
                {
                    fieldNames = new List<String>();
                    foreach (Option opt in Fields)
                        fieldNames.Add(opt.Name);
                }
                return fieldNames;
            }
        }
        private List<String> fieldTypeNames;
        public List<String> FieldTypeNames
        {
            get
            {
                if (fieldTypeNames == null)
                {
                    fieldTypeNames = new List<String>();
                    foreach (Option opt in Fields)
                        fieldTypeNames.Add(opt.TypeName);
                }
                return fieldTypeNames;
            }
        }

        public FullMessage(List<Option> fields)
        {
            this.fields = fields;
        }
    }
}

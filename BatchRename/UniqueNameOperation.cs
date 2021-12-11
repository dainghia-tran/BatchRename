using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class UniqueNameArgs:StringArgs
    {
       
    }

    public class UniqueNameOperation: StringOperation
    {
        public override string Name => "Unique name";

        public override string Description
        {
            get
            {
                return "Rename with unique names using GUID";
            }
        }

        public override string OperateString(string origin)
        {
            Guid g = Guid.NewGuid();
            string result = g.ToString();
            return result;
        }

        public override void OpenDialog()
        {
            throw new Exception();
        }

        public override StringOperation Clone()
        {
            return new UniqueNameOperation();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BatchRename
{
    public class FullnameNormalizeArgs : StringArgs
    {

    }
    public class FullnameNormalizeOperation : StringOperation
    {
        public override string Name => "Fullname normalize";

        public override string Description
        {
            get
            {
                return "Make a standard fullname";
            }
        }

        public override string OperateString(string origin)
        {
            //string result = origin;

            ////Remove space at head
            //for (int i = 0; result[i] == ' ';)
            //    result.Remove(i, 1);

            ////Remove space at tail
            //for (int i = result.Length - 1; result[i] == ' ';)
            //    result.Remove(i, 1);

            //List<int> indexsDeleted = new List<int>();
            //for (int i = 0; i < result.Length;)
            //{
            //    int countSpace = 0;
            //    if (result[i] == ' ')
            //    {
            //        int j = 0;
            //        while (result[i + j] == ' ') j++;
            //        countSpace = j;
            //    }

            //    if (countSpace > 1)
            //    {
            //        indexsDeleted.Add(i);
            //        i += countSpace;
            //    }
            //    else
            //        i++;

            //}

            //foreach (var index in indexsDeleted)
            //    result.Remove(index, 1);

            //// Uppper first Letter
            //string tmp = "";
            //if (result[0] >= 'a' && result[0] <= 'z')
            //{
            //    tmp += (char)('A' + (result[0] - 'a'));
            //}

            //for (int i = 1; i < result.Length; i++)
            //{
            //    if (result[i] >= 'a' && result[i] <= 'z' && result[i - 1] == ' ')
            //        tmp += (char)('A' + (result[i] - 'a'));
            //    else
            //        tmp += result[i];
            //}

            //result = tmp;
            string result = origin;
            result = result.Trim();
            result = String.Join(" ", origin.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries));

            NewCaseStringOperation operation = new NewCaseStringOperation()
            {
                Args = new CaseArgs()
                {
                    Case = "Upper First Letter",
                }
            };
            result = operation.OperateString(result);

            return result;
        }

        

        public override StringOperation Clone()
        {
            return new FullnameNormalizeOperation();
        }

        public override void OpenDialog()
        {
            throw new Exception();
        }
    }
}

using System;

namespace BatchRename
{

    public class MoveArgs : StringArgs
    {
        public string Mode { get; set; }
        public int Number { get; set; }
    }
    public class MoveOperation : StringOperation
    {
        public override string Name => "Move";
 
        public override string Description
        {
            get
            {
                string result = null;
                var arg = Args as MoveArgs;
                result = $"Move {arg.Number} characters to {arg.Mode} of the string";
                return result;
            }
        }

        public MoveOperation()
        {
            Args = new MoveArgs()
            {
                Mode = "Front",
                Number = 0
            };
        }


        public override string OperateString(string origin)
        {
            string result = null;

            switch((Args as MoveArgs).Mode)
            {
                case "Front":
                    {
                        try
                        {
                            result = BringToFront(origin, (Args as MoveArgs).Number);
                            break;
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }
                    }
                    
                    
                case "Back":
                    {
                        try
                        {
                            result = BringToBack(origin, (Args as MoveArgs).Number);
                            break;
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.Message);
                        }
                    }
                   
            }

            return result;
        }

        /// <summary>
        /// move number of characters to the front of the origin string
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string BringToFront(string origin, int num)
        {
            if (num > origin.Length)
            {
                throw new Exception("Substring wanted to move is longer than the string itself");
            }

            string substring = origin.Substring(origin.Length - num, num);
            string result = substring + origin.Substring(0, origin.Length - num);
            return result;
        }

        /// <summary>
        /// move number of characters to the back of the origin string
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string BringToBack(string origin, int num)
        {
            if (num > origin.Length)
            {
                throw new Exception("Substring wanted to move is longer than the string itself");
            }

            string substring = origin.Substring(0, num);
            string result = origin.Substring(num + 1) + substring;
            return result;
        }

        void ChangeMoveArgs(MoveArgs ChangedArgs)
        {
            Args = ChangedArgs;
        }

        public override void OpenDialog()
        {
            var screen = new MoveOperationDialog(Args);
            screen.StringArgsChange += ChangeMoveArgs;
            if (screen.ShowDialog() == true)
            {
                Notify("Description");
            }
        }

        public override StringOperation Clone()
        {
            var oldArgs = Args as MoveArgs;
            return new MoveOperation()
            {
                Args = new MoveArgs()
                {
                    Mode = oldArgs.Mode,
                    Number = oldArgs.Number
                }
            };
        }
    }
}

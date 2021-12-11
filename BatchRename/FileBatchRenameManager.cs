using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BatchRename.UtilsClass;

namespace BatchRename
{
    class BatchRenameError
    {
        public int NameErrorIndex { get; set; }
        public string LastNameValue { get; set; }
        public string Message { get; set; }
    }


    class FileBatchRenameManager
    {
        private List<FileInfo> FileList;
        private List<string> NewFileNames;

        private List<BatchRenameError> errors;
        public int DuplicateMode = 1;

        /// <summary>
        /// create manager to manage String Batch Renaming
        /// </summary>
        /// <param name="StringNames">names wanted to change</param>
        /// <param name="Operations">String operation wanted to perform on input names</param>
        public FileBatchRenameManager()
        {

            errors = new List<BatchRenameError>();
            FileList = new List<FileInfo>();
            NewFileNames = new List<string>();
        }

        private List<string> GetErrorList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < FileList.Count; i++) //fill list with default vaule
            {
                result.Add("None");
            }
            for (int i = 0; i < errors.Count; i++)
            {
                int ErrorIndex = errors[i].NameErrorIndex;
                string Message = errors[i].Message;
                result[ErrorIndex] = Message;
            }
            return result;
        }

        private bool isInErrorList(int index)
        {
            for (int i = 0; i < errors.Count; i++)
            {
                if (index == errors[i].NameErrorIndex)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Perform Batch Rename and return the result
        /// </summary>
        /// <param name="fileList">The list of file wanted to batch rename</param>
        /// <param name="operations">The list of String Operation wanted to perform on name list</param>
        /// <returns>A list of FIleObj</returns>
        public List<FileObj> BatchRename(List<FileObj> fileList, List<StringOperation> operations)
        {
            List<FileObj> result = new List<FileObj>(fileList);
            if (NewFileNames.Count != 0) // clear list to save new changed names
            {
                NewFileNames.Clear();
            }

            if (FileList.Count != 0)
            {
                FileList.Clear();
            }

            for (int i = 0; i < fileList.Count; i++)
            {
                string path = fileList[i].Path + "\\" + fileList[i].Name;
                FileInfo fileInfo = new FileInfo(path);
                FileList.Add(fileInfo);
                NewFileNames.Add(Path.GetFileNameWithoutExtension(fileList[i].Name));
            }



            for (int i = 0; i < operations.Count; i++)
            {

                for (int j = 0; j < NewFileNames.Count; j++)
                {
                    /*If the name is in error list, skip the rename process, to preserve the pre-error value*/
                    bool IsInErrorList = isInErrorList(j);
                    if (IsInErrorList)
                        continue;
                    try
                    {
                        NewFileNames[j] = operations[i].OperateString(NewFileNames[j]); // perform operation
                    }
                    catch (Exception e) //if operation has failed
                    {
                        BatchRenameError error = new BatchRenameError()
                        {
                            NameErrorIndex = j, // save the position of the string which caused the error
                            LastNameValue = NewFileNames[j], //save the last values of the string before error
                            Message = e.Message, //the error message
                        };
                        errors.Add(error);
                    }
                }
            }

            //attach file name with its file extension and error messages that goes along with it if there's one
            List<string> ErrorMessages = GetErrorList();

            for (int i = 0; i < fileList.Count; i++)
            {
                NewFileNames[i] += FileList[i].Extension;
                result[i].NewName = NewFileNames[i];
                result[i].Error = ErrorMessages[i];

            }

            //if handling fails or user refuses to change
            if (handleDuplicateFiles() == false)
            {
                return result;
            };

            for (int i = 0; i < NewFileNames.Count; i++)
            {
                if (result[i].NewName != NewFileNames[i])
                {
                    result[i].NewName = NewFileNames[i];
                    result[i].Error = "Name changed to avoid duplication";
                }
            }
            return result;

        }


        private bool handleDuplicateFiles()
        {
            //List<List<int>> DuplicatePositions = new List<List<int>>();
            //List<String> DuplicateVaules = new List<string>();

            //var duplicateKeys = NewFileNames.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
            //for (int i = 0; i <NewFileNames.Count; i++)
            //{

            //}

            var duplicateKeys = NewFileNames.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
            if (duplicateKeys.Count == 0)
            {
                Debug.WriteLine("No values");
                return true;
            }

            ChangesAlertDialog changesAlertDialog = new ChangesAlertDialog(duplicateKeys);
            if (changesAlertDialog.ShowDialog() != true)
            {
                return false;
            }

            if (DuplicateMode == 1)
            {
                for (int i = 0; i < NewFileNames.Count; i++)
                {
                    int count = 0;
                    bool isDuplicate = true;
                    string newName = NewFileNames[i];

                    //Change duplicated value till it's not the case
                    while (isDuplicate)
                    {
                        isDuplicate = false;

                        //check upper part of the list
                        for (int j = 0; j < i; j++)
                        {
                            if (newName == NewFileNames[j])
                            {
                                isDuplicate = true;
                                count++;
                                newName = Path.GetFileNameWithoutExtension(NewFileNames[i]) + "_" + count.ToString() + FileList[i].Extension;
                            }
                        }

                        //check lower part of the list
                        for (int j = i + 1; j < NewFileNames.Count; j++)
                        {
                            if (newName == NewFileNames[j])
                            {
                                isDuplicate = true;
                                count++;
                                newName = Path.GetFileNameWithoutExtension(NewFileNames[i]) + "_" + count.ToString() + FileList[i].Extension;

                            }
                        }
                    }
                    NewFileNames[i] = newName;
                }
            }
            else
            {
                bool StillDuplicate = true;
                while (StillDuplicate)
                {
                    StillDuplicate = false;
                    for (int i = 0; i < NewFileNames.Count; i++)
                    {
                         //check upper part of the list
                        for (int j = 0; j < i; j++)
                        {
                            if (NewFileNames[i] == NewFileNames[j])
                            {
                                StillDuplicate = true;
                                NewFileNames[i] = FileList[i].Name;
                            }
                        }

                        //check lower part of the list
                        for (int j = i + 1; j < NewFileNames.Count; j++)
                        {
                            if (NewFileNames[i] == NewFileNames[j])
                            {
                                StillDuplicate = true;
                                NewFileNames[j] = FileList[j].Name;
                            }
                        }
                    }
                }
            }
            return true;
            
        }


        public void CommitChange()
        {

            for (int i = 0; i < FileList.Count; i++)
            {
                try
                {
                    string newPath = FileList[i].Directory + "\\" + NewFileNames[i];
                    if (newPath != FileList[i].FullName)
                        FileList[i].MoveTo(newPath);
                }
                catch
                {
                    throw new Exception("path changed");
                }
                
            }

        }

        //public List<string> StartBatching(string[] names, List<StringOperation> operations)
        //{
        //    NameCount = names.Length;
        //    List<string> result = new List<string>(names);

        //    for (int i = 0; i < operations.Count; i++)
        //    {

        //        for (int j = 0; j < result.Count; j++)
        //        {
        //            try
        //            {
        //                result[j] = operations[i].OperateString(result[j]); // perform operation
        //            }
        //            catch (Exception e) //if operation has failed
        //            {
        //                BatchRenameError error = new BatchRenameError()
        //                {
        //                    NameErrorIndex = j, // save the position of the string which caused the error
        //                    LastNameValue = names[j], //save the last values of the string before error
        //                    Message = e.Message, //the error message
        //                };
        //                errors.Add(error);
        //                result.RemoveAt(j); //remove the string from the Batch reanaming list so that it can't be changed by later Operations
        //                j--;
        //            }
        //        }
        //    }
        //    //add the faulty names back to the list
        //    for (int i = 0; i < errors.Count; i++)
        //    {
        //        string ErrorString = errors[i].LastNameValue;
        //        int index = errors[i].NameErrorIndex;
        //        result.Insert(index, ErrorString);
        //    }
        //    return result;
        //}


    }

    class Preset
    {
        public string Name { get; set; }
        public BindingList<StringOperation> stringOperations { get; set; }
        public Preset()
        {
            Name = "";
            stringOperations = new BindingList<StringOperation>();
        }
    }


}

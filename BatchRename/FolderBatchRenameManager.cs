using BatchRename.UtilsClass;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BatchRename
{
    class FolderBatchRenameManager
    {

        private List<DirectoryInfo> FolderList;
        private List<string> NewFolderNames;

        private List<BatchRenameError> errors;
        public int DuplicateMode = 1;

        /// <summary>
        /// create manager to manage String Batch Renaming
        /// </summary>
        /// <param name="StringNames">names wanted to change</param>
        /// <param name="Operations">String operation wanted to perform on input names</param>
        public FolderBatchRenameManager()
        {

            errors = new List<BatchRenameError>();
            FolderList = new List<DirectoryInfo>();
            NewFolderNames = new List<string>();
        }

        private List<string> GetErrorList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < FolderList.Count; i++) //fill list with default vaule
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
        /// <param name="folderList">The list of folder wanted to batch rename</param>
        /// <param name="operations">The list of String Operation wanted to perform on name list</param>
        /// <returns>A list of FolderObj</returns>
        public List<FolderObj> BatchRename(List<FolderObj> folderList, List<StringOperation> operations)
        {
            List<FolderObj> result = new List<FolderObj>(folderList);
            if (NewFolderNames.Count != 0) // clear list to save new changed names
            {
                NewFolderNames.Clear();
            }

            if (FolderList.Count != 0)
            {
                FolderList.Clear();
            }

            for (int i = 0; i < folderList.Count; i++)
            {
                string path = folderList[i].Path + "\\" + folderList[i].Name;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FolderList.Add(directoryInfo);
                NewFolderNames.Add(directoryInfo.Name);
                Debug.WriteLine(directoryInfo.Name);
            }



            for (int i = 0; i < operations.Count; i++)
            {

                for (int j = 0; j < NewFolderNames.Count; j++)
                {
                    /*If the name is in error list, skip the rename process, to preserve the pre-error value*/
                    bool IsInErrorList = isInErrorList(j);
                    if (IsInErrorList)
                        continue;
                    try
                    {
                        NewFolderNames[j] = operations[i].OperateString(NewFolderNames[j]); // perform operation
                    }
                    catch (Exception e) //if operation has failed
                    {
                        BatchRenameError error = new BatchRenameError()
                        {
                            NameErrorIndex = j, // save the position of the string which caused the error
                            LastNameValue = NewFolderNames[j], //save the last values of the string before error
                            Message = e.Message, //the error message
                        };
                        errors.Add(error);
                    }
                }
            }

            //send back error messages that goes along with the folder name if there's one
            List<string> ErrorMessages = GetErrorList();

            for (int i = 0; i < folderList.Count; i++)
            {
                result[i].NewName = NewFolderNames[i];
                result[i].Error = ErrorMessages[i];

            }

            //if handling fails or user refuses to change
            if (handleDuplicateFolder() == false)
            {
                return result;
            };

            for (int i = 0; i < NewFolderNames.Count; i++)
            {
                if (result[i].NewName != NewFolderNames[i])
                {
                    result[i].NewName = NewFolderNames[i];
                    result[i].Error = "Name changed to avoid duplication";
                }
            }

            return result;
        }

        private bool handleDuplicateFolder()
        {
            //List<List<int>> DuplicatePositions = new List<List<int>>();
            //List<String> DuplicateVaules = new List<string>();

            //var duplicateKeys = NewFolderNames.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
            //for (int i = 0; i <NewFolderNames.Count; i++)
            //{

            //}

            var duplicateKeys = NewFolderNames.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
            if (duplicateKeys.Count == 0)
            {
                Debug.WriteLine("No values");
                return true;
            }

            //show duplicated names
            ChangesAlertDialog changesAlertDialog = new ChangesAlertDialog(duplicateKeys);
            if (changesAlertDialog.ShowDialog() != true)
            {
                return false;
            }

            if (DuplicateMode == 1)
            {
                for (int i = 0; i < NewFolderNames.Count; i++)
                {
                    int count = 0;
                    bool isDuplicate = true;
                    string newName = NewFolderNames[i];

                    //Change duplicated value till it's not the case
                    while (isDuplicate)
                    {
                        isDuplicate = false;

                        //check upper part of the list
                        for (int j = 0; j < i; j++)
                        {
                            if (newName == NewFolderNames[j])
                            {
                                isDuplicate = true;
                                count++;
                                newName = NewFolderNames[i] + "_" + count.ToString();
                            }
                        }

                        //check lower part of the list
                        for (int j = i + 1; j < NewFolderNames.Count; j++)
                        {
                            if (newName == NewFolderNames[j])
                            {
                                isDuplicate = true;
                                count++;
                                newName = NewFolderNames[i] + "_" + count.ToString();
                            }
                        }
                    }
                    NewFolderNames[i] = newName;
                }
            }
            else
            {
                bool StillDuplicate = true;
                while (StillDuplicate)
                {
                    StillDuplicate = false;
                    for (int i = 0; i < NewFolderNames.Count; i++)
                    {
                        //check upper part of the list
                        for (int j = 0; j < i; j++)
                        {
                            if (NewFolderNames[i] == NewFolderNames[j])
                            {
                                StillDuplicate = true;
                                NewFolderNames[i] = FolderList[i].Name;
                            }
                        }

                        //check lower part of the list
                        for (int j = i + 1; j < NewFolderNames.Count; j++)
                        {
                            if (NewFolderNames[i] == NewFolderNames[j])
                            {
                                StillDuplicate = true;
                                NewFolderNames[j] = FolderList[j].Name;
                            }
                        }
                    }
                }
            }

            
            return true;
        }


        public void CommitChange()
        {

            for (int i = 0; i < FolderList.Count; i++)
            {
                try
                {
                    string newPath = FolderList[i].Parent.FullName + "\\" + NewFolderNames[i];
                    if (newPath != FolderList[i].FullName)
                    {
                        //little gimmick
                        //windows folder file are case insentitive, so can't name it if NewCase Operation is called
                        //so we change folder's name to something different then change it back to the  right name
                        Guid g = Guid.NewGuid();
                        string temp = g.ToString();
                        string tempPath = FolderList[i].Parent.FullName + "\\" + temp; // change it to something diffirent than old name
                        FolderList[i].MoveTo(tempPath);
                        FolderList[i] = new DirectoryInfo(tempPath);
                        FolderList[i].MoveTo(newPath); //change it to the right new name
                    }
                        
                }
                catch
                {
                    throw new Exception("path changed");
                }
                
            }


        }
    }
}

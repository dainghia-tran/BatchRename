using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;


using BatchRename.UtilsClass;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BindingList<FileObj> filesList;
        private BindingList<FolderObj> foldersList;
        private List<StringOperationPrototype> addMethodPrototypes;
        public BindingList<StringOperation> operationsList;

        public BindingList<StringOperation> FileOperationsList;
        public BindingList<StringOperation> FolderOperationsList;

        private FileBatchRenameManager fileRenameManager;
        private FolderBatchRenameManager folderRenameManager;

        private BackgroundWorker fetchFilesWorker;
        private BackgroundWorker excludeFilesWorker;
        private BackgroundWorker fetchFoldersWorker;
        private BackgroundWorker excludeFoldersWorker;

        private BindingList<Preset> loadedPresets; //Use for back up preset loaded

        public MainWindow()
        {
            InitializeComponent();

            loadedPresets = new BindingList<Preset>();

            filesList = new BindingList<FileObj>();
            foldersList = new BindingList<FolderObj>();

            
            FileOperationsList = new BindingList<StringOperation>();
            FolderOperationsList = new BindingList<StringOperation>();
            operationsList = FileOperationsList;//new BindingList<StringOperation>();

            fileRenameManager = new FileBatchRenameManager();
            folderRenameManager = new FolderBatchRenameManager();

            //Populate prototypes
            addMethodPrototypes = new List<StringOperationPrototype>
            {
                new StringOperationPrototype(new ReplaceOperation(), this),
                new StringOperationPrototype(new NewCaseStringOperation(), this),
                new StringOperationPrototype(new MoveOperation(), this),
                new StringOperationPrototype(new FullnameNormalizeOperation(), this),
                new StringOperationPrototype(new UniqueNameOperation(), this)
            };

            //Bind
            RenameFilesList.ItemsSource = filesList;
            RenameFoldersList.ItemsSource = foldersList;
            AddMethodButton.ContextMenu.ItemsSource = addMethodPrototypes;
            OperationsList.ItemsSource = operationsList;
            PresetsList.ItemsSource = loadedPresets;

            //Create fetch files worker to invoke on click
            fetchFilesWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            fetchFilesWorker.DoWork += FetchFiles_DoWork;
            fetchFilesWorker.ProgressChanged += ProgressChanged;
            fetchFilesWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Create exclude files worker to invoke on click
            excludeFilesWorker = new BackgroundWorker {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            excludeFilesWorker.DoWork += ExcludeFiles_DoWork;
            excludeFilesWorker.ProgressChanged += ProgressChanged;
            excludeFilesWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Create fetch folders worker to invoke on click
            fetchFoldersWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            fetchFoldersWorker.DoWork += FetchFolders_DoWork;
            fetchFoldersWorker.ProgressChanged += ProgressChanged;
            fetchFoldersWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Create exclude folders worker to invoke on click
            excludeFoldersWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            excludeFoldersWorker.DoWork += ExcludeFolders_DoWork;
            excludeFoldersWorker.ProgressChanged += ProgressChanged;
            excludeFoldersWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Init UI Utilities down here
            RenameFilesList.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(Thumb_DragDelta), true);
            RenameFoldersList.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler(Thumb_DragDelta), true);

        }

        //------------------------------Background Workers---------------------------------

        private void DisableLoadingViews()
        {
            AddFileButton.IsEnabled = false;
            ExcludeFileButton.IsEnabled = false;
            AddFolderButton.IsEnabled = false;
            ExcludeFolderButton.IsEnabled = false;
            StartButton.IsEnabled = false;
        }

        private void EnableLoadingViews()
        {
            AddFileButton.IsEnabled = true;
            ExcludeFileButton.IsEnabled = true;
            AddFolderButton.IsEnabled = true;
            ExcludeFolderButton.IsEnabled = true;
            StartButton.IsEnabled = true;
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadingBar.Value = 100;
            Mouse.OverrideCursor = null;
            LoadingOutput.Text = "Action completed";

            EnableLoadingViews();
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadingBar.Value = e.ProgressPercentage;
            if(e.UserState != null)
                LoadingOutput.Text = (string)e.UserState;
        }

        private void FetchFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = (string)e.Argument + "\\";
            var children = Directory.GetFiles(path);
            StringBuilder output = new StringBuilder();

            for (int child = 0; child < children.Length; child++)
            {
                bool isDuplicated = false;
                string childName = children[child].Remove(0, path.Length);

                //Check duplicates
                for (int i = 0; i < filesList.Count; i++)
                {
                    if (filesList[i].Name.Equals(childName) && filesList[i].Path.Equals(path))
                    {
                        isDuplicated = true;
                        break;
                    }
                }

                output.Clear();
                string result = "Skip duplicate ";
                if (!isDuplicated) { 
                    result = "Add ";
                    Dispatcher.Invoke(() =>
                    {
                        filesList.Add(new FileObj() { Name = childName, Path = path, });
                    });
                }
                output.Append(result);
                output.Append(path);
                output.Append(childName);
                
                fetchFilesWorker.ReportProgress((child * 100/children.Length), output.ToString());
            }
        }

        private void ExcludeFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            var items = ((IList<object>)e.Argument).Cast<FileObj>().ToList();

            int amount = items.Count;

            StringBuilder output = new StringBuilder();

            for (int item = 0; item < amount; item++)
            {
                Dispatcher.Invoke(()=> {
                    filesList.Remove(items[item]);
                });
                output.Clear();
                output.Append("Remove ");
                output.Append(items[item].Path + items[item].Name);
                excludeFilesWorker.ReportProgress((item * 100 / amount), output.ToString());
            }
        }

        private void FetchFolders_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = (string)e.Argument + "\\";
            var children = Directory.GetDirectories(path);
            StringBuilder output = new StringBuilder();

            for (int child = 0; child < children.Length; child++)
            {
                bool isDuplicated = false;
                string childName = children[child].Remove(0, path.Length);

                //Check duplicates
                for (int i = 0; i < foldersList.Count; i++)
                {
                    if (foldersList[i].Name.Equals(childName) && foldersList[i].Path.Equals(path))
                    {
                        isDuplicated = true;
                        break;
                    }
                }

                output.Clear();
                string result = "Skip duplicate ";
                if (!isDuplicated)
                {
                    result = "Add ";
                    Dispatcher.Invoke(() =>
                    {
                        foldersList.Add(new FolderObj() { Name = childName, Path = path });
                    });
                }
                output.Append(result);
                output.Append(path);
                output.Append(childName);

                fetchFoldersWorker.ReportProgress((child * 100 / children.Length), output.ToString());
            }
        }

        private void ExcludeFolders_DoWork(object sender, DoWorkEventArgs e)
        {
            var items = ((IList<object>)e.Argument).Cast<FolderObj>().ToList();

            int amount = items.Count;

            StringBuilder output = new StringBuilder();

            for (int item = 0; item < amount; item++)
            {
                Dispatcher.Invoke(() => {
                    foldersList.Remove(items[item]);
                });
                output.Clear();
                output.Append("Remove ");
                output.Append(items[item].Path + items[item].Name);
                excludeFoldersWorker.ReportProgress((item * 100 / amount), output.ToString());
            }
        }

        //-------------------------Button Clicks Implements---------------------------------

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            LoadingBar.Value = 0;
            LoadingOutput.Text = "";
            filesList.Clear();
            foldersList.Clear();
            Mouse.OverrideCursor = null;
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            int duplicateOption = fileRenameManager.DuplicateMode;
            BatchRenameOptionsDialog dialog = new BatchRenameOptionsDialog(duplicateOption);
            if (dialog.ShowDialog() == true)
            {
                fileRenameManager.DuplicateMode = dialog.DuplicateOption;
                folderRenameManager.DuplicateMode = dialog.DuplicateOption;
            }
        }

        private void AddMethodButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement addButton)
            {
                addButton.ContextMenu.PlacementTarget = addButton;
                addButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                addButton.ContextMenu.MinWidth = addButton.ActualWidth;
                addButton.ContextMenu.MinHeight = 30;
                addButton.ContextMenu.Margin = new Thickness(0,5,0,0);
                addButton.ContextMenu.IsOpen = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(RenameTabControl.SelectedItem as TabItem).Header == "Rename Files")
            {
                List<FileObj> inputList = new List<FileObj>(filesList);
                if (inputList.Count == 0)
                    return;

                List<StringOperation> inputOperations = new List<StringOperation>(operationsList);
                if (operationsList.Count == 0)
                {
                    return;
                }
                PreviewButton_Click(sender, e);
                try
                {
                    fileRenameManager.CommitChange();
                }
                catch
                {
                    FilePathChangedDialog dialog = new FilePathChangedDialog();
                    dialog.ShowDialog();
                }
                
            }

            //if Tab "Rename folders" is selected, rename folders
            else
            {
                List<FolderObj> inputList = new List<FolderObj>(foldersList);
                if (inputList.Count == 0)
                    return;

                List<StringOperation> inputOperations = new List<StringOperation>(operationsList);
                if (operationsList.Count == 0)
                {
                    return;
                }
                PreviewButton_Click(sender, e);
                folderRenameManager.CommitChange();
                try
                {
                    
                }
                catch
                {
                    FilePathChangedDialog dialog = new FilePathChangedDialog();
                    dialog.ShowDialog();
                }
            }
            FinishedRenameDialog notiDialog = new FinishedRenameDialog();
            notiDialog.ShowDialog();
            RefreshButton_Click(sender, e);
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Open";
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.RestoreDirectory = true;
            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var reader = new StreamReader(openFileDialog.FileName);
                Preset preset = new Preset();
                var tokens2 = openFileDialog.FileName.Split(new string[] { "\\" }, StringSplitOptions.None);
                var tokens3 = tokens2[tokens2.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
                preset.Name = tokens3[0];
                if (reader.ReadLine() == "BatchRename")
                {
                    var n = int.Parse(reader.ReadLine());
                    operationsList.Clear();
                    for (int i = 1; i <= n; i++)
                    {
                        var tokens = reader.ReadLine().Split(new string[] { " - " }, StringSplitOptions.None);
                        StringOperation operation;
                        switch (tokens[0])
                        {
                            case "Replace":
                                operation = new ReplaceOperation
                                {
                                    Args = new ReplaceArgs
                                    {
                                        From = tokens[1],
                                        To = tokens[2]
                                    }
                                };
                                operationsList.Add(operation);
                                preset.stringOperations.Add(operation.Clone());
                                break;
                            case "Change Case":
                                operation = new NewCaseStringOperation
                                {
                                    Args = new CaseArgs
                                    {
                                        Case = tokens[1]
                                    }
                                };
                                operationsList.Add(operation);
                                preset.stringOperations.Add(operation.Clone());
                                break;
                            case "Move":
                                operation = new MoveOperation
                                {
                                    Args = new MoveArgs
                                    {
                                        Mode = tokens[1],
                                        Number = int.Parse(tokens[2])
                                    }
                                };
                                operationsList.Add(operation);
                                preset.stringOperations.Add(operation.Clone());
                                break;
                            case "Fullname normalize":
                                operationsList.Add(new FullnameNormalizeOperation());
                                preset.stringOperations.Add(new FullnameNormalizeOperation());
                                break;
                            case "Unique name":
                                operationsList.Add(new UniqueNameOperation());
                                preset.stringOperations.Add(new UniqueNameOperation());
                                break;
                        }
                    }
                    loadedPresets.Add(preset);
                    PresetsList.SelectedIndex = loadedPresets.Count() -1;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("This txt file is not BatchRename's data");
                }
                reader.Close();
                
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = @"C:\";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var writer = new StreamWriter(saveFileDialog.FileName);
                writer.WriteLine("BatchRename");
                writer.WriteLine(operationsList.Count);
                foreach(var StringOperation in operationsList)
                {
                    if (StringOperation.Name == "Replace") {
                        string From = (StringOperation.Args as ReplaceArgs).From;
                        string To = (StringOperation.Args as ReplaceArgs).To;
                        writer.WriteLine($"{StringOperation.Name} - {From} - {To}");
                    }
                    else if (StringOperation.Name == "Move")
                    {
                        string Mode = (StringOperation.Args as MoveArgs).Mode;
                        int Number = (StringOperation.Args as MoveArgs).Number;
                        writer.WriteLine($"{StringOperation.Name} - {Mode} - {Number}");
                    }
                    else if (StringOperation.Name == "Change Case")
                    {
                        string Case = (StringOperation.Args as CaseArgs).Case;
                        writer.WriteLine($"{StringOperation.Name} - {Case} - ");
                    } else
                    {
                        writer.WriteLine($"{StringOperation.Name} - ");
                    }
                }
                writer.Close();
            }
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy 
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy) return;
            var dialog = new FolderBrowserDialog();

            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                DisableLoadingViews();
                LoadingBar.Value = 0;
                fetchFilesWorker.RunWorkerAsync(dialog.SelectedPath);
            }

        }

        private void ExcludeFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy 
                || filesList.Count <= 0) return;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            DisableLoadingViews();
            LoadingBar.Value = 0;
            excludeFilesWorker.RunWorkerAsync(RenameFilesList.SelectedItems);
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy) return;
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                DisableLoadingViews();
                LoadingBar.Value = 0;
                fetchFoldersWorker.RunWorkerAsync(dialog.SelectedPath);
            }
        }

        private void ExcludeFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy
                || foldersList.Count <= 0) return;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            DisableLoadingViews();
            LoadingBar.Value = 0;
            excludeFoldersWorker.RunWorkerAsync(RenameFoldersList.SelectedItems);
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = OperationsList.SelectedItem as StringOperation;
            try
            {
                item.OpenDialog();
            }
            catch
            {
                var screen = new NoEditAvailableDialog();
                screen.ShowDialog();
            }
            
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            StringOperation local = ((sender as System.Windows.Controls.Button).Tag as StringOperation);
            operationsList.Remove(local);
        }

        private void RenameTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)(RenameTabControl.SelectedItem as TabItem).Header == "Rename Files")
            {
                operationsList = FileOperationsList;
                OperationsList.ItemsSource = operationsList;
                
                //FolderOperationsList = new List<StringOperation>(operationsList);
                //operationsList = new BindingList<StringOperation>(FileOperationsList);
                //OperationsList.ItemsSource = operationsList;


                //operationsList.Clear();

                //for (int i = 0; i < FileOperationsList.Count; i++)
                //{
                //    operationsList.Add(FileOperationsList[i]);
                //}
                
            }
            if ((string)(RenameTabControl.SelectedItem as TabItem).Header == "Rename Folders")
            {
                operationsList = FolderOperationsList;
                OperationsList.ItemsSource = operationsList;

                //FileOperationsList = new List<StringOperation>(operationsList);
                //operationsList = new BindingList<StringOperation>(FolderOperationsList);
                //OperationsList.ItemsSource = operationsList;

                //operationsList.Clear();
                //for (int i = 0; i < FolderOperationsList.Count; i++)
                //{
                //    operationsList.Add(FolderOperationsList[i]);
                //}
            }
        }


        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            // if Tab "Rename Files" is selected, perform rename on file
            if ((string)(RenameTabControl.SelectedItem as TabItem).Header == "Rename Files")
            {
                List<FileObj> inputList = new List<FileObj>(filesList);
                if (inputList.Count == 0)
                    return;

                List<StringOperation> inputOperations = new List<StringOperation>(operationsList);
                if (operationsList.Count == 0)
                {
                    return;
                }

                List<FileObj> result = fileRenameManager.BatchRename(inputList, inputOperations);
                filesList.Clear();
                filesList = new BindingList<FileObj>(result);
                RenameFilesList.ItemsSource = filesList;
            }

            //if Tab "Rename folders" is selected, rename folders
            else
            {
                List<FolderObj> inputList = new List<FolderObj>(foldersList);
                if (inputList.Count == 0)
                    return;

                List<StringOperation> inputOperations = new List<StringOperation>(operationsList);
                if (operationsList.Count == 0)
                {
                    return;
                }

                List<FolderObj> result = folderRenameManager.BatchRename(inputList, inputOperations);
                foldersList.Clear();
                foldersList = new BindingList<FolderObj>(result);
                RenameFoldersList.ItemsSource = foldersList;
            }
            
        }

        private void PresetsList_DropDownClosed(object sender, EventArgs e)
        {
            var action = PresetsList.SelectedItem as Preset;
            if (action != null)
            {
                operationsList.Clear();
                foreach (var stringOperation in action.stringOperations)
                {
                    operationsList.Add(stringOperation.Clone());
                }
            }
        }
        //--------------------------Utilities--------------------------
        void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb senderAsThumb = e.OriginalSource as Thumb;
            GridViewColumnHeader header
                = senderAsThumb?.TemplatedParent as GridViewColumnHeader;
            if (header?.Column.ActualWidth < header?.MinWidth)
            {
                header.Column.Width = header.MinWidth;
            }
        }

        private void DownBottomButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OperationsList.SelectedItem as StringOperation;
            if (item != null)
            {
                operationsList.Remove(item);
                operationsList.Add(item);
                OperationsList.SelectedIndex = operationsList.Count - 1;
            }
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OperationsList.SelectedItem as StringOperation;
            if (item != null)
            {
                int index = operationsList.IndexOf(item);
                if (index < operationsList.Count - 1)
                {
                    operationsList.RemoveAt(index);
                    operationsList.Insert(index + 1, item);
                    OperationsList.SelectedIndex = index + 1;
                }
            }
        }

        private void UpTopButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OperationsList.SelectedItem as StringOperation;
            if (item != null)
            {
                operationsList.Remove(item);
                operationsList.Insert(0, item);
                OperationsList.SelectedIndex = 0;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            var item = OperationsList.SelectedItem as StringOperation;
            if (item != null)
            {
                int index = operationsList.IndexOf(item);
                if (index > 0)
                {
                    operationsList.RemoveAt(index);
                    operationsList.Insert(index - 1, item);
                    OperationsList.SelectedIndex = index - 1;
                }
            }
        }
    }
}

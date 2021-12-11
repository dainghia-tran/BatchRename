using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename.UtilsClass
{
    class FileObj : INotifyPropertyChanged
    {
        private string _name, _newName, _path, _error;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Notify("Name");
            }
        }
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                Notify("NewName");
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Notify("Path");
            }
        }
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                Notify("Error");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string attrib)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(attrib));
        }
    }

    class FolderObj : INotifyPropertyChanged
    {
        private string _name, _newName, _path, _error;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Notify("Name");
            }
        }
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                Notify("NewName");
            }
        }
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Notify("Path");
            }
        }
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                Notify("Error");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string attrib)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(attrib));
        }
    }
}

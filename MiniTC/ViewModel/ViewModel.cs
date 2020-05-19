using MiniTC.Model;
using MiniTC.ViewModel.BaseClass;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;


namespace MiniTC.ViewModel
{
    class ViewModel: ViewModelBase
    {
        TCModel Model = new TCModel();
        public ViewModel()
        {
            LeftDrive = -1;
            RightDrive = -1;
            PathLeft = "";
            PathRight = "";
        }

        #region Properites
        public string PathLeft
        {
            get
            {
                return Model.CurrentPath[0];
            }
            set
            {
                Model.CurrentPath[0] = value;
            }
        }

        public string PathRight
        {
            get
            {
                return Model.CurrentPath[1];
            }
            set
            {
                Model.CurrentPath[1] = value;
            }
        }

        public string[] Drives
        {
            get
            {
                return Model.Drives;
            }
        }

        public List<string> FilesLeft
        {
            get
            {
                List<string> files = Model.Directories[0];
                files.AddRange(Model.Files[0]);
                return files;
            }

        }

        public List<string> FilesRight
        {
            get
            {
                List<string> files = Model.Directories[1];
                files.AddRange(Model.Files[1]);
                return files;
            }
        }

        public int LeftDrive
        {
            get
            {
                return Model.Drive[0];
            }
            set
            {
                Model.Drive[0] = value;
            }
        }

        public int RightDrive
        {
            get
            {
                return Model.Drive[1];
            }
            set
            {
                Model.Drive[1] = value;
            }
        }

        public int CurrentLeft { get; set; } = -1;

        public int CurrentRight { get; set; } = -1;

        public int LastSelected { get; set; } = -1;
        #endregion

        #region Commands

        private ICommand leftDriveChanged = null;
        public ICommand LeftDriveChanged
        {
            get
            {
                if (leftDriveChanged == null)
                    leftDriveChanged = new RelayCommand(
                        arg => { Model.DirectoryChanged(Drives[LeftDrive],0); onPropertyChanged(nameof(FilesLeft), nameof(PathLeft)); },
                        arg => true
            );
                return leftDriveChanged;
            }
        }

        private ICommand rightDriveChanged = null;
        public ICommand RightDriveChanged
        {
            get
            {
                if (rightDriveChanged == null)
                    rightDriveChanged = new RelayCommand(
                        arg => { Model.DirectoryChanged(Drives[RightDrive], 1); onPropertyChanged(nameof(FilesRight), nameof(PathRight)); },
                        arg => true
            );
                return rightDriveChanged;

            }
        }

        private ICommand leftDirectoryChanged = null;
        public ICommand LeftDirectoryChanged
        {
            get
            {
                if (leftDirectoryChanged == null)
                    leftDirectoryChanged = new RelayCommand(
                        arg => {
                            string current = FilesLeft[CurrentLeft];
                            if (current == "..")
                            {
                                Model.DirectoryChanged(Path.GetDirectoryName(PathLeft), 0);
                                CurrentLeft = -1;
                                onPropertyChanged(nameof(FilesLeft), nameof(PathLeft), nameof(CurrentLeft));
                                if (LastSelected == 0)
                                    LastSelected = -1;
                            }
                            else if (Directory.Exists(PathLeft + @"\" + current.Substring(3)))
                            {
                                Model.DirectoryChanged(PathLeft + @"\" + current.Substring(3), 0);
                                CurrentLeft = -1;
                                onPropertyChanged(nameof(FilesLeft), nameof(PathLeft), nameof(CurrentLeft));
                                if (LastSelected == 0)
                                    LastSelected = -1;
                            }
                            else LastSelected = 0;
                        },
                        arg => CurrentLeft >= 0
            );
                return leftDirectoryChanged;

            }
        }

        private ICommand rightDirectoryChanged = null;
        public ICommand RightDirectoryChanged
        {
            get
            {
                if (rightDirectoryChanged == null)
                    rightDirectoryChanged = new RelayCommand(
                        arg => {
                            string current = FilesRight[CurrentRight];
                            if (current == "..")
                            {
                                Model.DirectoryChanged(Path.GetDirectoryName(PathRight), 1);
                                CurrentRight = -1;
                                onPropertyChanged(nameof(FilesRight), nameof(PathRight), nameof(CurrentRight));
                                if (LastSelected == 1)
                                    LastSelected = -1;
                            }
                            else if (Directory.Exists(PathRight + @"\" + current.Substring(3)))
                            {
                                Model.DirectoryChanged(PathRight + @"\" + current.Substring(3), 1);
                                CurrentRight = -1;
                                onPropertyChanged(nameof(FilesRight), nameof(PathRight), nameof(CurrentRight));
                                if (LastSelected == 1)
                                    LastSelected = -1;
                            }
                            else LastSelected = 1;
                        },
                        arg => CurrentRight >= 0
            );
                return rightDirectoryChanged;

            }
        }

        private ICommand copy = null;
        public ICommand Copy
        {
            get
            {
                if (copy == null)
                    copy = new RelayCommand(
                        arg => {
                            if (LastSelected == 0)
                            {
                                Model.CopyFile(PathLeft + @"\" + FilesLeft[CurrentLeft], PathRight + @"\" + FilesLeft[CurrentLeft]);
                                Model.DirectoryChanged(PathRight, 1);
                            }
                            else
                            {
                                Model.CopyFile(PathRight + @"\" + FilesRight[CurrentRight], PathLeft + @"\" + FilesRight[CurrentRight]);
                                Model.DirectoryChanged(PathLeft, 0);
                            }

                            onPropertyChanged(nameof(FilesRight), nameof(FilesLeft));
                        },
                        arg => LastSelected != -1 && PathLeft.Length > 0 && PathRight.Length > 0
            ) ;
                return copy;

            }
        }
        #endregion
    }
}

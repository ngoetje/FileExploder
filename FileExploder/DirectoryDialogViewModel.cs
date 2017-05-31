using System.Windows.Input;
using System;
using FileExploder.Ui.Infrastructure;
using FileExploder.Model;

namespace FileExploder
{
    public class DirectoryDialogViewModel : ViewModelBase
    {
        public DirectoryDialogViewModel(DirectoryDialog dialog, CrawlerDirectory directory)
        {
            this.directoryDialog = dialog;

            this.Directory = directory;
            this.OkCommand = new RelayCommand(p => true,
                                              p => {
                                                    this.IsCancelled = false;
                                                    this.directoryDialog.Close();
                                                    }
                                              );
            this.CancelCommand = new RelayCommand(p => true,
                                                  p => {
                                                      this.IsCancelled = true;
                                                      this.directoryDialog.Close();
                                                  }
                                              );
            this.directoryDialog.DataContext = this;
        }

        public DirectoryDialogViewModel(DirectoryDialog dialog) : this(dialog, new CrawlerDirectory())
        {
            
        }

        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private CrawlerDirectory directory = new CrawlerDirectory();
        private DirectoryDialog directoryDialog;

        public CrawlerDirectory Directory
        {
            get { return directory; }
            set
            {
                directory = value;
                RaisePropertyChanged("Directory");
            }
        }

        internal void ShowDialog()
        {
            this.directoryDialog.ShowDialog();
        }

        public Boolean IsCancelled { get; set; } = false;
    }
}
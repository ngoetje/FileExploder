using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows;
using FileExploder.Ui.Infrastructure;
using FileExploder.Model;
using FileExploder.Bl;

namespace FileExploder
{

    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(MainWindow window)
        {            
            this.mainWindow = window;
            this.mainWindow.Closing += OnClosing;
        
            InitializeCommands();
            ParseDirectoryFile();            
        }

        private void CrawlerResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileExploder.Bl.FileHandler.SaveDirectories(this.Entries.Keys);
        }

        private void InitializeCommands()
        {
            this.AddDirectoryCommand = new RelayCommand(p => true, p => AddDirectory());
            this.RemoveDirectoryCommand = new RelayCommand(p => CanRemoveDirectory(), p => RemoveDirectory());
            this.RefreshDirectoryCommand = new RelayCommand(p => CanRefreshDirectory(), p => RefreshDirectory());
            this.OpenExplorerCommand = new RelayCommand(p => true, p => OpenExplorer(p));
        }

        private void OpenExplorer(object p)
        {
            var result = p as CrawlerResult;
            if (result != null)
            {
                if (this.SelectedCrawlerResult != result) this.SelectedCrawlerResult = result;
                Process.Start(result.Directory.FullName);
            }
        }

        private async void RefreshDirectory()
        {
            this.Entries[this.SelectedDirectory].Clear();
            this.CrawlerResults.Clear();
            this.SelectedCrawlerResult = null;

            Loading = Visibility.Visible;

            var result = await FileCrawler.CrawlAsync(this.SelectedDirectory);            

            foreach (var item in result)
            {
                this.Entries[this.SelectedDirectory].Add(item);
            }            

            UpdateCrawlerResults(this.Entries[this.SelectedDirectory]);

            Loading = Visibility.Hidden;
        }

        private bool CanRefreshDirectory()
        {
            return this.SelectedDirectory != null;
        }

        private void RemoveDirectory()
        {
            this.Entries.Remove(this.SelectedDirectory); 
            if (this.Entries.Any())
            {
                this.SelectedDirectory = this.Entries.Keys.First();
            }
        }

        private bool CanRemoveDirectory()
        {
            return this.SelectedDirectory != null;
        }

        private void AddDirectory()
        {            
            var dialogVm = new DirectoryDialogViewModel(new DirectoryDialog());
            
            dialogVm.ShowDialog();
            if (!dialogVm.IsCancelled)
            {
                this.Entries.Add(dialogVm.Directory, new ObservableCollection<CrawlerResult>());
                FileHandler.SaveDirectories(this.Entries.Keys);
            }
        }

        private void ParseDirectoryFile()
        {
            this.Entries = new Dictionary<CrawlerDirectory, ObservableCollection<CrawlerResult>>(new CrawlerDirectoryEqualityComparer());

            var directories = FileExploder.Bl.FileHandler.LoadDirectories();
            if (directories.Any())
            {
                foreach (var directory in directories)
                {
                    this.Entries.Add(directory, new ObservableCollection<CrawlerResult>());
                }

                this.SelectedDirectory = this.Entries.Keys.First();
            }
        }

        public ICommand AddDirectoryCommand { get; set; }
        public ICommand RemoveDirectoryCommand { get; set; }
        public ICommand RefreshDirectoryCommand { get; set; }
        public ICommand OpenExplorerCommand { get; set; }
        private CrawlerDirectory selectedDirectory;
        private MainWindow mainWindow;

        public CrawlerDirectory SelectedDirectory
        {
            get { return this.selectedDirectory; }
            set
            {
                this.selectedDirectory = value;
                RaisePropertyChanged("SelectedDirectory");

                this.CrawlerResults.Clear();

                UpdateCrawlerResults(this.Entries[value]);
                
                if (this.CrawlerResults.Any())
                {
                    this.SelectedCrawlerResult = this.CrawlerResults[0];
                }
                else
                {
                    this.SelectedCrawlerResult = null;
                }
            }
        }

        private void UpdateCrawlerResults(IEnumerable<CrawlerResult> results)
        {
            foreach (var item in results)
            {
                this.CrawlerResults.Add(item);
            }
        }
   

        public Dictionary<CrawlerDirectory, ObservableCollection<CrawlerResult>> Entries { get; set; }

        private ObservableCollection<CrawlerResult> crawlerResults = new ObservableCollection<CrawlerResult>();

        public ObservableCollection<CrawlerResult> CrawlerResults
        {
            get { return crawlerResults; }
            set
            {
                crawlerResults = value;
                RaisePropertyChanged("CrawlerResults");
            }
        }


        private CrawlerResult selectedCrawlerResult;

        public CrawlerResult SelectedCrawlerResult
        {
            get { return selectedCrawlerResult; }
            set
            {
                selectedCrawlerResult = value;
                RaisePropertyChanged("SelectedCrawlerResult");
            }
        }

        private Visibility loading = Visibility.Hidden;

        public Visibility Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                RaisePropertyChanged("Visibility");
            }
        }

        //public object FileHandler { get; private set; }
    }
}
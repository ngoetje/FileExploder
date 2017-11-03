using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FileExploder.Bl;
using FileExploder.Model;
using FileExploder.Ui.Infrastructure;

namespace FileExploder
{
    /// <summary>
    /// Main ViewModel for Main Window
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new ViewModel for the given <see cref="MainWindow"/>
        /// </summary>
        /// <param name="window"></param>
        public MainWindowViewModel(MainWindow window)
        {
            this.mainWindow = window;
            this.mainWindow.Closing += OnClosing;

            InitializeCommands();
            ParseDirectoryFile();
        }

        /// <summary>
        /// The command for adding a new directory entry
        /// </summary>
        public ICommand AddDirectoryCommand { get; set; }
        /// <summary>
        /// The command for copying the path
        /// </summary>
        public ICommand CopyPathCommand { get; set; }

        /// <summary>
        /// The Crawler results
        /// </summary>
        public ObservableCollection<CrawlerResult> CrawlerResults
        {
            get { return this.crawlerResults; }
            set
            {
                this.crawlerResults = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// The command for editing the selected directory entry
        /// </summary>
        public ICommand EditDirectoryCommand { get; set; }

        /// <summary>
        /// Dictionary containing the directory and its files matching the criteria
        /// </summary>
        public Dictionary<CrawlerDirectory, ObservableCollection<CrawlerResult>> Entries { get; set; }

        /// <summary>
        /// The Error from the crawler
        /// </summary>
        public String Error
        {
            get { return this.error; }
            set
            {
                this.error = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Visibility Loading
        {
            get { return this.loading; }
            set
            {
                this.loading = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenExplorerCommand { get; set; }
        public ICommand RefreshDirectoryCommand { get; set; }
        public ICommand RemoveDirectoryCommand { get; set; }

        public CrawlerResult SelectedCrawlerResult
        {
            get { return this.selectedCrawlerResult; }
            set
            {
                this.selectedCrawlerResult = value;
                RaisePropertyChanged();
            }
        }

        public CrawlerDirectory SelectedDirectory
        {
            get { return this.selectedDirectory; }
            set
            {
                this.selectedDirectory = value;
                RaisePropertyChanged();

                CrawlerResults.Clear();

                UpdateCrawlerResults(Entries[value]);

                if (CrawlerResults.Any())
                {
                    SelectedCrawlerResult = CrawlerResults[0];
                }
                else
                {
                    SelectedCrawlerResult = null;
                }
            }
        }

        private void AddDirectory()
        {
            var dialogVm = new DirectoryDialogViewModel(new DirectoryDialog());

            dialogVm.ShowDialog();
            if (!dialogVm.IsCancelled)
            {
                Entries.Add(dialogVm.Directory, new ObservableCollection<CrawlerResult>());
                FileHandler.SaveDirectories(Entries.Keys);
            }
        }

        private Boolean CanRefreshDirectory()
        {
            return SelectedDirectory != null;
        }

        private Boolean CanRemoveDirectory()
        {
            return SelectedDirectory != null;
        }

        private void CopyPath(Object o)
        {
            var result = o as CrawlerResult;
            if (result != null)
            {
                if (SelectedCrawlerResult != result)
                {
                    SelectedCrawlerResult = result;
                }
                Clipboard.SetData(DataFormats.StringFormat, result.Directory.FullName);
            }
        }

        private void EditDirectory()
        {
            var dialogVm = new DirectoryDialogViewModel(new DirectoryDialog());
            dialogVm.Directory = this.selectedDirectory;
            dialogVm.ShowDialog();
            if (!dialogVm.IsCancelled)
            {
                Entries.Remove(dialogVm.Directory);
                Entries.Add(dialogVm.Directory, new ObservableCollection<CrawlerResult>());
                FileHandler.SaveDirectories(Entries.Keys);
            }
        }

        private void InitializeCommands()
        {
            AddDirectoryCommand = new RelayCommand(p => true, p => AddDirectory());
            RemoveDirectoryCommand = new RelayCommand(p => CanRemoveDirectory(), p => RemoveDirectory());
            EditDirectoryCommand = new RelayCommand(p => CanRemoveDirectory(), p => EditDirectory());
            RefreshDirectoryCommand = new RelayCommand(p => CanRefreshDirectory(), p => RefreshDirectory());
            OpenExplorerCommand = new RelayCommand(p => true, OpenExplorer);
            CopyPathCommand = new RelayCommand(p => true, CopyPath);
        }

        private void OnClosing(Object sender, CancelEventArgs e)
        {
            FileHandler.SaveDirectories(Entries.Keys);
        }

        private void OpenExplorer(Object p)
        {
            var result = p as CrawlerResult;
            if (result != null)
            {
                if (SelectedCrawlerResult != result)
                {
                    SelectedCrawlerResult = result;
                }
                Process.Start(result.Directory.FullName);
            }
        }

        private void ParseDirectoryFile()
        {
            Entries = new Dictionary<CrawlerDirectory, ObservableCollection<CrawlerResult>>(new CrawlerDirectoryEqualityComparer());

            var directories = FileHandler.LoadDirectories();
            if (directories.Any())
            {
                foreach (var directory in directories)
                {
                    if (!Entries.ContainsKey(directory))
                    {
                        Entries.Add(directory, new ObservableCollection<CrawlerResult>());
                    }
                }

                SelectedDirectory = Entries.Keys.First();
            }
        }

        private async void RefreshDirectory()
        {
            Entries[SelectedDirectory].Clear();
            CrawlerResults.Clear();
            SelectedCrawlerResult = null;

            Loading = Visibility.Visible;

            var result = await Crawler.CrawlAsync(SelectedDirectory);

            if (!String.IsNullOrEmpty(result.Error))
            {
                Error = result.Error;
                CrawlerResults.Clear();
            }
            else
            {
                Error = String.Empty;
                foreach (var item in result.Items)
                {
                    Entries[SelectedDirectory].Add(item);
                }

                CrawlerResults.Clear();
                UpdateCrawlerResults(Entries[SelectedDirectory]);
            }

            Loading = Visibility.Hidden;
        }

        private void RemoveDirectory()
        {
            Entries.Remove(SelectedDirectory);
            if (Entries.Any())
            {
                SelectedDirectory = Entries.Keys.First();
            }
        }

        private void UpdateCrawlerResults(IEnumerable<CrawlerResult> results)
        {
            foreach (var item in results)
            {
                CrawlerResults.Add(item);
            }
        }

        private ObservableCollection<CrawlerResult> crawlerResults = new ObservableCollection<CrawlerResult>();

        private String error = String.Empty;

        private Visibility loading = Visibility.Hidden;
        private readonly MainWindow mainWindow;

        private CrawlerResult selectedCrawlerResult;
        private CrawlerDirectory selectedDirectory;
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScoreKeeper.Model;
using ScoreKeeper.ViewModels.Services;
using ScoreKeeper.Views;

namespace ScoreKeeper.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private EditMatchViewModel editMatchViewModel;
        private int selectedTabIndex;
        private IDialogService dialogService;

        public ObservableCollection<string> AllPlayers { get; private set; }
        public RelayCommand NewMatch { get; private set; }
        public RelayCommand Exit { get; private set; }
        public RelayCommand Publish { get; private set; }
        public RelayCommand Settings { get; private set; }

        public StatsViewModel StatsViewModel { get; set; }
        public MatchesViewModel MatchesViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }

        public EditMatchViewModel EditMatchViewModel
        {
            get { return editMatchViewModel; }
            set
            {
                if (Equals(value, editMatchViewModel)) return;
                editMatchViewModel = value;
                OnPropertyChanged();
            }
        }

        private bool isSettingsFlyoutOpen;

        public bool IsSettingsFlyoutOpen
        {
            get { return isSettingsFlyoutOpen; }
            set
            {
                if (!value.Equals(isSettingsFlyoutOpen))
                {
                    isSettingsFlyoutOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                if (value == selectedTabIndex) return;
                selectedTabIndex = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            Initializate();
            NewMatch = new RelayCommand(_ => EditMatch(null));
            Exit = new RelayCommand(_ => Application.Current.MainWindow.Close());
            Publish = new RelayCommand(_ => OnPublishCommand());
            Settings = new RelayCommand(_ => OnSettingsCommand());
        }

        private void OnSettingsCommand()
        {
            IsSettingsFlyoutOpen = true;
            //var w = new Window();
            //w.Title = "Settings";
            //w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //w.Content = new SettingsView();
            //w.SizeToContent = SizeToContent.WidthAndHeight;
            //var vm = new SettingsViewModel(() => w.Close());
            //w.DataContext = vm;
            //w.Owner = Application.Current.MainWindow;
            //w.ShowDialog();
        }

        private async void OnPublishCommand()
        {
            var controller = await dialogService.ShowProgressAsync("OnPublishing", "please wait");

            controller.SetCancelable(true);
            int uploadCount = 0;
            foreach (var m in MatchesViewModel.Matches)
            {
                await UploadMatch(m);
                if (controller.IsCanceled)
                    break;
                uploadCount++;
                controller.SetProgress((double)uploadCount / MatchesViewModel.Matches.Count);
            }
            await controller.CloseAsync();
            if (controller.IsCanceled)
            {
                await dialogService.ShowMessageDialog("Publishing", "Publishing was canceled");
            }
            else
            {
                await dialogService.ShowMessageDialog("Publishing", "Publishing was published normaly");
            }
            //var w = new Window();
            //w.Width = 400;
            //w.Height = 150;
            //w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //w.Title = "Publishing";
            //w.Content = new PublishView();
            //var vm = new PublishViewModel(() => w.Close());
            //w.DataContext = vm;
            //w.Owner = Application.Current.MainWindow;
            //w.ShowDialog();
        }

        private Task UploadMatch(MatchViewModel m)
        {
            return Task.Delay(250);
        }

        private void Initializate()
        {
            var matchesJson = File.ReadAllText("Matches.json");
            var matches = JsonConvert.DeserializeObject<List<Match>>(matchesJson,
                new StringEnumConverter());
            //var s = JsonConvert.SerializeObject(matches, Formatting.Indented, new StringEnumConverter());
            //var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //var p = Path.Combine(appData, "ScoreKeeper");
            //Directory.CreateDirectory(p);
            //var f = Path.Combine(p, "matches.json");        
            //File.WriteAllText(f, s);
            var matchViewModels = new ObservableCollection<MatchViewModel>(matches.Select(m => new MatchViewModel(m)));

            AllPlayers = new ObservableCollection<string>(matches.SelectMany(m => m.StartingEleven));

            MatchesViewModel = new MatchesViewModel(matchViewModels,
                new RelayCommand(_ => EditMatch(null)),
                new RelayCommand(m => EditMatch(((MatchViewModel)m).Match),
                    o => o != null),
                dialogService);

            StatsViewModel = new StatsViewModel(matchViewModels);
            SettingsViewModel = new SettingsViewModel(() => IsSettingsFlyoutOpen = false);

        }

        private void EditMatch(Match m)
        {
            EditMatchViewModel = new EditMatchViewModel(m ?? Match.CreateNew(), AllPlayers);
            SelectedTabIndex = 2;
        }
    }
}

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreKeeper.ViewModels.Services
{
    internal class DialogService : IDialogService
    {
        private readonly MetroWindow metroWindow;
        public DialogService(MetroWindow metroWindow)
        {
            this.metroWindow = metroWindow;
        }

        public Task<MessageDialogResult> AskQuestonAsync(string title, string message)
        {
            var settings = new MetroDialogSettings
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            return metroWindow.ShowMessageAsync("Delete Match", "Are you sure you want to delete this Match record ? ",
                MessageDialogStyle.AffirmativeAndNegative, settings);
        }

        public Task ShowMessageDialog(string title, string message)
        {
            return metroWindow.ShowMessageAsync(title, message);
        }

        public Task<ProgressDialogController> ShowProgressAsync(string title, string message)
        {
            return metroWindow.ShowProgressAsync(title, message);
        }
    }
}

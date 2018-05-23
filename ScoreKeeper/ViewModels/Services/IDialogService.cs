using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace ScoreKeeper.ViewModels.Services
{
    internal interface IDialogService
    {
        Task<MessageDialogResult> AskQuestonAsync(string title, string message);
        Task<ProgressDialogController> ShowProgressAsync(string title, string message);
        Task ShowMessageDialog(string title, string message);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using Application = System.Windows.Application;

namespace TodoList_Project.Features.CharacterDialogs
{
    public partial class CharacterDialogViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _message;
        [ObservableProperty]
        private string _imagePath;
        public CharacterDialogViewModel(string message, string imagePath)
        {
            Message = message;
            ImagePath = imagePath;
        }
    }
}

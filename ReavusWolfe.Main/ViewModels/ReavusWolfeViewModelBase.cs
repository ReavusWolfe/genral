using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace ReavusWolfe.Main.ViewModels
{
    public class ReavusWolfeViewModelBase : ViewModelBase
    {
        private string _windowTitle;
        private bool _loadingData;
        public Action Close;

        public bool LoadingData
        {
            get { return _loadingData; }
            set
            {
                _loadingData = value;
                RaisePropertyChanged(() => LoadingData);
            }
        }

        public string WindowTitle
        {
            get { return "ReavusWolfe BoilerPlate - " + _windowTitle; }
            set
            {
                _windowTitle = value;
                RaisePropertyChanged(() => WindowTitle);
            }
        }
    }

    public class DetailsViewModelBase : ReavusWolfeViewModelBase
    {
        private bool _isANewRecord = true;

        public bool IsANewRecord
        {
            get { return _isANewRecord; }
            set
            {
                _isANewRecord = value;
                RaisePropertyChanged(() => IsANewRecord);
            }
        }

        public virtual void LoadNewObject(int id)
        {
            throw new InvalidOperationException();
        }

        public virtual void LoadExistingObject(int id)
        {
            throw new InvalidOperationException();
        }
    }
}

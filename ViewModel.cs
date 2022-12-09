using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ConcatPdf
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ConcatModel _concatModel = new ConcatModel();
        public ViewModel()
        {
            KolFiles = _concatModel.kolFiles;
        }
        public int KolFiles
        {
            get => _concatModel.kolFiles;
            set
            {
                _concatModel.kolFiles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KolFiles)));

                RowTableFilesCollection.Clear();
                RowTableFilesCollection = new ObservableCollection<RowTableFiles>();
                for (int numPl = 0; numPl < _concatModel.kolFiles; ++numPl)
                {
                    var pl = numPl;
                    RowTableFilesCollection.Add(new RowTableFiles("Не загружен", $"Файл {numPl + 1}",
                        new BaseCommand(
                            (o) =>
                            {
                                var ofd = new OpenFileDialog
                                {
                                    Filter = @"pdf|*.pdf"
                                };
                                if (ofd.ShowDialog() ?? false)
                                {
                                    RowTableFilesCollection[pl].IsLoad = true;
                                }
                            })));
                }
            }
        }

        ObservableCollection<RowTableFiles> _RowTableFilesCollection = new ObservableCollection<RowTableFiles>();

        public ObservableCollection<RowTableFiles> RowTableFilesCollection
        {
            get => _RowTableFilesCollection;
            set
            {
                _RowTableFilesCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RowTableFilesCollection)));
            }
        }

        public BaseCommand BaseCommandConcat { get; } = new BaseCommand(
            (o) =>
            {
                SaveFileDialog opf = new SaveFileDialog
                {
                    Filter = @"pdf|*.pdf"
                };
                if (opf.ShowDialog() ?? false)
                {
                    var file = opf.FileName;

                    MessageBox.Show("Объединение завершено успешно.");
                }
            });

    }
    public class RowTableFiles : INotifyPropertyChanged
    {
        public RowTableFiles(string n, string button, BaseCommand command)
        {
            NameFile = n;
            ButtonLabelFilePl = button;
            BaseCommandButton = command;
            IsLoad = false;
        }
        public string NameFile { get; set; }
        public string ButtonLabelFilePl { get; set; }
        public BaseCommand BaseCommandButton { get; set; }
        public string IsLoadStr { get; set; }
        private bool _isLoad = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoad
        {
            get => _isLoad;
            set
            {
                _isLoad = value;
                IsLoadStr = _isLoad ? "Да" : "Нет";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoadStr)));
            }
        }
    }

    public class BaseCommand : ICommand
    {
        private Action<object> _action;
        Predicate<object> _predicate;
        public BaseCommand(Action<object> action, Predicate<object> predicate = null)
        {
            _action = action;
            _predicate = predicate;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _predicate?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter);
        }
    }
}


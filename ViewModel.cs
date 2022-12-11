using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.IO;

namespace ConcatPdf
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static ConcatModel _concatModel = new ConcatModel();
        private static Window _own;
        public ViewModel(Window own)
        {
            KolFiles = _concatModel.kolFiles;
            _own = own;
        }
        public int KolFiles
        {
            get => _concatModel.kolFiles;
            set
            {
                _concatModel.kolFiles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KolFiles)));

                RowTableFilesCollection.Clear();
                _concatModel.pathPdfFiles = new string[_concatModel.kolFiles];
                RowTableFilesCollection = new ObservableCollection<RowTableFiles>();
                for (int numF = 0; numF < _concatModel.kolFiles; ++numF)
                {
                    var numFile = numF;
                    RowTableFilesCollection.Add(new RowTableFiles("Не загружен", $"Файл {numF + 1}",
                        new BaseCommand(
                            (o) =>
                            {
                                var tuple = _concatModel.OpenAndReadPdf(numFile);
                                if (tuple.res != 0)
                                {
                                    MessageBox.Show(tuple.str + "_concatModel.OpenAndReadPdf");
                                    return;
                                }
                                RowTableFilesCollection[numFile].IsLoad = true;
                                FileInfo fInfo = new FileInfo(_concatModel.pathPdfFiles[numFile]);
                                RowTableFilesCollection[numFile].NameFile = fInfo.Name;
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(fInfo.Name)));
                                //Проверить, возможно ли уже объединение файлов
                                BaseCommandConcat?.OnCanExecuteChanged();
                            })));
                }

                BaseCommandConcat?.OnCanExecuteChanged();
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

                BaseCommandConcat?.OnCanExecuteChanged();
            }
        }

        public BaseCommand BaseCommandConcat { get; } = new BaseCommand(
            async (o) =>
            {

                var tuple = await _concatModel.SaveConcatPdf(_own);
                
                if (tuple.res != 0)
                {
                    MessageBox.Show(tuple.str + "_concatModel.SaveConcatPdf");
                    return;
                }
                MessageBox.Show("Объединение завершено успешно.");
            },
            (o) =>
            {
                return _concatModel.IsCorretPaths();
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
        private string _nameFile;
        public string NameFile
        {
            get => _nameFile;
            set
            {
                _nameFile = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameFile)));
            }
        }
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

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }
}


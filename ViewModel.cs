using System.ComponentModel;

namespace ConcatPdf
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int kolFiles = 2;
        public int KolFiles
        {
            get => kolFiles; 
            set {
                kolFiles = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KolFiles)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MMO.Launcher
{
    public class MainWindowViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _canLogin;
        private string _status;
        private double _precent;
        private string _username;

        public string Status {
            get { return _status; }
            set {
                Precent = 0;

                _status = value; 
                OnPropertyChanged();
            }
        }

        public double Precent {
            get { return _precent; }
            set {
                _precent = value; 
                OnPropertyChanged();
            }
        }

        public string Username {
            get { return _username; }
            set {
                _username = value; 
                OnPropertyChanged();
            }
        }

        public bool CanLogin {
            get { return _canLogin; }
            set {
                _canLogin = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName]string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

using ProgExtinction.Helpers;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace ProgExtinction.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        System.Media.SoundPlayer m_notif;
        private Timer m_timer = new Timer();

        private DateTime m_dateExtinction;
        private bool m_modeMinuteur, m_extinctionDesactivee;
        private int m_heure, m_minute, m_seconde;

        public ICommand ProgrammerExtinctionCommand => new RelayCommand(p => ActiverExtinction());

        public DateTime DateExtinction
        {
            get
            {
                return m_dateExtinction;
            }
            set
            {
                m_dateExtinction = value;
                RaisePropertyChanged(nameof(DateExtinction));
            }
        }

        public bool ExtinctionDesactivee
        {
            get => m_extinctionDesactivee;
            set { m_extinctionDesactivee = value; RaisePropertyChanged(nameof(ExtinctionDesactivee)); }
        }

        public bool ModeMinuteur
        {
            get => m_modeMinuteur;
            set 
            {
                m_modeMinuteur = value; 
                RaisePropertyChanged(nameof(ModeMinuteur));

                if (ModeMinuteur)
                {
                    m_heure = 0;
                    m_minute = 0;
                    m_seconde = 0;

                    MiseAJourDateExtinction();
                }
                else
                {
                    m_heure = DateTime.Now.Hour;
                    m_minute = DateTime.Now.Minute;
                    m_seconde = DateTime.Now.Second;

                    MiseAJourDateExtinction();
                }

                RaisePropertyChanged(nameof(Heure), nameof(Minute), nameof(Seconde));
            }
        }

        public int Heure
        {
            get => m_heure;
            set { m_heure = value; RaisePropertyChanged(nameof(Heure)); MiseAJourDateExtinction(); }
        }

        public int Minute
        {
            get => m_minute;
            set { m_minute = value; RaisePropertyChanged(nameof(Minute)); MiseAJourDateExtinction(); }
        }

        public int Seconde
        {
            get => m_seconde;
            set { m_seconde = value; RaisePropertyChanged(nameof(Seconde)); MiseAJourDateExtinction(); }
        }

        public MainWindowViewModel()
        {
            m_notif = new System.Media.SoundPlayer();
            m_notif.SoundLocation = "notif.wav";

            m_timer.Elapsed += new ElapsedEventHandler(MiseAJourHeureMinuteSeconde);
            m_timer.Interval = 300;
            m_timer.Enabled = true;

            ExtinctionDesactivee = true;
            ModeMinuteur = true;
        }

        private void MiseAJourHeureMinuteSeconde(object sender, ElapsedEventArgs e)
        {
            Func<DateTime, DateTime, TimeSpan> Soustraction = (d1, d2) => d1 > d2 ? d1 - d2 : d2 - d1;

            // Mode configuration
            if (ExtinctionDesactivee)
            {
                if (ModeMinuteur)
                {
                    DateExtinction = DateTime.Now.AddHours(Heure).AddMinutes(Minute).AddSeconds(Seconde);
                }
            }

            // Mode extinction enclenché
            else if (!ExtinctionDesactivee)
            {
                var tempRestant = Soustraction(DateTime.Now, DateExtinction);

                m_heure = tempRestant.Hours;
                m_minute = tempRestant.Minutes;
                m_seconde = tempRestant.Seconds;
                RaisePropertyChanged(nameof(Heure), nameof(Minute), nameof(Seconde));
            }

            // Extinction
            if (!ExtinctionDesactivee && Heure == 0 && Minute == 0)
            {
                if (Seconde == 30 || Seconde == 10 || Seconde == 6 || Seconde <= 2)
                    m_notif.Play();
;
                if (Seconde == 0)
                {
                    m_timer.Enabled = false;
                    Process.Start("shutdown", "/s /t 0");
                }
            }
        }

        private void MiseAJourDateExtinction(DateTime? dateTime = null)
        {
            if (ModeMinuteur)
            {
                DateExtinction = DateTime.Now.AddHours(Heure).AddMinutes(Minute).AddSeconds(Seconde);
            }
            else
            {
                DateExtinction = dateTime ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Heure, Minute, Seconde);
            }
        }

        private bool ActiverExtinction()
        {
            if (DateExtinction < DateTime.Now.AddSeconds(3)
                && MessageBox.Show("Êtes-vous sûr de vouloir éteindre l'ordinateur immédiatement ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return false;

            return ExtinctionDesactivee = !ExtinctionDesactivee;
        }
    }
}

using AppLocalizacao.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppLocalizacao.ViewModel
{
    public class ViewModelHome : BaseViewModel
    {
        #region variáveis
        private bool comecarEstado;
        private bool pararEstado;
        private string mensagem;
        private double latitude;
        private double longitude;
        private const string STATUS_APP = "status";
        #endregion

        #region propiedades
        public bool ComecarEstado
        {
            get => comecarEstado;
            set => SetProperty(ref comecarEstado, value);
        }
        public bool PararEstado
        {
            get => pararEstado;
            set => SetProperty(ref pararEstado, value);
        }
        public string Mensagem
        {
            get => mensagem;
            set => SetProperty(ref mensagem, value);
        }
        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }
        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }
        #endregion

        #region comandos
        public ICommand ComecarCommand { get; }
        public ICommand PararCommand { get; }
        #endregion

        #region metódos
        private void MonitoraMessage()
        {
            MessagingCenter.Subscribe<LocalizacaoErroMessage>(this, "LocalizacaoErro", mensagem =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Mensagem = "Aconteceu um erro ao obter a localização !!";
                });
            });
            MessagingCenter.Subscribe<LocalizacaoParadaMessage>(this, "localizacaoParada", mensagem =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Mensagem = "Compartilhamento da localização parada !!";
                });
            });
            MessagingCenter.Subscribe<LocalizacaoMessage>(this, "localizacao", mensagem =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Latitude = mensagem.Latitude;
                    Longitude = mensagem.Longitude;
                    Mensagem = "Localização actualizada";
                });
            });
        }

        private void Parar()
        {
            LocalizacaoParadaMessage message = new LocalizacaoParadaMessage();
            MessagingCenter.Send(message, "localizacaoParada");
            SecureStorage.SetAsync(STATUS_APP, "0");
            ComecarEstado = true;
            PararEstado = false;
        }
        #endregion

        public ViewModelHome()
        {
            PararCommand = new Command(() => Parar());
        }
    }
}

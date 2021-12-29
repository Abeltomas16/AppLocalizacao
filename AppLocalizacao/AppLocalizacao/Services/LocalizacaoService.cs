using AppLocalizacao.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppLocalizacao.Services
{
    public class LocalizacaoService
    {
        private readonly bool rodando = false;
        public async Task Obter(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (!rodando)
                {
                    token.ThrowIfCancellationRequested();
                    try
                    {
                        var localizacaoRequest = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(2000));
                        var localizacao = await Geolocation.GetLocationAsync(localizacaoRequest);
                        if (localizacao == null) return;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LocalizacaoMessage localizacao_message = new LocalizacaoMessage
                            {
                                Latitude = localizacao.Latitude,
                                Longitude = localizacao.Longitude
                            };
                            MessagingCenter.Send(localizacao_message, "localizacao");
                        });
                    }
                    catch (Exception)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LocalizacaoErroMessage erro = new LocalizacaoErroMessage();
                            MessagingCenter.Send(erro, "LocalizacaoErro");
                        });
                    }
                }
                return;
            }, token);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AppLocalizacao.Contratos;
using AppLocalizacao.Droid.Services;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(VerificaPermissaoService))]
namespace AppLocalizacao.Droid.Services
{
    public class VerificaPermissaoService : ILocalizacaoVerificar
    {
        public async Task VerificaPermissao()
        {
            await Permissions.RequestAsync<Permissions.LocationAlways>();
        }
    }
}
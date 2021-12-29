using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AppLocalizacao.Droid.Contratos;
using AppLocalizacao.Messages;
using AppLocalizacao.Services;
using Xamarin.Forms;

namespace AppLocalizacao.Droid.Services
{
    [Service]
    public class LocalizacaoActivityService : Service
    {
        CancellationTokenSource ck;
        const int SERVICE_ID = 20000;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            ck = new CancellationTokenSource();
            Notification notification = DependencyService.Get<INotification>().RetornaNotificacao();
            StartForeground(SERVICE_ID, notification);
            Task.Run(() =>
            {
                try
                {
                    var service_localizacao = new LocalizacaoService();
                    service_localizacao.Obter(ck.Token).Wait(); ;
                }
                catch (System.OperationCanceledException)
                {
                }
                finally
                {
                    if (ck.IsCancellationRequested)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LocalizacaoParadaMessage message = new LocalizacaoParadaMessage();
                            MessagingCenter.Send(message, "localizacaoParada");
                        });
                    }
                }
            }, ck.Token);
            return StartCommandResult.Sticky;
        }
        public override void OnDestroy()
        {
            if (ck != null)
            {
                ck.Token.ThrowIfCancellationRequested();
                ck.Cancel();
            }
            base.OnDestroy();
        }
    }
}
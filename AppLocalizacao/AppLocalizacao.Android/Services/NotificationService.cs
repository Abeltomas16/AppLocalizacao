
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using AppLocalizacao.Droid.Contratos;
using AppLocalizacao.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationService))]
namespace AppLocalizacao.Droid.Services
{
    public class NotificationService : INotification
    {
        private static string foregroundChannelId = "9001";
        private static Context context = global::Android.App.Application.Context;

        public Notification RetornaNotificacao()
        {
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notifBuilder = new NotificationCompat.Builder(context, foregroundChannelId)
                .SetContentTitle("Abel Location")
                .SetContentText("Servico rodando em segundo plano")
                .SetSmallIcon(Resource.Drawable.pointer)
                .SetOngoing(true)
                .SetContentIntent(pendingIntent);

            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "Title", NotificationImportance.High);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300 });

                var notifManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notifManager != null)
                {
                    notifBuilder.SetChannelId(foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notifBuilder.Build();
        }
    }
}
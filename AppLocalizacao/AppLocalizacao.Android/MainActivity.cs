using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using AppLocalizacao.Droid.Services;
using Xamarin.Forms;
using AppLocalizacao.Messages;

namespace AppLocalizacao.Droid
{
    [Activity(Label = "AppLocalizacao", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent tarefa;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            tarefa = new Intent(this, typeof(LocalizacaoActivityService));
            DefinirDependencias();
            LoadApplication(new App());
        }

        private void DefinirDependencias()
        {
            MessagingCenter.Subscribe<LocalizacaoIniciar>(this, "localizacaoIniciar", Message =>
            {
                if (!IsServicoRodando(typeof(LocalizacaoActivityService)))
                {
                    if (!IsServicoRodando(typeof(LocalizacaoActivityService)))
                    {
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                            StartForegroundService(tarefa);
                        else
                            StartService(tarefa);
                    }
                }
            });

            MessagingCenter.Subscribe<LocalizacaoParadaMessage>(this, "localizacaoParada", Message =>
            {
                if (IsServicoRodando(typeof(LocalizacaoActivityService)))
                    StopService(tarefa);
            });
        }

        private bool IsServicoRodando(Type type)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);
            foreach (var servico in manager.GetRunningServices(int.MaxValue))
            {
                if (servico.Service.ClassName.Equals(Java.Lang.Class.FromType(type).CanonicalName))
                    return true;
            }
            return false;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
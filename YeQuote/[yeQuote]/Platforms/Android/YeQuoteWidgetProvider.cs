using _yeQuote_;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using Microsoft.Maui.Storage;

// HOLY ****
// EVERYTHING I WRITE HERE HAS TO END UP GETTING A WARNING
// **** MAUI AND ANDROID 

// PLEASE THIS TOOK SO LONG
// BECAUSE ANDROID LIKES JAVA SO MUCH
// AND EVERYTHING IS EASY IF YOU USE JAVA TO PROGAM ON THIER PLATFORM

namespace yeQuote.Platforms.Android
{
    [BroadcastReceiver(Label = "yeQuoteWidget", Exported = true)]
    [MetaData("android.appwidget.provider", Resource = "@xml/ye_widget_info")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    public class YeQuoteWidgetProvider : AppWidgetProvider
    {
#if DEBUG
        private static readonly long RefreshInterval = 10 * 1000; // 10 seconds
#else
        private static readonly long RefreshInterval = 12 * 60 * 60 * 1000; // 12 hours
#endif


        private async Task OnUpdateAsync(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            var quotex = await Quotes.GetQuote();

            foreach (int appWidgetId in appWidgetIds)
            {
                var remoteViews = new RemoteViews(context.PackageName, Resource.Layout.ye_widget_layout);

                string quoteText = Preferences.Default.Get("Quote", quotex);

                remoteViews.SetTextViewText(Resource.Id.NiceAtPingPong, quoteText);

                var intent = new Intent(context, typeof(MainActivity));
                intent.SetFlags(ActivityFlags.ClearTop);

                // THANK YOU GOOGLE
                var pendingIntent = PendingIntent.GetActivity(context,
                    0,
                    intent,
                    PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

                remoteViews.SetOnClickPendingIntent(Resource.Id.NiceAtPingPong, pendingIntent);


                appWidgetManager.UpdateAppWidget(appWidgetId, remoteViews);
            }


            SetRecurringAlarm(context); // this sets up the updater and things yeha ok?
        }

        // This is the brain of the widget
        // it updates the widget on time
        // i love this
        // we love this
        
        private void SetRecurringAlarm(Context context)
        {
            var intent = new Intent(context, typeof(YeQuoteWidgetProvider));
            intent.SetAction("android.appwidget.action.APPWIDGET_UPDATE");
            var pendingIntent = PendingIntent.GetBroadcast(context,
                0,
                intent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                // for android 6.0 and above
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + RefreshInterval, pendingIntent);
            }
            else
            {
                // for your nokia, idk if this app is supported on 5.0 and under. but just gotta make sure.
                // you just never know
                alarmManager.SetRepeating(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime(), RefreshInterval, pendingIntent);
            }
        }

        
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (intent.Action == "android.appwidget.action.APPWIDGET_UPDATE")
            {
                var appWidgetManager = AppWidgetManager.GetInstance(context);
                var componentName = new ComponentName(context.PackageName, Java.Lang.Class.FromType(typeof(YeQuoteWidgetProvider)).Name);
                var appWidgetIds = appWidgetManager.GetAppWidgetIds(componentName);
                OnUpdate(context, appWidgetManager, appWidgetIds);
            }
        }

        // manual trigger for testing, idk if i should turn this into a macro or not
        public static void NotifyWidgetUpdate(Context context)
        {
            var intent = new Intent(context, typeof(YeQuoteWidgetProvider));
            intent.SetAction("android.appwidget.action.APPWIDGET_UPDATE");
            context.SendBroadcast(intent);
        }



        // sit down boy!!
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            _ = OnUpdateAsync(context, appWidgetManager, appWidgetIds);
        }
    }
}

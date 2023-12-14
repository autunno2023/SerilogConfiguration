using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SerilogConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Host.CreateDefaultBuilder 
            /*
            Utilizzare (Host.CreateDefaultBuilder)  nelle applicazioni.NET, offre diversi vantaggi di configurazione:

            CONFIGURAZIONE SEMPLIFICATA: CreateDefaultBuilder imposta automaticamente molte configurazioni predefinite, riducendo la quantità di codice standard che è necessario scrivere.Include fonti di configurazione predefinite come appsettings.json, variabili di ambiente e argomenti della riga di comando.

            CONFIGURAZIONE DEL LOGGING: Configura di default il logging per le uscite console e debug. Questo significa che si ottiene il logging subito senza doverlo configurare manualmente. È possibile estendere o modificare facilmente questa configurazione di logging.

            INIEZIONE DELLE DIPENDENZE(DI): Imposta un contenitore di iniezione delle dipendenze, che è una caratteristica fondamentale delle moderne applicazioni .NET.Questo contenitore DI viene utilizzato per gestire i cicli di vita dei tuoi servizi e delle loro dipendenze.

            SUPPORTO A IHOSTEDSERVICE: Fornisce supporto per le implementazioni di IHostedService, che sono utili per eseguire attività in background e servizi ospitati all'interno dell'applicazione.

            CONFIGURAZIONE DEGLI AMBIENTI: Legge automaticamente l'impostazione dell'ambiente(come Sviluppo, Staging, Produzione) e regola di conseguenza il comportamento dell'applicazione (ad esempio, pagine di errore dettagliate in sviluppo).

            CONFIGURAZIONE DEL WEB SERVER: Per le applicazioni web, configura il web server predefinito(Kestrel) e lo integra con IIS quando si esegue su Windows.

            OTTIMIZZAZIONE DELLE PRESTAZIONI: Include alcune ottimizzazioni delle prestazioni, come impostazioni del pool di thread e della raccolta dei rifiuti adatte per carichi di lavoro web tipici.

            CONFIGURAZIONE COERENTE E STANDARDIZZATA: Utilizzando CreateDefaultBuilder, la configurazione della tua applicazione segue un modello coerente con altre applicazioni.NET.Questa standardizzazione rende più facile per altri sviluppatori comprendere il tuo codice e per te comprendere quello degli altri.

            ESTENDIBILITÀ: Sebbene fornisca molte impostazioni predefinite, è anche facilmente estendibile. È possibile aggiungere o sostituire servizi, configurare ulteriori provider di logging e altro, sfruttando al contempo la configurazione di base.

            In sintesi, Host.CreateDefaultBuilder fornisce un punto di partenza conveniente e opinabile per la configurazione dell'applicazione, incapsulando molte delle migliori pratiche per lo sviluppo di applicazioni .NET. Risparmia tempo e sforzi nell'impostare componenti essenziali e consente agli sviluppatori di concentrarsi maggiormente sugli aspetti unici della loro applicazione.
            */
            #endregion

            IConfiguration configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();


            try
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom
                    .Configuration(configuration)
                    //.MinimumLevel.Debug()
                    //.WriteTo.Console()
                    .CreateLogger();

                var host = CreatehosteBuilder(args, configuration).Build();

                Log.Logger.Information(" App has started.");
                var myservice = host.Services.GetService<MyService>();
                myservice.Run();
                Log.Logger.Information(" App has finished.");


            }
            catch (System.Exception)
            {
                Log.Logger.Error(" App has Crashed!.");


            }

        }
        public static IHostBuilder CreatehosteBuilder(string[] args, IConfiguration configuration)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices(services =>
            {
                services.AddTransient<MyService>();
            }).UseSerilog();
        }
    }
    interface IMyService
    {

    }
    class MyService : IMyService
    {
        readonly IConfiguration _configuration;
        readonly ILogger<IMyService> _logger;

        public MyService(ILogger<IMyService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public void Run()
        {
            try
            {
                for (int i = 0; i < _configuration.GetValue<int>("LoopNumber"); i++)
                {
                    _logger.LogInformation($" ==> Run Number {i}");
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}

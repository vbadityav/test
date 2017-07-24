using Topshelf;

namespace LegacyGateway
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(serviceConfig => {
                serviceConfig.UseNLog();

                serviceConfig.Service<LegacyGatewayService>(serviceInstance =>
                {
                    serviceInstance.ConstructUsing(() => new LegacyGatewayService());

                    serviceInstance.WhenStarted(execute => execute.Start());

                    serviceInstance.WhenStopped(execute => execute.Stop());

                    serviceInstance.WhenPaused(execute => execute.Pause());

                    serviceInstance.WhenContinued(execute => execute.Continue());

                    serviceInstance.WhenCustomCommandReceived((execute, hostControl, commandNumber) => execute.CustomCommand(commandNumber));
                });

                serviceConfig.EnableServiceRecovery(recoveryOperation => {
                    recoveryOperation.RestartService(1);
                });

                serviceConfig.EnablePauseAndContinue();

                serviceConfig.SetServiceName("e-BuilderLegacyGateway");
                serviceConfig.SetDisplayName("e-Builder Legacy Gateway");
                serviceConfig.SetDescription("Monitors SQL Server for changes on specific tables and notifying Provisioning to update MongoDB.");

                serviceConfig.StartAutomatically();
            });
        }
    }
}

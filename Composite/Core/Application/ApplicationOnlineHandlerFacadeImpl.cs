﻿using System;
using System.Web.Hosting;
using Composite.C1Console.Events;
using Composite.Core.Application.Foundation.PluginFacades;
using Composite.Core.Logging;
using Composite.Core.Types;


namespace Composite.Core.Application
{
    internal class ApplicationOnlineHandlerFacadeImpl : IApplicationOnlineHandlerFacade
    {
        private static readonly string LogTitle = typeof (ApplicationOnlineHandlerFacadeImpl).Name;

        private bool _isApplicationOnline = true;
        private bool _wasLastTurnOffSoft = false;
        //private bool _buildManagerCachingWasDisabled;
        private bool _recompileCompositeGenerated;
        private ShutdownGuard _shutdownGuard;

        public void TurnApplicationOffline(bool softTurnOff, bool recompileCompositeGenerated)
        {
            Verify.IsTrue(this.IsApplicationOnline, "The application is already offline");

            Log.LogVerbose("ApplicationOnlineHandlerFacade", string.Format("Turning off the application ({0})", softTurnOff == true ? "Soft" : "Hard"));


            _recompileCompositeGenerated = recompileCompositeGenerated;

            _shutdownGuard = new ShutdownGuard();

            try
            {
                if (softTurnOff == false)
                {
                    ApplicationOnlineHandlerPluginFacade.TurnApplicationOffline();
                    Verify.IsFalse(ApplicationOnlineHandlerPluginFacade.IsApplicationOnline(), "Plugin failed to turn the application offline");
                }
                else
                {
                    ConsoleMessageQueueFacade.Enqueue(new LockSystemConsoleMessageQueueItem(), "");
                }
            }
            catch(Exception)
            {
                _shutdownGuard.Dispose();
                _shutdownGuard = null;

                throw;
            }

            _isApplicationOnline = false;
            _wasLastTurnOffSoft = softTurnOff;

#warning MRJ: BM: Cleanup here
            //if ((clearGeneratedAssemblies == true) && (BuildManager.CachingEnabled == true))
            //{
            //    _buildManagerCachingWasDisabled = true;
            //    BuildManager.CachingEnabled = false;
            //}
        }



        public void TurnApplicationOnline()
        {
            Verify.IsFalse(this.IsApplicationOnline, "The application is already online");

            Log.LogVerbose("ApplicationOnlineHandlerFacade", "Turning on the application");

#warning MRJ: BM: Cleanup here
            if (_recompileCompositeGenerated)
            {
                try
                {
                    CodeGenerationManager.GenerateCompositeGeneratedAssembly();
                }
                catch (Exception ex)
                {
                    Log.LogError(LogTitle, "Failed to recompile Composite.Generated.dll");
                    Log.LogError(LogTitle, ex);
                }
            }
            //if (_buildManagerCachingWasDisabled)
            //{
            //    BuildManager.CachingEnabled = true;
            //    BuildManager.ClearCache();
            //    // We're not turning-on caching since in this case changing of Composite.Generated will be done 
            //    // at the same time with loading a new application domain.
            //    BuildManager.CachingEnabled = false;
            //}
            

            _shutdownGuard.Dispose();
            _shutdownGuard = null;

            try
            {
                if (_wasLastTurnOffSoft == false)
                {
                    ApplicationOnlineHandlerPluginFacade.TurnApplicationOnline();
                    Verify.IsTrue(ApplicationOnlineHandlerPluginFacade.IsApplicationOnline(), "Plugin failed to turn the application online");
                }
            }
            finally
            {
                if (HostingEnvironment.IsHosted)
                {
                    HostingEnvironment.InitiateShutdown();
                }
            }

            _isApplicationOnline = true;
        }



        public bool IsApplicationOnline
        {
            get
            {
                return _isApplicationOnline;
            }
        }
    }
}

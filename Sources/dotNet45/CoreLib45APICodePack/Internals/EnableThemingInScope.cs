using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Mohammad.Internals
{
    [SuppressUnmanagedCodeSecurity]
    internal class EnableThemingInScope : IDisposable
    {
        private uint cookie;
        // Private data
        private const int ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID = 0x004;
        private static ACTCTX enableThemingActivationContext;
        private static IntPtr hActCtx;
        private static bool contextCreationSucceeded;

        public EnableThemingInScope(bool enable)
        {
            this.cookie = 0;
            //if (enable && OSFeature.Feature.IsPresent(OSFeature.Themes))
            {
                if (this.EnsureActivateContextCreated())
                    if (!ActivateActCtx(hActCtx, out this.cookie))
                        // Be sure cookie always zero if activation failed
                        this.cookie = 0;
            }
        }

        ~EnableThemingInScope() { this.Dispose(false); }

        private void Dispose(bool disposing)
        {
            if (this.cookie != 0)
                if (DeactivateActCtx(0, this.cookie))
                    // deactivation succeeded...
                    this.cookie = 0;
        }

        private bool EnsureActivateContextCreated()
        {
            lock (typeof(EnableThemingInScope))
            {
                if (!contextCreationSucceeded)
                {
                    // Pull manifest from the .NET Framework install
                    // directory

                    string assemblyLoc = null;

                    var fiop = new FileIOPermission(PermissionState.None);
                    fiop.AllFiles = FileIOPermissionAccess.PathDiscovery;
                    fiop.Assert();
                    try
                    {
                        assemblyLoc = typeof(object).Assembly.Location;
                    }
                    finally
                    {
                        CodeAccessPermission.RevertAssert();
                    }

                    string manifestLoc = null;
                    string installDir = null;
                    if (assemblyLoc != null)
                    {
                        installDir = Path.GetDirectoryName(assemblyLoc);
                        const string manifestName = "XPThemes.manifest";
                        manifestLoc = Path.Combine(installDir, manifestName);
                    }

                    if (manifestLoc != null && installDir != null)
                    {
                        enableThemingActivationContext = new ACTCTX();
                        enableThemingActivationContext.cbSize = Marshal.SizeOf(typeof(ACTCTX));
                        enableThemingActivationContext.lpSource = manifestLoc;

                        // Set the lpAssemblyDirectory to the install
                        // directory to prevent Win32 Side by Side from
                        // looking for comctl32 in the application
                        // directory, which could cause a bogus dll to be
                        // placed there and open a security hole.
                        enableThemingActivationContext.lpAssemblyDirectory = installDir;
                        enableThemingActivationContext.dwFlags = ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID;

                        // Note this will fail gracefully if file specified
                        // by manifestLoc doesn't exist.
                        hActCtx = CreateActCtx(ref enableThemingActivationContext);
                        contextCreationSucceeded = hActCtx != new IntPtr(-1);
                    }
                }

                // If we return false, we'll try again on the next call into
                // EnsureActivateContextCreated(), which is fine.
                return contextCreationSucceeded;
            }
        }

        // All the pinvoke goo...
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateActCtx(ref ACTCTX actctx);

        [DllImport("Kernel32.dll")]
        private static extern bool ActivateActCtx(IntPtr hActCtx, out uint lpCookie);

        [DllImport("Kernel32.dll")]
        private static extern bool DeactivateActCtx(uint dwFlags, uint lpCookie);

        void IDisposable.Dispose() { this.Dispose(true); }

        private struct ACTCTX
        {
            public int cbSize;
            public uint dwFlags;
            public string lpApplicationName;
            public string lpAssemblyDirectory;
            public string lpResourceName;
            public string lpSource;
            public ushort wLangId;
            public ushort wProcessorArchitecture;
        }
    }
}
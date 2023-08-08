using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading;

namespace Polaris2DEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void StartEngineDelegate(IntPtr hInstance); 
        
        public MainWindow()
        {
            InitializeComponent();

            // Load the Polaris2D.dll
            IntPtr pDll = LoadLibrary("Polaris2D.dll");
            if (pDll == IntPtr.Zero)
            {
                // Handle the error
                MessageBox.Show("Failed to load the DLL.");
                return;
            }

            // Get the address of StartEngine function from the DLL
            IntPtr pAddressOfFunctionToCall = GetProcAddress(pDll, "StartEngine");
            if (pAddressOfFunctionToCall == IntPtr.Zero)
            {
                // Handle the error
                MessageBox.Show("Failed to get started the Engine.");
                return;
            }

            // Convert the function pointer to a delegate
            StartEngineDelegate startEngine = (StartEngineDelegate)Marshal.GetDelegateForFunctionPointer(pAddressOfFunctionToCall, typeof(StartEngineDelegate));

            Thread engineThread = new Thread(() =>
            {
                // Call the function
                startEngine(System.Diagnostics.Process.GetCurrentProcess().Handle);
            });

            engineThread.Start();
        }
    }
}

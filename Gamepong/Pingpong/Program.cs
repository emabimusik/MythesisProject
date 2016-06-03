using System;
using System.Windows.Forms;
using EyeXFramework;
using Tobii.EyeX.Framework;
using EyeXFramework.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Pingpong
{
   
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
       // private  static EyeXHost eyeXHost;
       public static  Boolean cancel = false;
        [STAThread]
      
        static void Main()
        {
            using (var eyeXHost = new EyeXHost())
            {
                cancel = true;
                eyeXHost.Start();
                // Create a data stream: lightly filtered gaze point data.
                // Other choices of data streams include EyePositionDataStream and FixationDataStream.
                using (var lightlyFilteredGazeDataStream = eyeXHost.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered))
                {
                    // Start the EyeX host.
                    FileStream fs = new FileStream("kasperstandard.txt", FileMode.Create);
                    TextWriter tmp = Console.Out;
                    StreamWriter sw = new StreamWriter(fs);
                    Console.SetOut(sw);

                    while (cancel == true)
                   {
                        // Write the data to the console  
                        //lightlyFilteredGazeDataStream.Next += (s, e) => Console.WriteLine("Gaze point at ({0:0.0}, {1:0.0}) @{2:0}", e.X, e.Y, e.Timestamp);
                        lightlyFilteredGazeDataStream.Next += (s, e) => Console.WriteLine("{0:0.0}, {1:0.0}", e.X, e.Y);

                        // Let it run until a key is pressed.
                        Console.WriteLine("Listening for gaze data, press any key to exit...");
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());

                        Console.In.Read();
                       
                    }

                   
                    // First, save the standard output.
          
                    Console.SetOut(tmp);
                    //Console.WriteLine("Hello World");
                   // sw.Close();
                   
                }
                // eyeXHost.Dispose();

            }







        }
        
    }
}

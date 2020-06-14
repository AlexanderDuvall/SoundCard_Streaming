using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using NAudio.Wave;

namespace Vibin
{
    public class record
    {
        public static String filePath = @"C:\Users\Alex\Documents\Vibin Docs\test.wav";

        public record()
        {
        }


        public void play(IWaveProvider provider)
        {
            WaveOut wo = new WaveOut();
            //wo.DesiredLatency = 10;
            wo.Init(provider);
            wo.Play();
            //Thread.Sleep(50);
            wo.Stop();
            //    wo.PlaybackStopped += new EventHandler<StoppedEventArgs>(dispose);
        }

        public void dispose(Object s, StoppedEventArgs ss)
        {
        }

        public static void caller()
        {
            while (true)
            {
                Thread.Sleep(2000);
            }
        }


        public static void beginRecording()
        {
            Console.WriteLine("starting up");
            // Redefine the capturer instance with a new instance of the LoopbackCapture class
            WasapiLoopbackCapture CaptureInstance = new WasapiLoopbackCapture();
            BufferedWaveProvider bfp = new BufferedWaveProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

            // Redefine the audio writer instance with the given configuration
            // WaveFileWriter RecordedAudioWriter = new WaveFileWriter(filePath, CaptureInstance.WaveFormat);

            Server server = new Server();
            var x = new ArrayList();
            try
            {
                Console.WriteLine("BLOCK 1");
                // When the capturer receives audio, send to socket
                CaptureInstance.DataAvailable += (s, a) =>
                {
                    // add all consecutive bytes for 2s then send over                 
                   //independent async code...
                    //--------
                    server.sendData(a.Buffer);
                    //---------
                    
                    // async task
                    //await on adding the bytes to the arraylist
                    //await on sending bytes to the socket
                    x.AddRange(a.Buffer);

                };

                // When the Capturer Stops, dispose instances of the capturer and writer
                CaptureInstance.RecordingStopped += (s, a) =>
                {
                    CaptureInstance.Dispose();
                    Console.WriteLine("........DEAD.........");
                };

                // Start audio recording !
                CaptureInstance.StartRecording();
                
                //while (true) ;
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Exception Caught:... \n");
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Caught:... \n");
                Console.WriteLine(e.StackTrace);
            }
            
        }


        public static void Main(String[] args)
        {
            ThreadStart recordchild = new ThreadStart(beginRecording);
            Thread record = new Thread(recordchild);
            record.Start();
        }
    }
}
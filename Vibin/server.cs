using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic;
using NAudio.Wave;

namespace Vibin
{
    public class Server
    {
        // Establishes HOST UDP @ port 900
        public static UdpClient sender = new UdpClient(900);

        public Server()
        {
            //IP adress that will recieve the audio bytes (RECIPIENT - RECIP)
            sender.Connect("192.168.1.117", 11000);
        }
/**
 * sends audio bytes to the RECIP. 
 */
        public void sendData(byte[] sounddata)
        {
            try
            {
                sender.Send(sounddata, sounddata.Length);
            }
            catch (Exception send_exception)
            {
                Console.WriteLine(" Exception {0}", send_exception.Message);
            }
        }


        /**
        * Good
        */
        public static void listener()
        {
            //sets up local UDP client to listen for incoming "messages" with ipv6 ... i think
            IPAddress ipAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
            Console.WriteLine(Dns.Resolve(Dns.GetHostName()).AddressList[0]);
            IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, 11000);
            UdpClient listen = new UdpClient(ipLocalEndPoint);
            
            Console.WriteLine(listen.Client);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 0);
            record r = new record();

            try
            {
                /**
                 * PROBLEM AREA
                 *
                 * Here the udp client loops forever until it dies.
                 * It waits for new data to come into port 11000
                 * once it gets that new data, it plays it.
                 *
                 * There are leaks when playing the data. When attempting to discard the data, it wont always work, and
                 * the data will continuously loop.
                 *
                 * To be written in C++
                 */
                WaveFormat f = WaveFormat.CreateIeeeFloatWaveFormat(47000, 2);
                while (true)
                {Console.WriteLine("waiting");
                    byte[] recieve_byte_array = listen.Receive(ref groupEP);
                    IWaveProvider provider = new RawSourceWaveStream(
                        new MemoryStream(recieve_byte_array), f);
                    Thread.Sleep(10);
                    r.play(provider);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.StackTrace);
            }
        }

        public static void play(IWaveProvider provider)
        {
            WaveOut wo = new WaveOut();
            wo.Init(provider);
            wo.Play();
        }
/**
        public static void Main(String[] args)
        {
            ThreadStart s = new ThreadStart(listener);
            Thread t = new Thread(s);
            t.Start();
        }
    **/}
}
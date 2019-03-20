using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TempratureController
{
    public class Warning
    {
        public static void RaiseWarning (int warningValue,DateTime warningTime)
        {
            Warn warn = new Warn();
            Notify notification = new Notify();
            notification.Send(warn);
            warn.notificationMessage("The temp has reached: " + warningValue+" at "+warningTime);
        }
    }
    public class Warn
    {
        public delegate void warnMessage (string msg);
        public event warnMessage sendMessage;
        public void notificationMessage (string message)
        {
             if (sendMessage != null)
                sendMessage(message);

        }
    }
    public class Notify
    {
        public void sendMessages (string msg)
        {
                Console.WriteLine("Warning!!" + msg);
        }

        public void Send (Warn send)
        {
                send.sendMessage += sendMessages;
        }
    }
    class Program
    {
        public static void Options()
        {
            Console.WriteLine("Enter Your option\n");
            Console.WriteLine("1)Check for the treshold values\n");
            Console.WriteLine("2)Retreive");

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    checkTemp();
                    Options();
                    break;
                case 2:
                    Retrieve();
                    Options();
                    break;
                default:
                    Console.WriteLine("Please Enter correct value\n");
                    Options();
                    break;
                case 3:
                    return;
                    
            }
        }
        public static void Main (string[] args)
        {
            Options();
        }
         static void checkTemp()
         {  
            Random rnd = new Random();
            Console.WriteLine("Enter the Min treshold: ");
            int min = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the Max treshold: ");
            int max = Convert.ToInt32(Console.ReadLine());
            int[] a = new int[100];
            int temp;
            for (int i = 0; i < 10; i++)
            {

                temp = rnd.Next(min-5, max+5);
                a[i] = temp;
                

            }
            for(int i=0;i<10;i++)
            {
                Thread.Sleep(300);
                if (a[i]<=min||a[i]>=max)
                {
                    using (TempratueControllerEntities context = new TempratueControllerEntities())
                    {
                        TempTime tmp = new TempTime
                        {
                            Temprature = a[i],
                            Time = DateTime.Now
                        };
                        context.TempTimes.Add(tmp);
                        context.SaveChanges();

                    }
                        int warningValue = a[i];
                        DateTime warningTime = DateTime.Now.ToLocalTime();
                        Warning.RaiseWarning(warningValue,warningTime);

                }

            }

            Console.ReadKey();
        }
        static void Retrieve()
        {
            Console.WriteLine("Enter Your option\n");
            Console.WriteLine("1)MINIMUM\n");
            Console.WriteLine("2)MAXIMUM");
            int option = Convert.ToInt32(Console.ReadLine());
            using (var context = new TempratueControllerEntities())
            {
                if (option == 2)
                {
                    var values = context.TempTimes
                        .OrderByDescending(a => a.Temprature)
                       .First();
                    Console.WriteLine("The Max value recorded was: " + values.Temprature + " at time: " + values.Time);
                }
                if (option == 1)
                {
                    var values = context.TempTimes
                        .OrderBy(a => a.Temprature)
                       .First();
                    Console.WriteLine("The Min value recorded was: " + values.Temprature + " at time: " + values.Time);
                }
            }
        }
    }

}

/* public class GFG<T>
    {

        // private data members 
        public T min;
        public T max;

        // using properties 
        public T value
        {

            // using accessors 
            get
            {
                return this.min;
                return this.min;

            }
            set
            {
                this.min= value;
            }
        }
    }*/

using System;

namespace DbFirstConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Tasks.Task<string> longRunningTask = AccessTheWebAsync();
            //ServiceReference1.Service1Client clt = new ServiceReference1.Service1Client();
            //System.Threading.Tasks.Task<string> getStringTask = clt.GetDataAsync(5);

            //string urlContents = await getStringTask;

            Console.WriteLine("Hello World!");
        }

        static async System.Threading.Tasks.Task<string> AccessTheWebAsync()
        {
            ServiceReference1.Service1Client clt = new ServiceReference1.Service1Client();
            System.Threading.Tasks.Task<string> getStringTask = clt.GetDataAsync(5);

            // You need to add a reference to System.Net.Http to declare client.  
            //HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the  
            // task you'll get a string (urlContents).  
            //Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.  
            //DoIndependentWork();

            // The await operator suspends AccessTheWebAsync.  
            //  - AccessTheWebAsync can't continue until getStringTask is complete.  
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync.  
            //  - Control resumes here when getStringTask is complete.   
            //  - The await operator then retrieves the string result from getStringTask.  
            string urlContents = await getStringTask;

            // The return statement specifies an integer result.  
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value.  
            return urlContents;
        }
    }
}
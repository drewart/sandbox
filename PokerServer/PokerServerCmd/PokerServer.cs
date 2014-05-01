using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PokerHands;
using PokerServerCmd;

namespace PokerServer
{
    public class PokerService
    {
        private static int maxRequestsHandlers = 5;

        private static int requestHandlerID = 0;

        private static HttpListener listener;

        private static void RequestHandler(IAsyncResult result)
        {
            Console.WriteLine("{0}: Activated", result.AsyncState);
           

            try
            {
                HttpListenerContext context = listener.EndGetContext(result);
                Thread.Sleep(2000);

                Console.WriteLine("HTTP Method: {0}",context.Request.HttpMethod);

                string dataText = new StreamReader(context.Request.InputStream,context.Request.ContentEncoding).ReadToEnd();
                Console.WriteLine("dataText: " + dataText);

                string resultData = string.Empty;
                bool jsonResult = false;


                try
                {
                    // ajax
                    if (dataText.Trim().StartsWith("{"))
                    {
                        jsonResult = true;
                        Hands jsonHands = Hands.JsonDeserialize(dataText);

                        PlayerHand[] playerHands = new PlayerHand[jsonHands.hands.Count];

                        for(int i = 0; i < jsonHands.hands.Count; i++)
                        {
                            playerHands[i] = new PlayerHand(jsonHands.hands[i].name,string.Join(",",jsonHands.hands[i].hand));
                        }


                        Array.Sort(playerHands);

                        resultData = playerHands.Last().PlayerName + " wins";

                    } //xml
                    else if (dataText.Trim().StartsWith("<"))
                    {
                        Hands jsonHands = Hands.XmlDeserialize(dataText);

                        PlayerHand[] playerHands = new PlayerHand[jsonHands.hands.Count];

                        for (int i = 0; i < jsonHands.hands.Count; i++)
                        {
                            playerHands[i] = new PlayerHand(jsonHands.hands[i].name, string.Join(",", jsonHands.hands[i].hand));
                        }


                        Array.Sort(playerHands);

                        resultData = playerHands.Last().PlayerName + " wins";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception:");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine();
                    resultData = e.Message;
                }




                context.Response.StatusCode = 200;
                context.Response.StatusDescription = "OK";

                context.Response.ContentType = "text/xml";
                context.Request.Headers.Add("Access-Control-Allow-Origin","*");
                context.Response.ContentEncoding = Encoding.UTF8;

                StreamWriter sw = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
                if (jsonResult)
                    sw.WriteLine("{ \"result\" : " + resultData + "}");
                else
                    sw.WriteLine("<results><result>" + resultData + "</result></results>");
                sw.Flush();
                
                context.Response.Close();


                // Get the data from the HTTP stream
                /*var body = new StreamReader(context.Request.InputStream).ReadToEnd();

                byte[] b = Encoding.UTF8.GetBytes("ACK");
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);
                //context.Response.Close();
                Console.WriteLine("body"+body);
                //var output = context.Response.OutputStream;
                //output.Write(b, 0, b.Length);
                //context.Response.Close();


                if (context.Request.ContentType == "application/json")
                    Console.WriteLine("request json");
                else if (context.Request.ContentType == "text/xml")
                    Console.WriteLine("xml request");


                //StreamReader reader = new StreamReader(context.Request.InputStream,context.Request.ContentEncoding);

                //string post = reader.ReadToEnd();

                //Console.WriteLine("posted: {0}",post);


                Console.WriteLine("{0}: processing HTTP request from {1} ({2}) {3}.",result.AsyncState,context.Request.UserHostName,context.Request.RemoteEndPoint,context.Request.UserHostAddress);
                //context.Response.Headers.
                context.Response.ContentType = "text/xml";
                context.Response.ContentEncoding = Encoding.UTF8;


                

                StreamWriter sw = new StreamWriter(context.Response.OutputStream, Encoding.UTF8);
                sw.WriteLine("<results><result>foo wins</result></results>");
                sw.Flush();


                context.Response.Close();
                

                Console.WriteLine("{0}: sent resonce.",result.AsyncState);*/
            }
            catch(ObjectDisposedException)
            {
                Console.WriteLine("{0}: HttpListener disposed--shutting down.",result.AsyncState);
            }
            finally
            {
                if (listener.IsListening)
                {
                    Console.WriteLine("{0}: creating new request handler",result.AsyncState);
                    listener.BeginGetContext(RequestHandler,"RequestHandler_" + Interlocked.Increment(ref requestHandlerID));
                }
            }
        }



        static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                System.Console.WriteLine("HttpListener not supported");
                return;
            }

            using (listener = new HttpListener())
            { 
                listener.Prefixes.Add("http://localhost:9000/poker/");

                listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                
                listener.Start();
                Console.WriteLine("Server started");
                for (int cnt = 0; cnt < maxRequestsHandlers; cnt++)
                {
                    listener.BeginGetContext(RequestHandler, "RequestHandler_" + Interlocked.Increment(ref requestHandlerID));

                }

                Console.WriteLine("Press Enter to stop HTTP server");
                Console.ReadLine();
                listener.Stop();
                listener.Abort();
            }
        }
    }
}
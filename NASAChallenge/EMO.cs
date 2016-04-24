using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading; 
namespace NASAChallenge
{
    public class EMO
    {
        public static  String getEmo(byte[] img) {
            WebClient wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/octect-stream");
            wc.Headers.Add("Ocp-Apim-Subscription-Key", "7b3fe9298eb040288292c76f30f12eb0");
            try
            {

                Console.WriteLine(img.Length);
                //var res = wc.UploadData(new Uri("https://api.projectoxford.ai/emotion/v1.0/recognize"), img);
                //  var res = wc.UploadData(new Uri("https://api.projectoxford.ai/emotion/v1.0/recognize"), img);
               // var res = wc.UploadFile(new Uri("https://api.projectoxford.ai/emotion/v1.0/recognize"), "POST", @"C:/Users/Dark/Pictures/AC91.tmp.png");
               // var result = wc.UploadString("https://api.projectoxford.ai/emotion/v1.0/recognize", "{ \"url\": \"https://scontent-amt2-1.xx.fbcdn.net/hphotos-xft1/v/t1.0-9/10991161_1411998482434265_7232853448028132767_n.jpg?oh=ed1b9439c945354364af43cb3fcac734&oe=57AF0F81\" }"); 
               //string result = System.Text.Encoding.UTF8.GetString(res);
               // Console.WriteLine(result);
                

                var request = WebRequest.Create("https://api.projectoxford.ai/emotion/v1.0/recognize") as HttpWebRequest;
                request.Headers["Ocp-Apim-Subscription-Key"] = "7b3fe9298eb040288292c76f30f12eb0";
                request.ContentType = "application/octet-stream";
                request.Method = "POST";
                request.ContentLength = img.Length;
                var requestStream = request.GetRequestStream();
                
                requestStream.Write(img, 0, img.Length);
                requestStream.Close();
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    var reader = new StreamReader(response.GetResponseStream());
                    var data = reader.ReadToEnd();
                    Console.WriteLine(data);
                    return data; 
                }
    
                
            
            }
            catch (WebException e) {
               
                Console.WriteLine(e);
               
                return ""; 
            };
        }
    }
}

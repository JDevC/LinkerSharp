using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Models;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace LinkerSharp.Common.Endpoints.FTP.Connectors
{
    public class DefaultFTPConnector : IFTPInConnector, IFTPOutConnector
    {
        public bool GetData(string Endpoint, out string StatusCode, out string Data)
        {
            bool Result = false;
            Data = "";

            var Request = WebRequest.Create(Endpoint);

            using (var Response = Request.GetResponse() as HttpWebResponse)
            {
                StatusCode = Response.StatusCode.ToString();
                Result = Response.StatusCode.Equals(HttpStatusCode.OK);

                if (Result)
                {
                    using (var DataStream = Response.GetResponseStream())
                    {
                        Data = this.ExtractFromStream(DataStream);
                    }
                }
                else
                {
                    Data = Response.StatusDescription;
                }
            }

            return Result;
        }

        public bool SendData(string Endpoint, TransactionDTO Transaction)
        {
            var Result = false;

            string TempFilePath = Path.Combine(Path.GetTempPath(), Transaction.ResponseMessage.Name);

            try
            {
                using (var FileStream = new StreamWriter(TempFilePath))
                {
                    FileStream.WriteLine(Transaction.ResponseMessage.Content);
                }
            }
            catch (Exception ex)
            {
                Transaction.ResponseMessage.Error.Reason = $"Temp message couldn't be created: {ex.Message}";
            }

            if (System.IO.File.Exists(TempFilePath))
            {
                Result = this.AppendToStream(Endpoint, TempFilePath, Transaction);

                if (Result)
                {
                    System.IO.File.Delete(TempFilePath);
                }
            }

            return Result;
        }

        #region Private Methods: Helpers
        private string ExtractFromStream(Stream DataStream)
        {
            //using (var DataReader = new StreamReader(DataStream))
            //{
            //    return DataReader.ReadToEnd();
            //}

            var StrBuilder = new StringBuilder();
            byte[] buffer = new byte[8192];
            int counter = 0;

            do
            {
                counter = DataStream.Read(buffer, 0, buffer.Length);
                if (counter != 0)
                {
                    StrBuilder.Append(Encoding.ASCII.GetString(buffer, 0, counter));
                }
            }
            while (counter > 0);

            return StrBuilder.ToString();
        }

        private bool AppendToStream(string Endpoint, string TempFilePath, TransactionDTO Transaction)
        {
            bool Result = false;
            try
            {
                var Request = WebRequest.Create($"{Endpoint}/{Transaction.ResponseMessage.Name}") as FtpWebRequest;
                Request.Method = WebRequestMethods.Ftp.UploadFile;

                using (var FileStream = System.IO.File.OpenRead(TempFilePath))
                {
                    using (var FtpStream = Request.GetRequestStream())
                    {
                        FileStream.CopyTo(FtpStream);

                        Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Transaction.ResponseMessage.Error.Reason = $"Message couldn't be sent: {ex.Message}";
            }

            return Result;
        }
        #endregion
    }
}

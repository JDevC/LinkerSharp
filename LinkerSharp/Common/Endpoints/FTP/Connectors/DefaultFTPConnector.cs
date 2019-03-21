using LinkerSharp.Common.Endpoints.FTP.IFaces;
using LinkerSharp.Common.Models;
using LinkerSharp.TransactionHeaders;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace LinkerSharp.Common.Endpoints.FTP.Connectors
{
    public class DefaultFTPConnector : IFTPInConnector, IFTPOutConnector
    {
        public bool GetData(string Endpoint, Dictionary<string, object> Params, out string StatusCode, out List<TransmissionMessageDTO> Data)
        {
            bool Result = false;
            Data = new List<TransmissionMessageDTO>();

            if (Params.ContainsKey(Headers.JUST_IN) && bool.TryParse(Params[Headers.JUST_IN].ToString(), out bool Val) && Val)
            {
                Result = this.GetSingleFile(Endpoint, out StatusCode, out TransmissionMessageDTO DataUnit);
                Data.Add(DataUnit);
            }
            else
            {
                Result = this.GetMultiFiles(Endpoint, out StatusCode, out Data);
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
        private bool GetSingleFile(string Endpoint, out string StatusCode, out TransmissionMessageDTO Data)
        {
            var Result = false;
            StatusCode = "";

            var Request = WebRequest.Create(Endpoint) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.DownloadFile;

            using (var Response = Request.GetResponse() as FtpWebResponse)
            {
                StatusCode = Response.StatusCode.ToString();
                Result = Response.StatusCode.Equals(HttpStatusCode.OK);

                if (Result)
                {
                    using (var DataStream = Response.GetResponseStream())
                    {
                        Data = new TransmissionMessageDTO() { Content = this.ExtractFromStream(DataStream) };
                    }
                }
                else
                {
                    Data = new TransmissionMessageDTO();
                    Data.Error.Code = StatusCode;
                    Data.Error.Reason = Response.StatusDescription;
                }
            }

            return Result;
        }

        private bool GetMultiFiles(string Endpoint, out string StatusCode, out List<TransmissionMessageDTO> Data)
        {
            StatusCode = "";
            Data = new List<TransmissionMessageDTO>();
            var Result = false;

            #region File List
            var Request = WebRequest.Create(Endpoint) as FtpWebRequest;
            Request.Method = WebRequestMethods.Ftp.ListDirectory;

            var Files = new List<string>();
            using (var Response = Request.GetResponse() as FtpWebResponse)
            {
                StatusCode = Response.StatusCode.ToString();
                Result = Response.StatusCode.Equals(HttpStatusCode.OK);

                if (Result)
                {
                    using (var DataStream = new StreamReader(Response.GetResponseStream()))
                    {
                        string Line = DataStream.ReadLine();
                        while (!string.IsNullOrEmpty(Line))
                        {
                            Files.Add(Line);
                            Line = DataStream.ReadLine();
                        }
                    }
                }
                else
                {
                    var DataUnit = new TransmissionMessageDTO();
                    DataUnit.Error.Code = StatusCode;
                    DataUnit.Error.Reason = Response.StatusDescription;

                    Data.Add(DataUnit);
                    return Result;
                }
            }
            #endregion

            foreach (var File in Files)
            {
                this.GetSingleFile($"{Endpoint}/{File}", out StatusCode, out TransmissionMessageDTO Message);                
                Data.Add(Message);
            }

            return Result;
        }

        private string ExtractFromStream(Stream DataStream)
        {
            var StrBuilder = new StringBuilder();
            var buffer = new byte[8192];
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

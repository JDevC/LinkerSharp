using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LinkerSharp.Common.EndpointClasses.Interfaces;
using LinkerSharp.Common.Models;

namespace LinkerSharp.Common.EndpointClasses.Consumers
{
    internal sealed class FILEConsumer : BaseEndpoint, IConsumer
    {
        private static readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(typeof(FILEConsumer));

        public FILEConsumer(string Path)
        {
            this.Endpoint = Path;
        }

        //public override List<TransmissionMessageDTO> ReceiveMessages()
        public List<TransactionDTO> ReceiveMessages()
        {
            var Result = new List<TransactionDTO>();

            if (this.GetFiles(this.Endpoint, out string[] FileNames))
            {
                int ID = 1;
                foreach (var FileName in FileNames)
                {
                    var DTO = base.CreateTransaction(ID, FileName, FileName.Split("\\".ToCharArray()).Last(), this.Params);

                    try
                    {
                        DTO.RequestMessage.Content = File.ReadAllText(FileName);

                        this.Success = true;
                    }
                    catch (DirectoryNotFoundException DNotFoundEx)
                    {
                        EndpointTools.SetErrorReason(DTO, "", $"Endpoint not found: {DNotFoundEx.Message}", DNotFoundEx.StackTrace, _Logger);
                    }
                    catch (ArgumentException ArgEx)
                    {
                        EndpointTools.SetErrorReason(DTO, "", $"Incorrect endpoint (value -> {this.Endpoint}): {ArgEx.Message}", ArgEx.StackTrace, _Logger);
                    }
                    catch (UnauthorizedAccessException NotAllowedEx)
                    {
                        EndpointTools.SetErrorReason(DTO, "", $"Endpoint '{this.Endpoint}' cannot be reached: {NotAllowedEx.Message}", NotAllowedEx.StackTrace, _Logger);
                    }

                    Result.Add(DTO);
                    ID++;
                }
            }

            return Result;
        }

        #region Private Methods
        private bool GetFiles(string Endpoint, out string[] Files)
        {
            Files = new string[] { };
            try
            {
                Files = Directory.GetFiles(this.Endpoint);
            }
            catch (DirectoryNotFoundException DNotFoundEx)
            {
                _Logger.Error($"Endpoint not found: {DNotFoundEx.Message}");
            }
            catch (ArgumentException ArgEx)
            {
                _Logger.Error($"Incorrect endpoint (value -> {this.Endpoint}): {ArgEx.Message}");
            }
            catch (UnauthorizedAccessException NotAllowedEx)
            {
                _Logger.Error($"Endpoint '{this.Endpoint}' cannot be reached: {NotAllowedEx.Message}");
            }

            return Files.Any();
        }
        #endregion
    }
}

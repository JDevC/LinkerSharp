
namespace LinkerSharp.Common.EndpointClasses.Interfaces
{
    public interface IFTPConnector
    {
        /// <summary>
        /// Se encarga de la petición al servidor de FTP
        /// </summary>
        /// <param name="Endpoint">URL del servidor FTP.</param>
        /// <param name="StatusCode">Indica el código de estado concreto de la petición.</param>
        /// <param name="Data">Devuelve los datos requeridos si ha ido bien; en caso contrario devuelve la causa del error.</param>
        /// <returns>La petición se ha completado con éxito o no.</returns>
        bool GetData(string Endpoint, out string StatusCode, out string Data);
    }
}

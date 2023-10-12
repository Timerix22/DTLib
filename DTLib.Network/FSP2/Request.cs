namespace DTLib.Network.FSP2;

public enum RequestType
{
    Message,
    DownloadFile,
    UploadFile
}

public record Request(RequestType Type)
{
    
}
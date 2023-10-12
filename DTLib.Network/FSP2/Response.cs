namespace DTLib.Network.FSP2;

public enum ResponseStatus
{
    OK,
    InvalidRequest,
    AccessDenied
}

public record Response(ResponseStatus Status)
{ 
    
}
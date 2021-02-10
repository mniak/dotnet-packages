[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/mniak/dotnet-packages/RequestCompression)](https://github.com/mniak/dotnet-packages/actions?query=workflow%3ARequestCompression+event%3Apush+branch%3Amaster)

Request Compression
=======================

Enables `HttpClient` to make compressed requests and enables the server to understand compressed requests.

### Motivation
Well, sometimes the request payload is so big that it ends up affecting performance or stumbling on reverse proxy limits.

Nginx, for example, has a default request payload limit (`client_max_body_size`) of 1 MB.

There is a lot of resources on how to enable [Response Compression in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression), but not inverse. For _Request Compression_ the resources are scarce.


### Implemented compression algorithms
| Algorithm | Compression | Decompression |
|-----------|-------------|---------------|
| Gzip      | [![Nuget](https://img.shields.io/nuget/v/Mniak.Extensions.RequestCompression.Gzip)](https://www.nuget.org/packages/Mniak.Extensions.RequestCompression.Gzip/) | [![Nuget](https://img.shields.io/nuget/v/Mniak.Extensions.RequestDecompression.Gzip)](https://www.nuget.org/packages/Mniak.Extensions.RequestDecompression.Gzip/) |

## Usage

The examples consider Gzip as the compression algorithm.

### Server (decompression)

Install the package:
```ps
Install-Package Mniak.Extensions.RequestDecompression.Gzip
```

On `Startup.cs` add to `ConfigureServes`:
```cs
services.AddRequestDecompression()
    .AddGzipDecompression();
```

and to `Configure` before any middleware that reads the request body:
```cs
app.UseRequestDecompression();
```

### Client (compression)
#### With ASP.NET Core Typed Clients

Install the package:
```ps
Install-Package Mniak.Extensions.RequestCompression.Gzip
```

Suppose that you have a typed client `MyApiClient` that implements the interface `IApiClient`, then add this:
```cs
services.AddHttpClient<IApiClient, MyApiClient>()
    //... existing HttpClient configuration
    .AddGzipCompression() // <-- Add this line 
    //... more existing configuration
    ;
```

#### With bare HttpClients
Install the package:
```ps
Install-Package Mniak.HttpClient.RequestCompression.Gzip
```

And set the compression handler in the `HttpClient` constructor
```
var compressionHandler = new GzipMessageHandler().WithNewInnerHandler();
using var httpClient = new HttpClient(compressionHandler);
```

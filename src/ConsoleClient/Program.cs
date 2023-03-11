using System.Net;

Console.WriteLine("Hello, World!");
Console.WriteLine("Downloading a 100MB file...");


var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;

DownloadFileWithToken(cancellationToken);



Console.WriteLine("Press C, to cancel the download");
if (Console.ReadKey().KeyChar == 'c')
{
    cancellationTokenSource.Cancel();
    Console.WriteLine("\n\rCancellation Sent");
}


Console.ReadLine();


 void DownloadFileWithToken(CancellationToken token)
{
    const string file1Gb = "https://speed.hetzner.de/1GB.bin";

    var webClient = new WebClient();

    token.Register(() => webClient.CancelAsync());
    webClient.DownloadProgressChanged += (sender, e) =>
    {
        Console.WriteLine(e.BytesReceived);
    };

    webClient.DownloadDataCompleted += (sender, e) =>
    {


        if (!e.Cancelled)
        {
            byte[] data = (byte[])e.Result;
            Console.WriteLine($"\nDownload Completed: {data.Length}");
        }
        else
        {

            Console.WriteLine("\nDownload canceled");
        }
    };

    webClient.DownloadDataAsync(new Uri(file1Gb));

}

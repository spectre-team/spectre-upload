using System.Net.Http;
using System.Threading.Tasks;

namespace upload_api.Services
{
    public class DownloadService: IDownloadService
    {
        public async Task DownloadAsync(string url, string destination)
        {
            using (var client = new HttpClient())
            using (var content = await client.GetAsync(url))
            using (var outputFile = System.IO.File.Create(destination))
                await content.Content.CopyToAsync(outputFile);
        }
    }
}

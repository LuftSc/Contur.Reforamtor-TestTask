using System.Diagnostics;
using XSLTReformator.Abstracts;

namespace XSLTReformator.Services
{
    public class XSLTProcessor : IXSLTProcessor
    {
        public async Task<string> TransformAsync(string xmlDataPath, string xsltPath
            , CancellationToken cancellationToken = default)
        {
            var resultFile = Path.Combine(Directory.GetCurrentDirectory(), "XML/Employees.xml");

            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = $"-jar /opt/saxonhe/saxon-he-12.0.jar -s:{xmlDataPath} -xsl:{xsltPath} -o:{resultFile}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            var tcs = new TaskCompletionSource<bool>();

            process.Exited += (sender, args) =>
            {
                if (process.ExitCode != 0)
                {
                    var error = process.StandardError.ReadToEnd();
                    tcs.TrySetException(new Exception($"Saxon Error (exit code {process.ExitCode}): {error}"));
                }
                else tcs.TrySetResult(true);
                process.Dispose();
            };

            process.Start();

            var outputTask = process.StandardOutput.ReadToEndAsync(cancellationToken);

            await tcs.Task;

            return await File.ReadAllTextAsync(resultFile, cancellationToken);
        }
    }
}

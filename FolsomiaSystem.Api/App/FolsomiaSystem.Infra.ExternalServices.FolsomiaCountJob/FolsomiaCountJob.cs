using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Domain.Enums;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;

namespace FolsomiaSystem.Infra.ExternalServices.FolsomiaCountJob
{

    public class FolsomiaCountJob : IFolsomiaCountJob
    {
         protected string ParameterBackground(FolsomiaCount backgroundImage)
        {

            if (backgroundImage.BackgroundImage == BackgroundImage.Dark)
            {
                return "-d";
            } else if (backgroundImage.BackgroundImage == BackgroundImage.Light)
            {
                return "-l";
            }
            return "";
        }

        protected int ResultadoContagem (JObject res)
        {
            JToken token = res["count_res"];

            if (token!= null && token?.Type != JTokenType.Null)
            {
                return int.Parse(token.ToString());
            }
            return 0;
        }

        protected StatusLog ResultadoStatus(JObject res)
        {
            JToken token = res["status"];

            if (token!=null && token?.Type != JTokenType.Null)
            {
                if (token.ToString() == "500") return StatusLog.fail;
                else return StatusLog.success;
            }
            return StatusLog.fail;
        }

        protected string ResultadoError(JObject res)
        {
            JToken token = res["error"];

            if (token!=null && token?.Type != JTokenType.Null)
            {
                return token.ToString();
            }
            return null;
        }


        private Task<string> ExecuteJob(FolsomiaCount folsomiaCount, string localPythonFolsomiaCount)
        {
            var tcs = new TaskCompletionSource<string>();
            string args = localPythonFolsomiaCount + " " + 
                          folsomiaCount.ImageFolsomiaURL + " "+
                          ParameterBackground(folsomiaCount)+ 
                          " -ir " + 
                          folsomiaCount.ImageFolsomiaURL;

            var process = new Process
            {
                StartInfo = {
                FileName = "python",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true

            },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();
                tcs.SetResult(output);
                process.Dispose();
            };
            process.Start();
            process.WaitForExit();
            process.Close();
            return tcs.Task;

        }

        private FolsomiaCount ResultImage64(FolsomiaCount folsomiaCount)
        {

            string base64 = ConvertToBase64(DeleteFileSaveMemory(folsomiaCount.ImageFolsomiaURL));
            folsomiaCount.FileResult.FileAsBase64 = "data:image/png;base64,"+ base64;

            return folsomiaCount;

        }


        private  string ConvertToBase64( Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        private Stream DeleteFileSaveMemory(string path)
        {
            Stream retVal = null;

            if (File.Exists(path))

            {

                byte[] fileData = File.ReadAllBytes(path);

                retVal = new MemoryStream(fileData);

                File.Delete(path);

            }

            return retVal;
        }


        public Task<FolsomiaCount> FolsomiaCountJobPython(FolsomiaCount folsomiaCount, string localPythonFolsomiaCount)
        {
            var taskFolsomiaCount = new TaskCompletionSource<FolsomiaCount>();


            try
            {
                Task<string> output = ExecuteJob(folsomiaCount, localPythonFolsomiaCount);
                JObject res = JObject.Parse(output.Result.ToString());
                folsomiaCount.TotalCountFolsomia = ResultadoContagem(res);
                folsomiaCount.AuditLog.StatusLog = ResultadoStatus(res);
                folsomiaCount.AuditLog.MessageLog = ResultadoError(res);
                folsomiaCount = ResultImage64(folsomiaCount);
                taskFolsomiaCount.SetResult(folsomiaCount);
                return taskFolsomiaCount.Task;

            }catch{
                folsomiaCount.AuditLog.MessageLog = "The job did not run, check the system log files.";
                folsomiaCount.AuditLog.StatusLog = StatusLog.fail;
                return taskFolsomiaCount.Task;
            }

        }
    }
}

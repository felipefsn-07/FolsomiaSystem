using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using FolsomiaSystem.Domain.Entities;
using FolsomiaSystem.Domain.Enums;
using Newtonsoft.Json.Linq;

namespace FolsomiaSystem.Infra.ExternalServices.FolsomiaCountJob
{

    public class FolsomiaCountJob : IFolsomiaCountJob
    {
  
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
                if (token.ToString() != "500") return StatusLog.fail;
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


        private Task<string> ExecuteJob(string imgIn, string imgOut, string localPythonFolsomiaCount)
        {
            var tcs = new TaskCompletionSource<string>();

            var process = new Process
            {
                StartInfo = {
                FileName = "python",
                Arguments = localPythonFolsomiaCount + " " + imgIn + " -ir " + imgOut,
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

        public Task<FolsomiaCount> FolsomiaCountJobPython(string imgIn, string imgOut, string localPythonFolsomiaCount)
        {
            FolsomiaCount folsomiaCount = new FolsomiaCount
            {
                ImageFolsomiaURL = imgIn,
                ImageFolsomiaOutlinedURL = imgOut,
            };
            var taskFolsomiaCount = new TaskCompletionSource<FolsomiaCount>();


            try
            {
                Task<string> output = ExecuteJob(imgIn, imgOut, localPythonFolsomiaCount);
                JObject res = JObject.Parse(output.Result.ToString());
                folsomiaCount.TotalCountFolsomia = ResultadoContagem(res);
                folsomiaCount.StatusLog = ResultadoStatus(res);
                folsomiaCount.MessageLog = ResultadoError(res);
                taskFolsomiaCount.SetResult(folsomiaCount);
                return taskFolsomiaCount.Task;

            }catch{
                folsomiaCount.MessageLog = "The job did not run, check the system log files.";
                folsomiaCount.StatusLog = StatusLog.fail;
                return taskFolsomiaCount.Task;
            }

        }
    }
}

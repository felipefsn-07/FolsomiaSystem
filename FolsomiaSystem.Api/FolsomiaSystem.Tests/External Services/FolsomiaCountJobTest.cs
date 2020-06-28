using FolsomiaSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FolsomiaSystem.Infra.ExternalServices.FolsomiaCountJob;
using FolsomiaSystem.Application.Interfaces.ExternalServices;
using System.Configuration;

namespace FolsomiaSystem.Tests.External_Services
{
    public class FolsomiaCountJobTest
    {
        #region Constants

        private const string IMG_OUT = "C:/Users/cs319260/Desktop/TCC/Sistema/FolsomiaSystem.SharedFolder/Imagens/Testes/testWebApi.jpg";
        private const string IMG_IN = "C:/Users/cs319260/Desktop/TCC/Sistema/FolsomiaSystem.Job/App/test_inputs_outputs/inputs/test_in_1.jpg";

        #endregion

        [Fact]

        public void FolsomiaCountJobPythonTest()
        {
            string LOCAL_PYTHON_FOLSOMIA_COUNT = ConfigurationManager.AppSettings["LOCAL_PYTHON_FOLSOMIA_COUNT"];
            FolsomiaCountJob job = new FolsomiaCountJob();
            var folsomia = job.FolsomiaCountJobPython(IMG_IN, IMG_OUT, LOCAL_PYTHON_FOLSOMIA_COUNT);
            Assert.True(folsomia?.TotalCountFolsomia != null);
        }


    }
}

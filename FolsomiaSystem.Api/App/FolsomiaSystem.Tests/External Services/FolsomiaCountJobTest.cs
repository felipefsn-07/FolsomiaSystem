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


        private readonly FolsomiaCount _folsomia= new FolsomiaCount
        {
            ImageFolsomiaURL = "/Imagens/Inputs/test_1.jpg",

        };

        #endregion

        [Fact]

        public void FolsomiaCountJobPythonTest()
        {
            string LOCAL_PYTHON_FOLSOMIA_COUNT = ConfigurationManager.AppSettings["LOCAL_PYTHON_FOLSOMIA_COUNT"];
            FolsomiaCountJob job = new FolsomiaCountJob();
            var folsomia = job.FolsomiaCountJobPython(_folsomia, LOCAL_PYTHON_FOLSOMIA_COUNT);
            Assert.True(folsomia?.Result.TotalCountFolsomia != null);
        }


    }
}

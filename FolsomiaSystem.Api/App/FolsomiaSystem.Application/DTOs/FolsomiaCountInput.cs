
using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Application.DTOs
{
    public class FolsomiaCountInput
    {
        public string IdTest { get; set; }

        public Nullable<BackgroundImage> BackgroundImage { get; set; }

        public string FileAsBase64 { get; set; }
    }
}
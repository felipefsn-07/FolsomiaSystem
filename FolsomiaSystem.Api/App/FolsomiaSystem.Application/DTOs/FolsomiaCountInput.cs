
using FolsomiaSystem.Domain.Enums;
using System;

namespace FolsomiaSystem.Application.DTOs
{
    public class FolsomiaCountInput
    {
        public string IdTest { get; set; }

        public string ImageFolsomiaURL { get; set; }

        public string ImageFolsomiaOutlinedURL { get; set; }

        public Nullable<BackgroundImage> BackgroundImage { get; set; }
    }
}
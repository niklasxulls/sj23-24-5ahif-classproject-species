using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiveSpecies.Application.Interfaces.Services.Data;
public interface IMediaService
{

    Task<List<string>> UploadMedia(IFormFileCollection formFiles);
}

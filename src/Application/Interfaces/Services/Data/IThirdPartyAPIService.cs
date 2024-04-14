using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Infrastructure.Models;

namespace DiveSpecies.Application.Interfaces.Services.Data;
public interface IThirdPartyAPIService
{

    Task<List<RapidApiFishSchema>> RequestAllFishData();
}

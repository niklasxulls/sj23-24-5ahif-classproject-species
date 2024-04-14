using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiveSpecies.Rest.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        //here is no simple consturctur inilization possible, because HttpContext is only provided during a request. Therefore it will return
        //null during the instantiation of a controller, but with a get prop it gets possible, since the Mediator is only called during a request
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}

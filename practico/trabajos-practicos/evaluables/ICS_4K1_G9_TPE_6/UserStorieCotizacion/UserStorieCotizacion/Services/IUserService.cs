using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Models.Request;

namespace UserStorieCotizacion.Services


{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest model);

    }
}

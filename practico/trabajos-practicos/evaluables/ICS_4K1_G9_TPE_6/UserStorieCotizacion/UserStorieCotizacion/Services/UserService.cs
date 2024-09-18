using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Models.Common;
using UserStorieCotizacion.Models.Request;
using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Tools;


namespace UserStorieCotizacion.Services
{
    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;


        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

       
        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userresponse = new UserResponse();

            using (var db = new ISWContext())
            {
                string hashedPassword = Encrypt.GetSHA256(model.Password);

                // Ahora, comparas la contraseña encriptada almacenada en la base de datos con la contraseña ingresada
                var usuario = db.Usuarios
                    .FirstOrDefault(d => d.Email == model.Email && d.Password == hashedPassword);

                if (usuario == null)
                {
                    // No se encontró el usuario o las contraseñas no coinciden
                    return null;
                }

                // Si llegas aquí, el usuario fue encontrado y las contraseñas coinciden
                userresponse.Id = usuario.Id;  // Agrega el Id al objeto UserResponse
                userresponse.Email = usuario.Email;
                userresponse.Token = GetToken(usuario);
                userresponse.Nombre = usuario.Nombre;
            }

            return userresponse;
        }


        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, usuario.Email)
                    }
                    ),
                //Expires = DateTime.UtcNow.AddDays(60),
                
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

     
    }
}

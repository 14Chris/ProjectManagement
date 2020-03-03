using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using ProjectManagement.Api.MailUtilities;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Repositories;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly ITokenRepository _tokenRepository;

        public UserService(IUserRepository userRepository, ITokenRepository tokenRepository, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Method to create an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Responses.Response> CreateAsync(User user)
        {
            Responses.Response response;

            //Verify if any user has the same email than the parameter
            if (_userRepository.EmailExists(user.email))
            {
                return new Responses.ErrorResponse("EMAIL_ALREADY_REGISTRED");

            }

            //Verify that the password checked the requirements
            if (!PasswordUtilities.PasswordMatchRegex(user.password))
            {
                //return BadRequest("PASSWORD_TOO_WEAK");
                return new Responses.ErrorResponse("PASSWORD_TOO_WEAK");
            }

            //Hash the password of the user
            user.password = PasswordUtilities.HashPassword(user.password);
            user.active = false;

            User resUser = await _userRepository.CreateAsync(user);

            // Génération d'un token pour l'activation du compte valable pendant 2h
            var token = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret("tokenReset33-password&!")
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();

            //Ajout du token en base de données
            Token tokenActivation = new Token();
            tokenActivation.id_user = resUser.id;
            tokenActivation.type = TypeToken.AccountActivation;
            tokenActivation.token = token;

            Token resToken = await _tokenRepository.CreateAsync(tokenActivation);

            if (resToken.id > 0)
            {
                // Envoi d'un email avec un lien d'activation du compte
                await ((EmailSender)_emailSender).SendAccountActivationMailAsync(resUser, resToken.token);
            }

            return new Responses.SuccessResponse(resUser);
        }

        /// <summary>
        /// Method to get one user by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User GetById(int id)
        {
            return _userRepository.List().Where(x => x.id == id).SingleOrDefault();
        }

        /// <summary>
        /// Method to list all the users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> List()
        {
            return _userRepository.List().ToList();
        }

        /// <summary>
        /// Method to update data of an user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Responses.Response> UpdateAsync(UserProfileModel model)
        {
            User user = GetById(model.id);

            user.first_name = model.first_name;
            user.last_name = model.last_name;
            if (model.profile_picture != null && model.profile_picture.Length > 0)
                user.profile_picture = Convert.FromBase64String(model.profile_picture);

            bool res = await _userRepository.UpdateAsync(user);

            if (res)
            {
                return new Responses.SuccessResponse(null);
            }
            else
            {
                return new Responses.ErrorResponse("Error");
            }
        }

        /// <summary>
        /// Method to update the password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Responses.Response> UpdatePasswordAsync(ModifyPasswordModel model)
        {
            User user = GetById(model.id);

            if (user.password != PasswordUtilities.HashPassword(model.oldPassword))
            {
                return new Responses.ErrorResponse("INVALID_OLD_PASSWORD");
            }

            if (!PasswordUtilities.PasswordMatchRegex(model.newPassword))
            {
                return new Responses.ErrorResponse("NEW_PASSWORD_TOO_WEAK");
            }

            user.password = PasswordUtilities.HashPassword(model.newPassword);

            bool res = await _userRepository.UpdateAsync(user);

            if (res)
            {
                return new Responses.SuccessResponse(null);
            }
            else
            {
                return new Responses.ErrorResponse("Error");
            }
        }

        /// <summary>
        /// Method to verify if an email is already taken
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EmailExists(string email)
        {
            return _userRepository.EmailExists(email);
        }

        /// <summary>
        /// Method to activate an account with the token send
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool ActivateAccount(string token)
        {
            //Récupération du dernier token (par l'id)
            Token tokenActivation =_tokenRepository.List()
                .Where(x => x.token == token && x.type == TypeToken.AccountActivation).OrderByDescending(x => x.id).FirstOrDefault();

            if (tokenActivation == null)
                return false;

            string secret = "tokenReset33-password&!";

            //Vérification du token (expiration, clé, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                _tokenRepository.DeleteAsync(tokenActivation);
                return false;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }

            //Si token ok, on active le compte
            User user = GetById(tokenActivation.id_user);
            user.active = true;

            //On supprime le token d'activation
            _tokenRepository.DeleteAsync(tokenActivation);

            _userRepository.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Method to modify password with token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Responses.Response> ModifyPasswordAsync(string token, string password)
        {
            //Si le token est bien valide pour changer le mot de passe
            Token tokenReset = _tokenRepository.List()
                .Where(x => x.token == token && x.type == TypeToken.ForgotPassword).OrderByDescending(x => x.id).FirstOrDefault();

            if (tokenReset == null)
            {
                return new Responses.ErrorResponse("BAD_TOKEN");
            }

            string secret = "tokenReset33-password&!";

            //Vérification du token (expiration, clé, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                await _tokenRepository.DeleteAsync(tokenReset);
                return new Responses.ErrorResponse("Error");
            }
            catch (SignatureVerificationException)
            {
                return new Responses.ErrorResponse("Error");
            }

            User user = GetById(tokenReset.id_user);

            if (user == null)
            {
                return new Responses.ErrorResponse("NO_USER");
            }

            //test si le nouveau mot de passe respect les conditions (regex)
            if (!PasswordUtilities.PasswordMatchRegex(password))
            {
                return new Responses.ErrorResponse("PASSWORD_TOO_WEAK");
            }

            user.password = PasswordUtilities.HashPassword(password);

            bool res = await _userRepository.UpdateAsync(user);

            if (res)
            {
                return new Responses.SuccessResponse(null);
            }
            else
            {
                return new Responses.ErrorResponse("Error");
            }
        }

        /// <summary>
        /// Method to validate the token send to reset passsword
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Responses.Response> ValidateToken(string token)
        {
            //Si le token est bien valide pour changer le mot de passe
            Token tokenReset = _tokenRepository.List()
                .Where(x => x.token == token && x.type == TypeToken.ForgotPassword).OrderByDescending(x => x.id).FirstOrDefault();

            if (tokenReset == null)
            {
                return new Responses.ErrorResponse("BAD_TOKEN");
            }

            string secret = "tokenReset33-password&!";

            //Vérification du token (expiration, clé, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                _tokenRepository.DeleteAsync(tokenReset);

                return new Responses.ErrorResponse("TOKEN_EXPIRED");
            }
            catch (SignatureVerificationException)
            {
                return new Responses.ErrorResponse("BAD_SIGNATURE");
            }

            //Si Ok
            return new Responses.SuccessResponse(null);
          
        }


        /// <summary>
        /// Method to create and save token for password reset
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> ForgotPassword(string email)
        {
            //Récupération de l'utilisateur par email
            User user = _userRepository.List().Where(x => x.email == email).SingleOrDefault();

            //Si pas d'utilisateur trouvé
            if (user == null)
                return false;

            //Génération d'un token pour réinitialiser le mot de passe de l'utilisateur
            var token = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret("tokenReset33-password&!")
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(6).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();

            //Ajout du token en base de données
            Token tokenActivation = new Token();
            tokenActivation.id_user = user.id;
            tokenActivation.type = TypeToken.ForgotPassword;
            tokenActivation.token = token;
            
            Token resToken = await _tokenRepository.CreateAsync(tokenActivation);

            if (tokenActivation.id > 0)
            {
                //Envoi d'un email contenant un lien pour réinitialier le mot de passe
                ((EmailSender)_emailSender).SendResetPasswordMailAsync(user, resToken.token);
            }

            return true;
        }
    }
}

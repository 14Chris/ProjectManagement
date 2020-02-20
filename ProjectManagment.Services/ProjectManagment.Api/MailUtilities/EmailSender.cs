using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using ProjectManagment.Api.MailUtilities.ViewModels;
using ProjectManagment.Models.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ProjectManagment.Api.MailUtilities
{
    public interface IEmailSender
    {
        Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _email;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IFluentEmail email, ILogger<EmailSender> logger)
        {
            _email = email;
            _logger = logger;
        }

        public async Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            try
            {
                
                var result = await _email.To(to)
                .Subject(subject)
                .UsingTemplateFromFile(string.Format("/app/publish/MailUtility/Templates/{0}.cshtml", template), model)
                .SendAsync();

                if (!result.Successful)
                {
                    _logger.LogError("Failed to send an email.\n{Errors}", string.Join(Environment.NewLine, result.ErrorMessages));
                }
                else
                {
                    _logger.LogError("Email sent");
                }

                return result.Successful;
            }
            catch (Exception ex)
            {
                //Directory.GetDirectories(currentDirectory+"/publish").ToList().ForEach(x => _logger.LogError(x));
                _logger.LogError("Failed to send an email.\n{Errors}", string.Join(Environment.NewLine, ex.Message));
                return false;
            }
        }

        private static ExpandoObject ToExpando(object model)
        {
            if (model is ExpandoObject exp)
            {
                return exp;
            }

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyDescriptor in model.GetType().GetTypeInfo().GetProperties())
            {
                var obj = propertyDescriptor.GetValue(model);

                if (obj != null && IsAnonymousType(obj.GetType()))
                {
                    obj = ToExpando(obj);
                }

                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }

        private static bool IsAnonymousType(Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
                .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
                .Any();

            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

        public async Task<bool> SendAccountActivationMailAsync(User user, string token)
        {
            AccountActivationViewModel model = new AccountActivationViewModel();

            model.UserName = user.first_name + " " + user.last_name;
            model.ActivateLink = "http://localhost:8000/users/account_activation/" + token;

            return await SendUsingTemplate(user.email, "Account activation", EmailTemplate.ActivateAccount, model);
        }

        public async Task<bool> SendResetPasswordMailAsync(User user, string token)
        {
            AccountActivationViewModel model = new AccountActivationViewModel();

            model.UserName = user.first_name + " " + user.last_name;
            model.ActivateLink = "http://localhost:8080/reset_password?token=" + token;

            return await SendUsingTemplate(user.email, "Reset password", EmailTemplate.ResetPassword, model);
        }
    }

    public enum EmailTemplate
    {
        ActivateAccount,
        ResetPassword
    }
}

using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.OptionsModel;
using System;

namespace SoapApi
{
    public static class MvcSoapBuilderExtensions
    {
        /// <summary>
        /// Adds the Soap formatters to MVC.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddSoapSerializerFormatters(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            AddXmlSerializerFormatterServices(builder.Services);
            return builder;
        }

        // Internal for testing.
        internal static void AddXmlSerializerFormatterServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcSoapSerializerMvcOptionsSetup>());
        }
    }
}

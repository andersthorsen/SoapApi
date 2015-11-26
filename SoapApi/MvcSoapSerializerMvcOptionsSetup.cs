using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;

namespace SoapApi
{
    /// <summary>
    /// A <see cref="ConfigureOptions{TOptions}"/> implementation which will add the
    /// data contract serializer formatters to <see cref="MvcOptions"/>.
    /// </summary>
    public class MvcSoapSerializerMvcOptionsSetup : ConfigureOptions<MvcOptions>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MvcXmlDataContractSerializerMvcOptionsSetup"/>.
        /// </summary>
        public MvcSoapSerializerMvcOptionsSetup()
            : base(ConfigureMvc)
        {
        }

        /// <summary>
        /// Adds the data contract serializer formatters to <see cref="MvcOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="MvcOptions"/>.</param>
        public static void ConfigureMvc(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new SoapMetadataProvider());

            options.OutputFormatters.Add(new Soap11OutputFormatter());
            options.OutputFormatters.Add(new Soap12OutputFormatter());

            options.InputFormatters.Add(new Soap11InputFormatter());
            options.InputFormatters.Add(new Soap12InputFormatter());

            options.ValidationExcludeFilters.Add(typeFullName: "System.Xml.Linq.XObject");
            options.ValidationExcludeFilters.Add(typeFullName: "System.Xml.XmlNode");
        }
    }
}
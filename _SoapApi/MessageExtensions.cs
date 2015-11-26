using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;

namespace SoapApi
{
    public static class MessageExtensions
    {
        public static object GetBody(this Message msg, Type type)
        {
            var typeOfMessage = msg.GetType();
            var genericMethod = typeOfMessage.GetMethod("GetBody", new[] { typeof(XmlObjectSerializer) });

            var method = genericMethod.MakeGenericMethod(type);

            var serializer = new DataContractSerializer(type);

            var obj = method.Invoke(msg, new object[] { serializer });

            return obj;
        }
    }
}
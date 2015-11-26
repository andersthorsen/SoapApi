//using System;
//using System.Linq;

//namespace SoapApi
//{
//    public class SoapActionParser
//    {
//        private readonly string[] _path;

//        public SoapActionParser(Uri actionUri)
//        {
//            var path = actionUri.GetLeftPart(UriPartial.Path);

//            _path = path.Split('/');
//        }

//        public string GetActionName()
//        {
//            return _path.Last();
//        }

//        public string GetNamespaceUri()
//        {
//            var itemsToUse = _path.Count() - 1;

//            return string.Join("/", _path, 0, itemsToUse);
//        }
//    }
//}
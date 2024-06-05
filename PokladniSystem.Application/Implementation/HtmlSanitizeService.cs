using Ganss.Xss;
using PokladniSystem.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokladniSystem.Application.Implementation
{
    public class HtmlSanitizerService : IHtmlSanitizerService
    {
        HtmlSanitizer _htmlSanitizer;

        public HtmlSanitizerService()
        {
            _htmlSanitizer = new HtmlSanitizer();
        }

        public T Sanitize<T>(T model) where T : class
        {
            if (model == null)
                return null;

            var type = model.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(model);
                    var sanitizedValue = _htmlSanitizer.Sanitize(value);
                    property.SetValue(model, sanitizedValue);
                }
                else if (property.PropertyType.IsClass)
                {
                    var nestedObject = property.GetValue(model);
                    Sanitize(nestedObject as object);
                }
            }

            return model;
        }
    }
}

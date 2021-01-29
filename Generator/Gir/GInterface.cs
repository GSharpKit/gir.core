﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Gir
{
    public class GInterface
    {
        #region Properties

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlElement("doc")]
        public GDoc? Doc { get; set; }

        [XmlAttribute("type", Namespace = "http://www.gtk.org/introspection/c/1.0")]
        public string? Type { get; set; }

        [XmlAttribute("type-name", Namespace = "http://www.gtk.org/introspection/glib/1.0")]
        public string? TypeName { get; set; }

        [XmlElement("method")]
        public List<GMethod> Methods { get; set; } = default!;

        [XmlAttribute("get-type", Namespace = "http://www.gtk.org/introspection/glib/1.0")]
        public string? GetTypeFunction { get; set; }

        [XmlElement("property")]
        public List<GProperty> Properties { get; set; } = default!;

        public virtual IEnumerable<GMethod> AllMethods
            => Methods.Where(MethodHasNoVariadicParameter);

        private static bool MethodHasNoVariadicParameter(GMethod method)
            => !method.HasVariadicParameter();

        #endregion
    }
}

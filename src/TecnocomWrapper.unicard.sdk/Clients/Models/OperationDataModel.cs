using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecnocomWrapper.unicard.sdk.Models
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3056.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://mensajeria/xsd")]
    public partial class OperationDataModel
    {

        private bool avanzarField;

        private bool avanzarFieldSpecified;

        private string canalField;

        private string claveFinField;

        private string claveInicioField;

        private string entidadField;

        private string idiomaField;

        private string pantPaginaField;

        private bool retrocederField;

        private bool retrocederFieldSpecified;

        private string securityHashField;

        /// <remarks/>
        public bool avanzar
        {
            get { return this.avanzarField; }
            set { this.avanzarField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool avanzarSpecified
        {
            get { return this.avanzarFieldSpecified; }
            set { this.avanzarFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string canal
        {
            get { return this.canalField; }
            set { this.canalField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string claveFin
        {
            get { return this.claveFinField; }
            set { this.claveFinField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string claveInicio
        {
            get { return this.claveInicioField; }
            set { this.claveInicioField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string entidad
        {
            get { return this.entidadField; }
            set { this.entidadField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string idioma
        {
            get { return this.idiomaField; }
            set { this.idiomaField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string pantPagina
        {
            get { return this.pantPaginaField; }
            set { this.pantPaginaField = value; }
        }

        /// <remarks/>
        public bool retroceder
        {
            get { return this.retrocederField; }
            set { this.retrocederField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool retrocederSpecified
        {
            get { return this.retrocederFieldSpecified; }
            set { this.retrocederFieldSpecified = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string securityHash
        {
            get { return this.securityHashField; }
            set { this.securityHashField = value; }
        }
    }
}
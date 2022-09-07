using System;
using System.Collections;
using System.ComponentModel;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public IDictionary Dictionary { get; }
        /// <summary>
        /// 
        /// </summary>
        public DictionaryPropertyGridAdapter(IDictionary d)
        {
            Dictionary = d;
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return Dictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(Array.Empty<Attribute>());
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            ArrayList properties = new();
            foreach (DictionaryEntry e in Dictionary)
            {
                properties.Add(new DictionaryPropertyDescriptor(Dictionary, e.Key));
            }

            PropertyDescriptor[] props =
                (PropertyDescriptor[])properties.ToArray(typeof(PropertyDescriptor));

            return new PropertyDescriptorCollection(props);
        }
    }

    internal class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        private readonly IDictionary _dictionary;
        private readonly object _key;

        internal DictionaryPropertyDescriptor(IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType => _dictionary[_key].GetType();

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly => false;

        public override Type ComponentType => null;

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
    internal class DefaultValue
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Browsable(true)]
        [ReadOnly(false)]
        [DisplayName("Value")]
        public bool Value { get; set; }
    }
}


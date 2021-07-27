using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Models
{
    public class WeatherDataModel //: DynamicObject
    {
        // Captures all of the weather data fields and values -- temp, humidity, wind speed, etc
        private Dictionary<string, object> _fields = new Dictionary<string, object>();

        public string StationName { get; set; }

        public string ObservationDate { get; set; }


        public object this[string key] 
        { 
            get => (_fields.ContainsKey(key)) ? _fields[key] : null; 
            set => _fields[key] = value; 
        }

        public ICollection<string> FieldNames => _fields.Keys;

        public int PropertyCount => _fields.Count;


        public void Add(string key, object value)
        {
            _fields.Add(key, value);
        }


        public bool ContainsKey(string key)
        {
            return _fields.ContainsKey(key);
        }


        //public override bool TryGetMember(GetMemberBinder binder, out object result)
        //{
        //    // If the property name is found in a dictionary,
        //    // set the result parameter to the property value and return true.
        //    // Otherwise, return false.
        //    return _measurements.TryGetValue(binder.Name, out result);
        //}

        //// If you try to set a value of a property that is
        //// not defined in the class, this method is called.
        //public override bool TrySetMember(SetMemberBinder binder, object value)
        //{
        //    _measurements[binder.Name] = value;

        //    // You can always add a value to a dictionary,
        //    // so this method always returns true.
        //    return true;
        //}

    }
}

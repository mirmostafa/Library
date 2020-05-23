#region Code Identifications

// Created on     2018/07/25
// Last update on 2018/08/12 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TestConsole45
{
    partial class App
    {
        protected override void Execute()
        {
            var xml =
                @"
<breakfast_menu>
<food>
    <name>Belgian Waffles</name>
    <price>$5.95</price>
    <description>
   Two of our famous Belgian Waffles with plenty of real maple syrup
   </description>
    <calories>650</calories>
</food>
<food>
    <name>Strawberry Belgian Waffles</name>
    <price>$7.95</price>
    <description>
    Light Belgian waffles covered with strawberries and whipped cream
    </description>
    <calories>900</calories>
</food>
<food>
    <name>Berry-Berry Belgian Waffles</name>
    <price>$8.95</price>
    <description>
    Belgian waffles covered with assorted fresh berries and whipped cream
    </description>
    <calories>900</calories>
</food>
<food>
    <name>French Toast</name>
    <price>$4.50</price>
    <description>
    Thick slices made from our homemade sourdough bread
    </description>
    <calories>600</calories>
</food>
<food>
    <name>Homestyle Breakfast</name>
    <price>$6.95</price>
    <description>
    Two eggs, bacon or sausage, toast, and our ever-popular hash browns
    </description>
    <calories>950</calories>
</food>
</breakfast_menu>

";
            var food = SerializationHelper.DeserializeText<Food>(xml);
        }

        #region Nested

        [XmlRoot(ElementName ="food")]
        public class Food {
            [XmlElement(ElementName ="name")]
            public string Name { get; set; }
            [XmlElement(ElementName ="price")]
            public string Price { get; set; }
            [XmlElement(ElementName ="description")]
            public string Description { get; set; }
            [XmlElement(ElementName ="calories")]
            public string Calories { get; set; }
        }

        [XmlRoot(ElementName ="breakfast_menu")]
        public class Breakfast_menu {
            [XmlElement(ElementName ="food")]
            public List<Food> Food { get; set; }
        }

        public static class SerializationHelper
        {
            public static TResult DeserializeText<TResult>(string xml)
            {
                if (xml == null)
                    throw new ArgumentNullException(nameof(xml));

                var serializer = new XmlSerializer(typeof(TResult));

                using (var reader = new StringReader(xml)) return (TResult) serializer.Deserialize(reader);
            }

            public static string SerializeToText(object value, params Type[] extraTypes)
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                var serializer = new XmlSerializer(value.GetType(), extraTypes);

                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, value);
                    return writer.ToString();
                }
            }
        }

        #endregion

        //private static void NewMethod()
        //{
        //    var cs = @"Data Source=192.168.88.31;Initial Catalog=SatrapS2;Integrated Security=True";
        //    var db = Database.GetDatabase(cs);
        //    db.Logger.Out = Console.Out;
        //    foreach (var sp in db.StoredProcedures) $"{sp.Schema}.{sp.Name}({sp.Params.Select(p => $"{p.SqlDataType} {p.Name}")})".WriteLine();
        //}
    }

    public class Person
    {
        public int    Age  { get; set; }
        public string Name { get; set; }
    }
}
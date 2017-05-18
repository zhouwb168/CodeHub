using System.Text;

using System.IO;
using System.Runtime.Serialization.Json;

namespace Wodeyun.Gf.Entities
{
    public class Helper
    {
        public static Entity GetEntity(bool success, string message, object value)
        {
            SimpleProperty property1 = new SimpleProperty("Success", typeof(bool));
            SimpleProperty property2 = new SimpleProperty("Message", typeof(string));
            SimpleProperty property3 = new SimpleProperty("Value", typeof(object));
            Entity entity = new Entity(new PropertyCollection() { property1, property2, property3 });

            entity.SetValue(property1, success);
            entity.SetValue(property2, message);
            entity.SetValue(property3, value);

            return entity;
        }

        public static Entity GetEntity(bool success, string message)
        {
            SimpleProperty property1 = new SimpleProperty("Success", typeof(bool));
            SimpleProperty property2 = new SimpleProperty("Message", typeof(string));
            Entity entity = new Entity(new PropertyCollection() { property1, property2 });

            entity.SetValue(property1, success);
            entity.SetValue(property2, message);

            return entity;
        }

        public static Entity Deserialize(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Entity));

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (Entity)serializer.ReadObject(stream);
            }
        }
    }
}

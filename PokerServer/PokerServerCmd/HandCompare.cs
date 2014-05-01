using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace PokerServerCmd
{

    [DataContract]
    public class Hand
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public List<string> hand { get; set; }
    }


    [DataContract]
    public class Hands
    {
        [DataMember(Name = "hands")]
        public List<Hand> hands { get; set; }

        public static Hands JsonDeserialize(string jsonString)
        {
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(jsonString));

            DataContractJsonSerializer serialize = new DataContractJsonSerializer(typeof(Hands));


            return (Hands)serialize.ReadObject(stream);           
        }



        public static Hands XmlDeserialize(string xmlString)
        {

            UTF8Encoding encoding = new UTF8Encoding();
            MemoryStream stream = new MemoryStream(encoding.GetBytes(xmlString));


            DataContractSerializer serialize = new DataContractSerializer(typeof(Hands));


            return (Hands)serialize.ReadObject(stream);
        }

    }

   

   


}

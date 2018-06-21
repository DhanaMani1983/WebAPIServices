namespace Saga.Gmd.WebApiServices.Models
{
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool HasValue => !string.IsNullOrEmpty(Value);
        public int?  IntValue {
            get
            {
                int retval;
                return (int.TryParse(Value, out retval)) ?  retval : new int?();
            } 
            

        }

    }
}

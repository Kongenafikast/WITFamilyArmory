namespace WITFamilyArmory
{
    public class StampHolder
    {
        public string Stamp {  get; set; }

       public StampHolder()
        {
            Stamp = Guid.NewGuid().ToString();
        }
    }
}

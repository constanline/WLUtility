namespace WLUtility.Model
{
    internal class Pet
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        public BagItem[] Equips { get; } = new BagItem[7];

        public Pet()
        {
            for (var i = 1; i <= 6; i++)
            {
                Equips[i] = new BagItem();
            }
        }
    }
}
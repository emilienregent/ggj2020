public class ResourcesModel
{
    private static int _stock = 0;

    public static int Stock { get => _stock; set => _stock = value; }

    public static int getStock() {
        return Stock;
    }

    public static int Add(int resourcesNb) {
        Stock += resourcesNb;
        return Stock;
    }

    public static int Use(int resourcesNb) {
        if (resourcesNb > Stock) {
            return -1;
        }
        Stock -= resourcesNb;
        return Stock;
    }
}

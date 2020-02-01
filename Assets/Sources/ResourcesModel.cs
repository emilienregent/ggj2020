public class ResourcesModel
{
    private static int _stock = 0;

    public static int Add(int resourcesNb) {
        _stock += resourcesNb;
        return _stock;
    }

    public static int Use(int resourcesNb) {
        if (resourcesNb > _stock) {
            return -1;
        }
        _stock -= resourcesNb;
        return _stock;    
    }
}

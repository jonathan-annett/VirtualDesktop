static int JSONListing()
{
    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

    int desktopCount = VirtualDesktop.Desktop.Count;
    int visibleDesktop = VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current);

    Console.Write("{");
    Console.Write("\"count\":" + desktopCount + ",");
    Console.Write("\"desktops\":[");
    string comma = "";
    for (int i = 0; i < desktopCount; i++)
    {
        Console.Write(comma + "{");
        Console.Write(
            "\"name\":" + serializer.Serialize(VirtualDesktop.Desktop.DesktopNameFromIndex(i)) + ","
        );
        Console.Write("\"visible\":");

        if (i != visibleDesktop)
            Console.Write("false,");
        else
            Console.Write("true,");

        Console.Write("\"wallpaper\":");

        //if (string.IsNullOrEmpty(VirtualDesktop.Desktop.DesktopWallpaperFromIndex(i)))
        Console.Write("null");
        //else
        //	Console.Write( serializer.Serialize(VirtualDesktop.Desktop.DesktopWallpaperFromIndex(i)) );
        Console.Write("}");
        comma = ",";
    }
    Console.Write("]");
    Console.WriteLine("}");
    return 0;
}

static int InteractiveMode()
{
    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

    string argstr = "";
    bool echo = true;

    int lastDT = VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current);
    bool interactive = true;

    sListDesktopPrefix = "[";
    sListDesktopLine1Prefix = "";
    sListDesktopLine2PlusPrefix = ",";
    sListDesktopSuffix = "]";

    try
    {
        bool test = Console.KeyAvailable;
    }
    catch
    {
        interactive = false;
    }

    verbose = false;
    while (true)
    {
        bool cmdReady = false;
        bool idle = true;

        if (interactive)
        {
            if (Console.KeyAvailable)
            {
                idle = false;

                var cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape)
                {
                    break;
                }

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (argstr != "")
                    {
                        argstr = argstr.Substring(0, argstr.Length - 1);
                    }
                    if (echo)
                    {
                        Console.Write(cki.KeyChar);
                        Console.Write(" ");
                        Console.Write(cki.KeyChar);
                    }
                    continue;
                }

                if (cki.Key == ConsoleKey.Enter)
                {
                    cmdReady = true;
                }
                else
                {
                    argstr += cki.KeyChar;
                    if (echo)
                    {
                        Console.Write(cki.KeyChar);
                    }
                    continue;
                }
            }
        }
        else
        {
            try
            {
                string temp = Reader.ReadLine(50);
                cmdReady = true;
                idle = false;
                argstr = temp;
            }
            catch (TimeoutException) { }
        }

        if (idle)
        {
            int thisDT = VirtualDesktop.Desktop.FromDesktop(VirtualDesktop.Desktop.Current);
            if (lastDT != thisDT)
            {
                Console.WriteLine(
                    "{\"visibleIndex\":"
                        + thisDT
                        + ",\"visible\":"
                        + serializer.Serialize(VirtualDesktop.Desktop.DesktopNameFromIndex(thisDT))
                        + ",\"count\":"
                        + VirtualDesktop.Desktop.Count
                        + "}"
                );
                lastDT = thisDT;
            }
            else
            {
                System.Threading.Thread.Sleep(5);
            }
        }
        else
        {
            if (cmdReady)
            {
                if (argstr == "")
                {
                    continue;
                }

                if (echo && interactive)
                {
                    Console.WriteLine("");
                }

                string[] splits = argstr.Split(' ');
                argstr = "";
                string token = splits[0].ToUpper();
                if (token.Length > 1 && token.Substring(0, 1) == "/")
                {
                    token = token.Substring(1);
                }
                switch (token)
                {
                    case "INTERACTIVE": // prevent recursion
                    case "INT": // prevent recursion

                    // various commands not valid in interactive mode
                    case "QUIET":
                    case "Q":
                    case "VERBOSE":
                    case "V":
                    case "BREAK":
                    case "B":
                    case "CONTINUE":
                    case "CO":
                    case "WAITKEY":
                    case "WK":

                        Console.WriteLine(
                            "\n{\"error\":"
                                + serializer.Serialize("Invalid Command:" + splits[0])
                                + "}"
                        );
                        continue;

                    case "NAMES":

                        int dtc = VirtualDesktop.Desktop.Count;
                        string json = "[";
                        string cma = "";
                        for (int i = 0; i < dtc; i++)
                        {
                            json +=
                                cma
                                + serializer.Serialize(
                                    VirtualDesktop.Desktop.DesktopNameFromIndex(i)
                                );
                            cma = ",";
                        }
                        json += "]";
                        Console.WriteLine(json);
                        continue;

                    case "LEFT":
                    case "P":
                    case "PREVIOUS":

                        if (lastDT == 0)
                        {
                            token = "GCD";
                        }
                        else
                        {
                            token = "L";
                        }
                        splits = token.Split(' ');
                        lastDT = -1;
                        break;

                    case "L":

                        if (lastDT == 0)
                        {
                            token = "GCD";
                            splits = token.Split(' ');
                        }
                        lastDT = -1;
                        break;

                    case "RIGHT":
                    case "NEXT":

                        if (lastDT == VirtualDesktop.Desktop.Count - 1)
                        {
                            token = "GCD";
                        }
                        else
                        {
                            token = "RI";
                        }
                        splits = token.Split(' ');
                        lastDT = -1;
                        break;

                    case "RI":

                        if (lastDT == VirtualDesktop.Desktop.Count - 1)
                        {
                            token = "GCD";
                            splits = token.Split(' ');
                        }
                        lastDT = -1;
                        break;

                    case "GCD":
                    case "GETCURRENTDESKTOP":

                        lastDT = -1; // force a reporting of the desktop

                        break;
                }

                if (
                    ((token.Length >= 7) && (token.Substring(0, 7) == "SWITCH:"))
                    || ((token.Length >= 2) && (token.Substring(0, 2) == "S:"))
                )
                {
                    if (splits.Length > 1)
                    {
                        int ix = 1;
                        while (ix < splits.Length)
                        {
                            token = token + " " + splits[ix];
                            ix++;
                        }
                        splits = "x".Split(' ');
                        splits[0] = token;
                    }
                    lastDT = -1; // force a reporting of the desktop, even if the switched to desktop is the same as the current desktop
                }

                Main(splits);

                switch (token)
                {
                    case "NEW":
                    case "N":
                        Main(("S:" + rc).Split(' '));
                        break;
                }

                Console.Out.Flush();
            }
        }
    }

    sListDesktopPrefix = "";
    sListDesktopLine1Prefix = "";
    sListDesktopLine2PlusPrefix = "\n";
    sListDesktopSuffix = "";

    return 0;
}

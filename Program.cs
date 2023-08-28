// See https://aka.ms/new-console-template for more information
using EventConsolev2;
using System.Text;
List<LineOfEvent> ListOfEvents = new();
string text;
bool InsideEvent = false;
string line;
string EventNum = "";
Char TypeFunction = 'a';
string Function = "";
int[] FunctionParams = { };
string ParamAsString = "";
using (var streamReader = new StreamReader(@"C:\server\arcanine\map\3.evt", Encoding.UTF8))
{
    text = streamReader.ReadToEnd();
}

StringReader ReaderOfEvents = new(text);
while ((line = ReaderOfEvents.ReadLine()) != null)
{
    if (InsideEvent == false)
    {
        if (line.StartsWith("EVENT "))
        {
            InsideEvent = true;
            EventNum = line.Split(' ').Skip(1).FirstOrDefault();
            LineOfEvent LineEvent = new();
            LineEvent.Function = "EVENT " + EventNum;
            ListOfEvents.Add(LineEvent);
        }
    }
    if (InsideEvent == true && (line.StartsWith("A") || (line.StartsWith("O") || line.StartsWith("E ")))) // Watch the trailing space of the startswith("E_") its needed.
    {
        LineOfEvent LineEvent = new();
        TypeFunction = line[0];
        Function = line.Split(' ').Skip(1).FirstOrDefault();
        FunctionParams = line.Split(' ').Skip(2).Select(n => Convert.ToInt32(n)).ToArray();
        
        LineEvent.ID = EventNum;
        LineEvent.TypeFunction = TypeFunction;
        LineEvent.Function = Function;
        LineEvent.FunctionParams = FunctionParams;
        ListOfEvents.Add(LineEvent);
    }


    if (line == "END")
    {
        InsideEvent = false;
        LineOfEvent LineEvent = new();
        LineEvent.Function = "END";
        ListOfEvents.Add(LineEvent);
    }
}
ReaderOfEvents.Dispose();
Console.WriteLine("Enter event number to find");
string SearchEvent = Console.ReadLine();

Console.WriteLine("EVENT " + SearchEvent + "\n\n");
foreach (LineOfEvent item in ListOfEvents.Where(x => x.ID == SearchEvent))
{
    ParamAsString = "";
    foreach (int param in item.FunctionParams)
    {
        ParamAsString += param + " ";
    }
    ParamAsString.Trim();
    Console.WriteLine(item.TypeFunction + " " + item.Function + " " + ParamAsString.ToString());
}
Console.WriteLine("\r\n\r\nWant some output?");
string answer = Console.ReadLine();
//
//
//try and save some output
if (answer == "yes" || answer == "Yes")
{
    using (StreamWriter writer = new StreamWriter("C:\\server\\test.evt"))
    {
        foreach (LineOfEvent item in ListOfEvents)
        {

            if (item.Function.StartsWith("EVENT ") || item.Function.StartsWith("END"))
            {
                writer.WriteLine(item.Function);
                if (item.Function.StartsWith("EVENT "))
                {
                    Console.WriteLine("\r\n\r\n");
                }
            }
            else
            {
                ParamAsString = "";
                foreach (int param in item.FunctionParams)
                {
                    ParamAsString += param + " ";
                }
                ParamAsString.Trim();
                writer.WriteLine(item.TypeFunction + " " + item.Function + " " + ParamAsString.ToString());
            }
        }
    }
}